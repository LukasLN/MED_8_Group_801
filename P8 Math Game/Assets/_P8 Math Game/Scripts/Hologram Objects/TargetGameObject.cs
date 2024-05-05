using UnityEngine;

namespace AstroMath
{
    public class TargetGameObject : MonoBehaviour
    {
        public Vector3 problemPosition;

        public bool highlightIsActive;
        public bool isAsteroid;
        [SerializeField] Renderer modelRenderer;
        [SerializeField] Material defaultMaterial;
        [SerializeField] Material highlightMaterial;
        [SerializeField] GameObject highlightSphereGO;

        public void SetHighlightActivation(bool activation)
        {
            highlightIsActive = activation;
            ChangeMaterial();
        }

        void ChangeMaterial()
        {
            if (highlightIsActive == true)
            {
                modelRenderer.material = highlightMaterial;
            }
            else
            {
                modelRenderer.material = defaultMaterial;
            }

            highlightSphereGO.SetActive(highlightIsActive);
        }
    }
}