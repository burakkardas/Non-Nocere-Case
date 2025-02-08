using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModelMaterialInitializer : MonoBehaviour
{
    [SerializeField] private List<Material> shaderMaterials;
    private Color _modelColor;

    [ContextMenu("Set Model Materials")]
    public void SetModelMaterials()
    {
        foreach (var model in GetAllChildren(transform))
        {
            var meshRenderer = model.GetComponent<MeshRenderer>();
            if (meshRenderer == null) continue;

            _modelColor = meshRenderer.sharedMaterial.color;

            var originalMaterialName = meshRenderer.sharedMaterial.name.Replace(" (Instance)", "");
            var newMaterial = shaderMaterials.FirstOrDefault(x => x.name == originalMaterialName);

            if (newMaterial != null)
            {
                meshRenderer.sharedMaterial = newMaterial;
                meshRenderer.sharedMaterial.color = _modelColor;
            }
        }
    }

    private IEnumerable<Transform> GetAllChildren(Transform parent)
    {
        return parent.GetComponentsInChildren<Transform>().Where(t => t != parent);
    }
}