using System;
using System.Collections.Generic;
using _Scripts.Player;
using UnityEngine;
using VContainer;

namespace _Scripts.UI
{
    public class ModelActionView : MonoBehaviour
    {
        #region Constants

        private const string ShowAllButton = "ShowAllButton";
        private const string CloseAllButton = "CloseAllButton";
        private const string ArterButton = "ArterButton";
        private const string BrainButton = "BrainButton";
        private const string SkullPartsButton = "SkullPartsButton";

        #endregion

        #region Fields

        private readonly Dictionary<string, Action> _modelActionsDictionary = new Dictionary<string, Action>();
        private PlayerTargetDetection _playerTargetDetection;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            SetupDictionary();
        }

        private void OnEnable()
        {
            _playerTargetDetection.OnButtonClicked += InvokeAction;
        }

        private void OnDisable()
        {
            _playerTargetDetection.OnButtonClicked -= InvokeAction;
        }

        #endregion

        #region Private Methods

        [Inject]
        private void Init(PlayerTargetDetection playerTargetDetection)
        {
            _playerTargetDetection = playerTargetDetection;
        }

        private void SetupDictionary()
        {
            _modelActionsDictionary.Add(ShowAllButton, ShowAll);
            _modelActionsDictionary.Add(CloseAllButton, CloseAll);
            _modelActionsDictionary.Add(ArterButton, ShowArter);
            _modelActionsDictionary.Add(BrainButton, ShowBrain);
            _modelActionsDictionary.Add(SkullPartsButton, ShowSkullParts);
        }

        private void ShowSkullParts()
        {
            Debug.LogError("ShowSkullParts");
        }

        private void ShowBrain()
        {
            Debug.LogError("ShowBrain");
        }

        private void ShowArter()
        {
            Debug.LogError("ShowArter");
        }

        private void CloseAll()
        {
            Debug.LogError("CloseAll");
        }

        private void ShowAll()
        {
            Debug.LogError("ShowAll");
        }

        private void InvokeAction(string actionName)
        {
            if (!_modelActionsDictionary.TryGetValue(actionName, out var action))
            {
                return;
            }

            action.Invoke();
        }

        #endregion
    }
}