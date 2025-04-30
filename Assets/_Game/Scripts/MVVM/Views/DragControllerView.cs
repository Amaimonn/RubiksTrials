using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.ViewModels;

namespace Assets._Game.Scripts.MVVM.Views
{
    public class DragControllerView : BaseScreen<DragControlViewModel>
    {
        [Header("Controller")]
        [SerializeField] private WorldDragController _worldDragControllerPrefab;

        [Space(10)]
        [SerializeField] private float _panelOffset = 0.1f;

        [Header("Input")]
        [SerializeField] private InputActionReference _holdClickReference;
        [SerializeField] private InputActionReference _clickPositionReference;
        
        private Camera _camera;
        private bool _isShown = false;
        private bool _isControlEnabled;
        private Vector2 _positionInput;
        private RaycastHit _readHit;
        private Vector3 _hitNormal;
        private Vector3 _readPartPosition;
        private CompositeDisposable _disposables;
        private WorldDragController _worldDragController;

        protected override void OnBind(DragControlViewModel viewModel)
        {
            _disposables = new()
            {
                viewModel.IsControlEnabled.Subscribe(e => {
                    if (!e)
                        HideArrows();
                    _isControlEnabled = e;
                })
            };
        }
        
#region MonoBehaviour
        private void Awake()
        {
            _camera = Camera.main;
            _worldDragController = Instantiate(_worldDragControllerPrefab);
            _worldDragController.Panel.SetActive(false);
        }

        private void OnEnable()
        {
            _holdClickReference.action.started += ShowArrows;
            _holdClickReference.action.canceled += HandleSwipe;
        }

        private void OnDisable()
        {
            _holdClickReference.action.started -= ShowArrows;
            _holdClickReference.action.canceled -= HandleSwipe;
        }

        private void OnDestroy()
        {
            if (_worldDragController != null)
                Destroy(_worldDragController);
            Dispose();
        }
#endregion

        private void ShowArrows(InputAction.CallbackContext context)
        {
            if(!_isControlEnabled)
                return;

            if(!_isShown)
            {
                _positionInput = _clickPositionReference.action.ReadValue<Vector2>();
                var ray = _camera.ScreenPointToRay(_positionInput);
                if (Physics.Raycast(ray, out _readHit, 100))
                {
                    if (_readHit.collider.gameObject.TryGetComponent<CubePart>(out var part))
                    {
                        if (part.IsBusy)
                            return;
                        
                        _hitNormal = _readHit.normal;//BoxIgnoreCornerNormal(_readHit);
                        Debug.Log(_hitNormal + " hit normal");

                        _readPartPosition = part.transform.position;
                        _isShown = true;
                        // Debug.Log($"LookRotation: {-_readHit.normal}, {part.transform.up}");
                        var panelUp = part.transform.up == _hitNormal ? part.transform.forward : part.transform.up;
                        _worldDragController.Panel.transform.SetPositionAndRotation(_readHit.point, Quaternion.LookRotation(-_hitNormal, panelUp));
                        _worldDragController.Panel.transform.position += _hitNormal * _panelOffset;
                        _worldDragController.Panel.SetActive(true);
                    }
                }
            }
        }
        
        // private Vector3 BoxIgnoreCornerNormal(RaycastHit hit)
        // {
        //     var part = hit.collider.gameObject;
        //     Vector3 localHitPoint = part.transform.InverseTransformPoint(hit.point);

        //     // Получаем размеры кубика
        //     Vector3 extents = hit.collider.bounds.extents;

        //     // Определяем нормаль в зависимости от того, какая координата локального попадания ближе к границе (по осям X, Y или Z)
        //     Vector3 localNormal = Vector3.zero;

        //     float maxDistance = Mathf.Max(Mathf.Abs(localHitPoint.x - extents.x), Mathf.Abs(localHitPoint.y - extents.y), 
        //         Mathf.Abs(localHitPoint.z - extents.z));

        //     if (Mathf.Abs(localHitPoint.x - extents.x) == maxDistance)
        //     {
        //         localNormal = new Vector3(Mathf.Sign(localHitPoint.x), 0, 0);
        //     }
        //     else if (Mathf.Abs(localHitPoint.y - extents.y) == maxDistance)
        //     {
        //         localNormal = new Vector3(0, Mathf.Sign(localHitPoint.y), 0);
        //     }
        //     else if (Mathf.Abs(localHitPoint.z - extents.z) == maxDistance)
        //     {
        //         localNormal = new Vector3(0, 0, Mathf.Sign(localHitPoint.z));
        //     }

        //     // Преобразуем нормаль обратно в мировые координаты
        //     Vector3 worldNormal = part.transform.TransformDirection(localNormal);
        //     return worldNormal;
        // }
        
        // only for cubes
        private Vector3 RoundVector(Vector3 vector)
        {
            return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
        }
        
        private void HandleSwipe(InputAction.CallbackContext context)
        {
            if(!_isControlEnabled)
                return;

            if (!_isShown)
                return;
                
            _positionInput = _clickPositionReference.action.ReadValue<Vector2>();
            bool isCanceled = false;

            var ray = _camera.ScreenPointToRay(_positionInput);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider == _worldDragController.CancelZone)
                {
                    isCanceled = true;
                }
            }

            if (!isCanceled)
            {
                var upDistance = ((_worldDragController.Panel.transform.position - _worldDragController.UpArrow.transform.position).normalized, ((Vector2)_camera.WorldToScreenPoint(_worldDragController.UpArrow.transform.position) - _positionInput).magnitude);
                var downDistance = ((_worldDragController.Panel.transform.position - _worldDragController.DownArrow.transform.position).normalized, ((Vector2)_camera.WorldToScreenPoint(_worldDragController.DownArrow.transform.position) - _positionInput).magnitude);
                var rightDistance = ((_worldDragController.Panel.transform.position - _worldDragController.RightArrow.transform.position).normalized, ((Vector2)_camera.WorldToScreenPoint(_worldDragController.RightArrow.transform.position) - _positionInput).magnitude);
                var leftDistance = ((_worldDragController.Panel.transform.position - _worldDragController.LeftArrow.transform.position).normalized, ((Vector2)_camera.WorldToScreenPoint(_worldDragController.LeftArrow.transform.position) - _positionInput).magnitude);

                var swipeChoise = new List<(Vector3 swipeDirection, float magnitude)>(){upDistance, downDistance, rightDistance, leftDistance};

                var (swipeDirection, magnitude) = swipeChoise.Where(x => x.magnitude == swipeChoise.Max(x => x.magnitude)).First();
                // Debug.Log($"definedSwipe: {definedSwipe}");

                _viewModel.Swipe(RoundVector(_hitNormal),  RoundVector(swipeDirection), _readPartPosition);
            }

            HideArrows();
        }

        private void HideArrows()
        {
            if(!_isControlEnabled)
                return;

            if (_isShown)
            {
                _isShown = false;
                _worldDragController.Panel.SetActive(false);
            }
        }

        public override void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}