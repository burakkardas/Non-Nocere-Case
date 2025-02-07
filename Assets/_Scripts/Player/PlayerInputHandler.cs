using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        #region Fields

        private InputMaster _inputMaster;

        #endregion


        #region Properties

        public Vector2 MovementInput => _inputMaster.Player.Movement.ReadValue<Vector2>();
        public Vector2 LookInput => _inputMaster.Player.Look.ReadValue<Vector2>();

        #endregion


        #region Unity Methods

        private void Awake()
        {
            _inputMaster = new InputMaster();
        }

        private void OnEnable() => _inputMaster.Enable();
        private void OnDisable() => _inputMaster.Disable();

        #endregion
    }
}