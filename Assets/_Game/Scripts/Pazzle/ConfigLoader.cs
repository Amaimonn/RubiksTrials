using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class ConfigLoader : MonoBehaviour
{
    [SerializeField] private CommonRubiksCube _rubiksCube;

    [ProButton]
    private void LoadPreset(CubePresetSO presetSO)
    {
        if (Application.isPlaying)
        {
            var parts = _rubiksCube.CubeParts;
            var presetParts = presetSO.Cube.GetComponentsInChildren<CubePart>();
            _rubiksCube.transform.rotation = presetSO.Cube.transform.rotation;
            var index = 0;
            foreach (var part in parts)
            {
                part.transform.SetLocalPositionAndRotation(presetParts[index].transform.localPosition, 
                    presetParts[index].transform.localRotation);
                part.Direction = presetParts[index].Direction;
                index++;
            }
        }
    }
}
