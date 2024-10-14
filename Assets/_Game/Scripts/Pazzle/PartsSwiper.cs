using System.Linq;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class PartsSwiper : MonoBehaviour
{
    [SerializeField] private CubePart[] _cubeParts;
    [SerializeField] private float _swipeAngle;
    [SerializeField] private Vector3 _relativeTo;

    private Vector3 GetAxisFromPartDirection(PartDirection partDirection)
    {
        return _cubeParts[0].AxisFromPartDirection(partDirection);
    }

    private PartDirection GetPartDirectionFromAxis(Vector3 axis)
    {
        return _cubeParts[0].PartDirectionFromAxis(axis);
    }

    [ProButton]
    private void Swipe(PartDirection partDirection, bool isClockwise)
    {
        if (_cubeParts.Length == 0)
            return;
        var cubesToSwipe = _cubeParts.Where(c => (partDirection & c.Direction) != 0);
        var sign = isClockwise ? 1 : -1;
        new RotateCommand(cubesToSwipe, sign * _swipeAngle, _relativeTo, 
            GetAxisFromPartDirection(partDirection)).Execute();
    }

    [ProButton]
    private void SwipeTop(bool isClockwise)
    {
        if (_cubeParts.Length == 0)
            return;
        var cubesToSwipe = _cubeParts.Where(c => c.Direction.HasFlag(PartDirection.Top));
        var sign = isClockwise ? 1 : -1;
        new RotateCommand(cubesToSwipe, sign * _swipeAngle, _relativeTo, 
            GetAxisFromPartDirection(PartDirection.Top)).Execute();
    }
    
    [ProButton]
    private void SwipeBottom(bool isClockwise)
    {
        if (_cubeParts.Length == 0)
            return;
        var cubesToSwipe = _cubeParts.Where(c => c.Direction.HasFlag(PartDirection.Bottom));
        var sign = isClockwise ? 1 : -1;
        new RotateCommand(cubesToSwipe, sign * _swipeAngle, _relativeTo, 
            GetAxisFromPartDirection(PartDirection.Bottom)).Execute();
    }
    
    [ProButton]
    private void SwipeNorth(bool isClockwise)
    {
        if (_cubeParts.Length == 0)
            return;
        var cubesToSwipe = _cubeParts.Where(c => c.Direction.HasFlag(PartDirection.North));
        var sign = isClockwise ? 1 : -1;
        new RotateCommand(cubesToSwipe, sign * _swipeAngle, _relativeTo, 
            GetAxisFromPartDirection(PartDirection.North)).Execute();
    }
    
    [ProButton]
    private void SwipeSouth(bool isClockwise)
    {
        if (_cubeParts.Length == 0)
            return;
        var cubesToSwipe = _cubeParts.Where(c => c.Direction.HasFlag(PartDirection.South));
        var sign = isClockwise ? 1 : -1;
        new RotateCommand(cubesToSwipe, sign * _swipeAngle, _relativeTo, 
            GetAxisFromPartDirection(PartDirection.South)).Execute();
    }
    
    [ProButton]
    private void SwipeEast(bool isClockwise)
    {
        if (_cubeParts.Length == 0)
            return;
        var cubesToSwipe = _cubeParts.Where(c => c.Direction.HasFlag(PartDirection.East));
        var sign = isClockwise ? 1 : -1;
        new RotateCommand(cubesToSwipe, sign * _swipeAngle, _relativeTo, 
            GetAxisFromPartDirection(PartDirection.East)).Execute();
    }
     
    [ProButton]   
    private void SwipeWest(bool isClockwise)
    {
        if (_cubeParts.Length == 0)
            return;
        var cubesToSwipe = _cubeParts.Where(c => c.Direction.HasFlag(PartDirection.West));
        var sign = isClockwise ? 1 : -1;
        new RotateCommand(cubesToSwipe, sign * _swipeAngle, _relativeTo, 
            GetAxisFromPartDirection(PartDirection.West)).Execute();
    }

    [ProButton]
    private void RandomMix(int swipesCount = 10)
    {
        if (_cubeParts.Length == 0)
            return;
        for (var i = 0; i < swipesCount; i++)
        {
            var angle = _swipeAngle * (Random.Range(0, 2) == 1 ? 1 : -1);
            Vector3 axis = Random.Range(0, 3) switch
            {
                0 => Vector3.up,
                1 => Vector3.right,
                2 => Vector3.forward,
                _ => Vector3.up
            };
            if (Random.Range(0, 2) == 1)
                axis = -axis;
            var partDirection = GetPartDirectionFromAxis(axis);
            var cubesToSwipe = _cubeParts.Where(c => (partDirection & c.Direction) != 0);
            new RotateCommand(cubesToSwipe, angle, _relativeTo, axis).Execute();
        }
    }
}
