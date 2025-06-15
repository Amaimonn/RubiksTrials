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
        var targetAspect = _targetAspectWidth / _targetAspectHeight;
        var currentAspect = screenSize.x / screenSize.y;

        if (currentAspect > targetAspect) // Широкий экран - регулируем по высоте
        {
            _camera.fieldOfView = _baseFieldOfView;
        }
        else // Узкий экран - регулируем по ширине
        {
            // Для перспективной камеры нужно учитывать тангенсы углов
            float tanFOV = Mathf.Tan(_baseFieldOfView * 0.5f * Mathf.Deg2Rad);
            float targetTanFOV = tanFOV * (targetAspect / currentAspect);
            float targetFOV = 2f * Mathf.Atan(targetTanFOV) * Mathf.Rad2Deg;
            
            _camera.fieldOfView = Mathf.Clamp(targetFOV, _minFieldOfView, _maxFieldOfView);
        }
    }
}
