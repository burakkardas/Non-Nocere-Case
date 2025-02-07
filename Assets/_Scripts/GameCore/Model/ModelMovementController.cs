using System;
using _Scripts.Player;
using UnityEngine;
using VContainer;

namespace _Scripts.Model
{
    public class ModelMovementController : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField] private Transform modelTransform;

        #endregion


        #region Fields

        private PlayerInputHandler _playerInputHandler;

        #endregion


        #region Unity Methods

        private void Update()
        {
            RotateModel();
        }

        #endregion


        #region Private Methods

        [Inject]
        private void Init(PlayerInputHandler playerInputHandler)
        {
            _playerInputHandler = playerInputHandler;
        }


        private void RotateModel()
        {
            var modelRotationInput = _playerInputHandler.ModelRotationInput;
            transform.Rotate(Vector3.up, modelRotationInput.x);
            modelTransform.Rotate(Vector3.right, modelRotationInput.y);
        }

        #endregion
    }
}