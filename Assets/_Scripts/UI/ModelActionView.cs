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

        private const string ArterButton = "Arter";
        private const string BrainButton = "Brain";
        private const string SkullPartsButton = "Skull_Parts";
        private const string VenousButton = "Venous";

        #endregion

        #region Fields

        private Dictionary<string, (Action<string>, HeadRegionState)> _modelActionsDictionary =
            new Dictionary<string, (Action<string>, HeadRegionState)>();

        private readonly List<HeadRegionState> _stateCycle = new List<HeadRegionState>()
        {
            HeadRegionState.Normal,
            HeadRegionState.Transparent,
            HeadRegionState.Disabled
        };

        private readonly Dictionary<HeadRegionState, float> _stateAlphaDictionary =
            new Dictionary<HeadRegionState, float>()
            {
                { HeadRegionState.Normal, 1f },
                { HeadRegionState.Transparent, 0.5f },
                { HeadRegionState.Disabled, 0f }
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
            _modelActionsDictionary.Add(ArterButton, (UpdateState, HeadRegionState.Normal));
            _modelActionsDictionary.Add(BrainButton, (UpdateState, HeadRegionState.Normal));
            _modelActionsDictionary.Add(SkullPartsButton, (UpdateState, HeadRegionState.Normal));
            _modelActionsDictionary.Add(VenousButton, (UpdateState, HeadRegionState.Normal));
        }

        private void InvokeAction(string actionName)
        {
            if (!_modelActionsDictionary.TryGetValue(actionName, out var action))
            {
                return;
            }

            action.Item1.Invoke(actionName);
        }

        private void UpdateState(string state)
        {
            if (!_modelActionsDictionary.ContainsKey(state)) return;

            var currentState = _modelActionsDictionary[state].Item2;
            var nextStateIndex = (_stateCycle.IndexOf(currentState) + 1) % _stateCycle.Count;
            var nextState = _stateCycle[nextStateIndex];

            _modelActionsDictionary[state] = (_modelActionsDictionary[state].Item1, nextState);

            var button = GetImage(state);
            button.color = new Color(button.color.r, button.color.g, button.color.b, _stateAlphaDictionary[nextState]);

            _modelVisualController.UpdateHeadRegionVisual(nextState, state);
        }

        #endregion
    }
}