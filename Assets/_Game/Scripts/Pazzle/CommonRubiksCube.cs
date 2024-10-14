using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets._Game.Scripts;

public class CommonRubiksCube : MonoBehaviour
{
    public Vector3 CoreCenter { get => _core.GetComponent<Renderer>().bounds.center; }

    public readonly float RotationAngle = 90;
    public CubePart[] CubeParts => _cubeParts;
    
    [SerializeField] private CubePart[] _cubeParts;
    [SerializeField] private Transform _core;
    [SerializeField] private ParticleSystem _victoryParticles;
    [SerializeField] private ParticleSystem _loseParticles;
    // [SerializeField] private TMP_Text _stateText;
    //private bool _inited = false;
    
    public void UpdatePartsDirection()
    {
        foreach (var part in _cubeParts)
            part.UpdateDirection(CoreCenter);
    }

    public bool CheckIsSolved()
    {
        foreach (PartColors color in Enum.GetValues(typeof(PartColors)))
        {
            if (CheckOneFaceIsSolved(color))
            {
                continue;
            }

            return false;
        }
        return true;
    }

    public bool CheckAnyFaceIsSolved()
    {
        foreach (PartColors color in Enum.GetValues(typeof(PartColors)))
        {
            if (CheckOneFaceIsSolved(color))
            {
                return true;
            }
        }
        return false;
    }

    public int GetSolvedFacesCount()
    {
        var solvedSides = 0;
        foreach (PartColors color in Enum.GetValues(typeof(PartColors)))
        {
            if (CheckOneFaceIsSolved(color))
            {
                solvedSides++;
            }
        }
        return solvedSides;
    }

    public bool CheckOneFaceIsSolved(PartColors partColor)
    {
        var checkCubes = _cubeParts.Where(part => part.Colors.HasFlag(partColor));

        return CheckPartsHasCommonDirection(checkCubes);
    }

    public bool CheckPartsHasCommonDirection(IEnumerable<CubePart> checkCubes)
    {
        // общие направления частей цвета partColor (без учета поворота частей)
        var faceForward = checkCubes.Select(p => p.Direction).Aggregate((a, b) => a & b);

        if (faceForward == 0)
            return false;

        // var localDirection = CubePart.AxisFromPartDirection(faceForward);
        // проверка частей на правильный поворот по нормали к грани
        if(checkCubes.All(c => c.CompareDirections(faceForward) == checkCubes.First().CompareDirections(faceForward)))
            return true;

        return false;
    }
    // private void OnValidate()
    // {
    //     if (!_inited)
    //     {
    //         _cubeParts = GetComponentsInChildren<CubePart>();
    //         SetInitialPartsDirection();
    //         _inited = true;
    //     }
    // }
    // private void SetInitialPartsDirection()
    // {
    //     foreach (var cubePart in _cubeParts)
    //     {
    //         cubePart.SetInitialDirection(CoreCenter);
    //     }
    // }

    public bool CheckPartsHasCommonDirection(CubePart checkCube1, CubePart checkCube2)
    {
        // общие направления частей цвета partColor (без учета поворота частей)
        var faceForward = checkCube1.Direction & checkCube2.Direction;

        if (faceForward == 0)
            return false;

        // var localDirection = CubePart.AxisFromPartDirection(faceForward);
        // проверка частей на правильный поворот 
        if(checkCube1.CompareDirections(faceForward) == checkCube2.CompareDirections(faceForward))
            return true;

        return false;
    }

    // проверка что части имеют одинаковый поворот на общих направлениях
    public bool CheckPartsHasEqualDirections(IEnumerable<CubePart> checkParts)
    {
        // все общие направления частей цвета partColor (без учета поворота частей)
        var faceForward = checkParts.Select(p => p.Direction).Aggregate((a, b) => a & b);

        if (faceForward == 0)
            return false;

        foreach (PartDirection checkDirection in Enum.GetValues(typeof(PartDirection)))
        {
            if (!faceForward.HasFlag(checkDirection))
                continue;

            if(!checkParts.All(c => c.CompareDirections(checkDirection) == checkParts.First().CompareDirections(checkDirection)))
                return false;
        }

        return true;
    }

