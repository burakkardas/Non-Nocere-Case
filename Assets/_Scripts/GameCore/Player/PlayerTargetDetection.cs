using System;
using _Scripts.GameCore.Model;
using UnityEngine;
using VContainer;
using static System.String;

namespace _Scripts.Player
{
    public class PlayerTargetDetection : MonoBehaviour
    {
        #region Actions

        public event Action<string> OnTargetDetected;
        public event Action<string> OnButtonClicked;

        #endregion


        #region Serializable Fields

        [SerializeField] private LayerMask targetLayer;

        #endregion


        #region Fields

        private Camera _mainCamera;
        private Ray _ray;
        private RaycastHit _hit;
        private PlayerInputHandler _playerInputHandler;
        private ModelVisualController _modelVisualController;

        private const string Model = "Model";
        private string _targetName;

        #endregion


        #region Unity Methods

        private void Awake()
        {
            _mainCamera = Camera.main;
            _playerInputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            DetectTarget();
        }

        #endregion


        #region Private Methods

        [Inject]
        private void Init(ModelVisualController modelVisualController)
        {
            _modelVisualController = modelVisualController;
        }

        private void DetectTarget()
        {
            _ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);

            if (!Physics.Raycast(_ray, out _hit, Mathf.Infinity, targetLayer))
            {
                Reset();
                return;
            }

            if (_hit.collider == null)
            {
                Reset();
                return;
            }

            (LayerMask.LayerToName(_hit.collider.gameObject.layer).Equals(Model) ? (Action)ClickModel : ClickModelUI)();
        }

        private void ClickModelUI()
        {
            if (!_playerInputHandler.ClickLeftButton) return;

            OnButtonClicked?.Invoke(_hit.collider.name);
        }


        private void ClickModel()
        {
            OnTargetDetected?.Invoke(_hit.collider.name);
            if (!_playerInputHandler.ClickLeftButton) return;
            if (_hit.collider.TryGetComponent(out Renderer xRenderer))
            {
                _modelVisualController.ToggleModelVisual(xRenderer.material);
            }
        }

        private void Reset()
        {
            OnTargetDetected?.Invoke(Empty);
        }

        #endregion
    }
}