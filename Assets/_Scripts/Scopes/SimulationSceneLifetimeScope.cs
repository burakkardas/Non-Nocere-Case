using _Scripts.GameCore.Model;
using _Scripts.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Scopes
{
    public class SimulationSceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerInputHandler playerInputHandler;
        [SerializeField] private PlayerTargetDetection playerTargetDetection;
        [SerializeField] private ModelVisualController modelVisualController;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(playerInputHandler);
            builder.RegisterInstance(playerTargetDetection);
            builder.RegisterInstance(modelVisualController);
        }
    }
}