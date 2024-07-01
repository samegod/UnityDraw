using UnityEngine;

namespace TexturePaintTool
{
    public class PaintObject : MonoBehaviour
    {
        [SerializeField] private MeshCollider meshCollider;
        [SerializeField] private Material material;

        public MeshCollider MeshCollider => meshCollider;
        public Material Material => material;
    }
}