using System;
using UnityEngine;
using static System.String;

namespace _Scripts.Player
{
    public class PlayerTargetDetection : MonoBehaviour
    {
        #region Actions

        public event Action<string> OnTargetDetected;

        #endregion


        #region Serializable Fields

        [SerializeField] private LayerMask targetLayer;

        #endregion


        #region Fields

        private Camera _mainCamera;
        private Ray _ray;
        private RaycastHit _hit;
        private PlayerInputHandler _playerInputHandler;

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

            Debug.LogError("Button Clicked");
        }


        private void ClickModel()
        {
            OnTargetDetected?.Invoke(_hit.collider.name);
        }

        private void Reset()
        {
            OnTargetDetected?.Invoke(Empty);
        }

        #endregion
    }
}