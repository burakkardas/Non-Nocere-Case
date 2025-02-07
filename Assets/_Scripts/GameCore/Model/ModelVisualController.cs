using UnityEngine;

namespace _Scripts.GameCore.Model
{
    public class ModelVisualController : MonoBehaviour
    {
        #region Fields

        private static readonly int IsInteracting = Shader.PropertyToID("_IsInteracting");

        #endregion


        #region Public Methods

        public void ToggleModelVisual(Material material)
        {
            material.SetFloat(IsInteracting, Mathf.Approximately(GetInteractingValue(material), 1f) ? 0f : 1f);
        }

        #endregion

        #region Private Methods

        private float GetInteractingValue(Material material) => material.GetFloat(IsInteracting);

        #endregion
    }
}