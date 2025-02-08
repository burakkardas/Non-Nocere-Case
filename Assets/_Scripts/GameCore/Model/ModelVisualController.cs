using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _Scripts.GameCore.Model
{
    public class ModelVisualController : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField] private List<HeadRegions> headRegionsList = new List<HeadRegions>();
        [SerializeField] private Transform headParent;

        #endregion

        #region Fields

        private static readonly int IsInteracting = Shader.PropertyToID("_IsInteracting");

        #endregion

        #region Public Methods

        public void ToggleInteractionState(Material material)
        {
            material.SetFloat(IsInteracting, Mathf.Approximately(GetInteractionValue(material), 1f) ? 0f : 1f);
        }


        public void UpdateHeadRegionVisual(HeadRegionState state, string regionName)
        {
            var headRegion = headRegionsList.Find(x => x.name.Equals(regionName));

            switch (state)
            {
                case HeadRegionState.Normal:
                    SetHeadRegionActive(headRegion, true);
                    break;
                case HeadRegionState.Transparent:
                    UpdateHeadRegionTransparency(headRegion, 1f);
                    break;
                case HeadRegionState.Disabled:
                    SetHeadRegionActive(headRegion, false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }


        [ContextMenu("Setup Head Regions")]
        public void InitializeHeadRegions()
        {
            headRegionsList = new List<HeadRegions>();

            foreach (Transform region in headParent)
            {
                headRegionsList.Add(new HeadRegions
                {
                    name = region.name,
                    headRegion = region.gameObject,
                    childRegions = GetAllChildren(region).ToList()
                });
            }
        }

        #endregion

        #region Private Methods

        private void SetHeadRegionActive(HeadRegions headRegion, bool isActive)
        {
            foreach (var region in headRegion.childRegions)
            {
                region.gameObject.SetActive(isActive);
            }

            UpdateHeadRegionTransparency(headRegion, 0f);
        }


        private void UpdateHeadRegionTransparency(HeadRegions headRegion, float transparency)
        {
            foreach (var region in headRegion.childRegions)
            {
                if (region.TryGetComponent(out Renderer renderer))
                {
                    renderer.material.SetFloat(IsInteracting, transparency);
                }
            }
        }

        private IEnumerable<Transform> GetAllChildren(Transform parent)
        {
            return parent.GetComponentsInChildren<Transform>().Where(t => t != parent);
        }

        private float GetInteractionValue(Material material) => material.GetFloat(IsInteracting);

        #endregion
    }

    [Serializable]
    public struct HeadRegions
    {
        public string name;
        public GameObject headRegion;
        public List<Transform> childRegions;
    }

    public enum HeadRegionState
    {
        Normal,
        Transparent,
        Disabled
    }
}