    public bool CheckEdgeIsSolved(PartColors color)
    {
        // получили все части с выбранным цветом
        var allEdgeParts = _cubeParts.Where(part => part.Colors.HasFlag(color));
        // получили все цвета рассматриваемых частей
        var checkColors = allEdgeParts.Select(part => part.Colors).Aggregate((a, b) => a | b );
        IEnumerable<CubePart> checkEdgeParts;
        foreach (PartColors checkColor in Enum.GetValues(typeof(PartColors)))
        {
            if (!checkColors.HasFlag(checkColor))
            {
                continue;
            }
            // все части рассматриваемой грани, имеющие цвет checkColor
            checkEdgeParts = allEdgeParts.Where(part => part.Colors.HasFlag(checkColor));
            // проверка, что одинаковые цвета (стороны) checkEdgeParts направлены одинаково   
            if (CheckPartsHasEqualDirections(checkEdgeParts))
            {
                continue;
            }
            return false;
        }

        return true;
    }

    private IEnumerator FlickerAnimation(IEnumerable<CubePart> cubeParts)
    {
        bool isVisible = false;
        for (var i = 0; i < 6; i++)
        {
            foreach (var part in cubeParts)
            {
                part.SetVisibility(isVisible);
            }
            isVisible = !isVisible;
            yield return new WaitForSeconds(0.06f);
        }
    }

    public void PlayOnSolvedEffects(bool isSolved)
    {
        ParticleSystem particleSystem;
        var soundPlayer = ServiceLocator.Current.Get<SoundPlayer>();

        if (isSolved)
        {
            particleSystem = Instantiate(_victoryParticles);
            soundPlayer.PlayVictory();
        } 
        else
        {
            particleSystem = Instantiate(_loseParticles);
            soundPlayer.PlayLoss();
        }

        var totalDuration = particleSystem.main.duration + particleSystem.main.startLifetime.constantMax * (1 + particleSystem.trails.lifetime.constantMax);
        Destroy(particleSystem.gameObject, totalDuration);
    }

    public IEnumerator MixCube(int swipesCount, Func<bool> endCondition, float speed = 200.0f, Action onFinishCallback = null)
    {
        if (_cubeParts.All(part => part.IsBusy == false))
        {       
            var soundPlayer = ServiceLocator.Current.Get<SoundPlayer>();

            _cubeParts.ToList().ForEach(part => part.IsBusy = true);

            do
            {
                yield return new WaitForSeconds(0.5f);

                for (var i = 0; i < swipesCount; i++)
                {
                    soundPlayer.PlaySwipe();
                    var angle = RotationAngle * (UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1);
                    Vector3 axis = UnityEngine.Random.Range(0, 3) switch
                    {
                        0 => Vector3.up,
                        1 => Vector3.right,
                        2 => Vector3.forward,
                        _ => Vector3.up
                    };
                    if (UnityEngine.Random.Range(0, 2) == 1)
                        axis = -axis;
                    var partDirection = _cubeParts.First().PartDirectionFromAxis(axis);
                    var cubesToSwipe = _cubeParts.Where(c => (partDirection & c.Direction) != 0);
                    yield return new SwipeLongCommand(cubesToSwipe, speed * (1 + i/5.0f), CoreCenter, axis, angle).Execute();
                }
            }
            while (endCondition.Invoke() && swipesCount > 0);

            _cubeParts.ToList().ForEach(part => part.IsBusy = false);
            onFinishCallback?.Invoke();
        }
    }
    
    // public void SetConfiguration(CommonRubiksCube cube)
    // {
    //     transform.SetPositionAndRotation(cube.transform.position, 
    //         cube.transform.rotation);
    //     var parts = GetComponentsInChildren<CubePart>();
    //     var presetParts = cube.GetComponentsInChildren<CubePart>();
    //     for (var i = 0; i < parts.Length; i++)
    //     {
    //         parts[i].transform.SetPositionAndRotation(presetParts[i].transform.position, presetParts[i].transform.rotation);
    //         parts[i].IsBusy = false;
    //     }
        
    //     UpdatePartsDirection();
    // }
}
