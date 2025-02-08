using System;
using System.Collections.Generic;
using _Scripts.GameCore.Model;
using _Scripts.Player;
using _Scripts.UI.Architecture;
using UnityEngine;
using VContainer;

namespace _Scripts.UI
{
    public class ModelActionView : VisualHandler
    {
        #region Constants

        private const string ShowAllButton = "ShowAllButton";
        private const string CloseAllButton = "CloseAllButton";
        private const string ArterButton = "Arter";
        private const string BrainButton = "BrainButton";
        private const string SkullPartsButton = "SkullPartsButton";

        #endregion

        #region Fields

        private readonly Dictionary<string, Action<string>> _modelActionsDictionary =
            new Dictionary<string, Action<string>>();

        private readonly Dictionary<float, HeadRegionState> _headRegionStateDictionary =
            new Dictionary<float, HeadRegionState>()
            {
                { 1f, HeadRegionState.Normal },
                { 0.5f, HeadRegionState.Transparent },
                { 0f, HeadRegionState.Disabled }
            };

        private PlayerTargetDetection _playerTargetDetection;
        private ModelVisualController _modelVisualController;

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
        private void Init(PlayerTargetDetection playerTargetDetection, ModelVisualController modelVisualController)
        {
            _playerTargetDetection = playerTargetDetection;
            _modelVisualController = modelVisualController;
        }

        private void SetupDictionary()
        {
            //_modelActionsDictionary.Add(ShowAllButton, ShowAll);
            //_modelActionsDictionary.Add(CloseAllButton, CloseAll);
            _modelActionsDictionary.Add(ArterButton, Action);
            _modelActionsDictionary.Add(BrainButton, Action);
            _modelActionsDictionary.Add(SkullPartsButton, Action);
        }


        private void InvokeAction(string actionName)
        {
            if (!_modelActionsDictionary.TryGetValue(actionName, out var action))
            {
                return;
            }

            action.Invoke(actionName);
        }


        private void Action(string state)
        {
            var button = GetImage(state);
            button.color = new Color(button.color.r, button.color.g, button.color.b,
                Mathf.Clamp(button.color.a - 0.5f, 0f, 1f));
            _modelVisualController.UpdateHeadRegionVisual(_headRegionStateDictionary[GetButtonOpacity(state)], state);
        }


        private float GetButtonOpacity(string buttonName) => GetImage(buttonName).color.a;

        #endregion
    }
}