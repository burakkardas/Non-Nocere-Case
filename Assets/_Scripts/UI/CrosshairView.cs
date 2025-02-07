using _Scripts.Player;
using _Scripts.UI.Architecture;
using UnityEngine;
using VContainer;

namespace _Scripts.UI
{
    public class CrosshairView : VisualHandler
    {
        #region Constants

        private const string CrosshairIcon = "CrosshairIcon";
        private const string TargetName = "TargetNameText";

        #endregion


        #region Fields

        private PlayerTargetDetection _playerTargetDetection;

        #endregion


        #region Unity Methods

        private void OnEnable()
        {
            _playerTargetDetection.OnTargetDetected += OnTargetDetected;
        }

        private void OnDisable()
        {
            _playerTargetDetection.OnTargetDetected -= OnTargetDetected;
        }

        #endregion


        #region Private Methods

        [Inject]
        private void Init(PlayerTargetDetection playerTargetDetection)
        {
            _playerTargetDetection = playerTargetDetection;
        }


        private void OnTargetDetected(string targetName)
        {
            ReplaceUnderscoreWithSpace(TargetName, targetName);
            SetColor(CrosshairIcon, targetName == "" ? Color.white : Color.red);
        }

        #endregion
    }
}