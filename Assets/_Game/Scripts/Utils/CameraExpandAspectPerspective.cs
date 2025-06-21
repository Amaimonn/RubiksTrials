using UnityEngine;


[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class CameraExpandAspectPerspective : MonoBehaviour
{
    [SerializeField] private float _targetAspectWidth = 9f;
    [SerializeField] private float _targetAspectHeight = 16f;
    [SerializeField] private float _baseFieldOfView = 60f;
    [SerializeField] private float _minFieldOfView = 30f;
    [SerializeField] private float _maxFieldOfView = 120f;

    private Camera _camera;
    private Vector2 _lastScreenSize = new();

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _camera.orthographic = false;
        UpdateCameraFOV(new Vector2(Screen.width, Screen.height));
    }

    private void Update()
    {
        var screenSize = new Vector2(Screen.width, Screen.height);
        if (_lastScreenSize != screenSize)
        {
            _lastScreenSize = screenSize;
            UpdateCameraFOV(screenSize);
        }
    }

    private void UpdateCameraFOV(Vector2 screenSize)
    {
        if (!(float.IsFinite(screenSize.x) && float.IsFinite(screenSize.y)))
            return;
        var targetAspect = _targetAspectWidth / _targetAspectHeight;
        var currentAspect = screenSize.x / screenSize.y;

        if (currentAspect > targetAspect)
        {
            _camera.fieldOfView = _baseFieldOfView;
        }
        else
        {
            var tanFOV = Mathf.Tan(_baseFieldOfView * 0.5f * Mathf.Deg2Rad);
            var targetTanFOV = tanFOV * (targetAspect / currentAspect);
            var targetFOV = 2f * Mathf.Atan(targetTanFOV) * Mathf.Rad2Deg;
            
            _camera.fieldOfView = Mathf.Clamp(targetFOV, _minFieldOfView, _maxFieldOfView);
        }
    }
}
