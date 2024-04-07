using Oculus.Interaction;
using UnityEngine;

namespace AstroMath
{
    public class HologramSpaceship : MonoBehaviour
    {
        bool infoPanelIsActive;
        public GameObject infoPanelGO;
        Vector3 startPosition, directionVector;


        private void Start()
        {
            startPosition = transform.position;
        }

        private void Update()
        {
            directionVector = transform.forward;
            //Debug.Log($"Direction Vector: {directionVector}");
        }

        public void ToggleCanvas()
        {
            infoPanelIsActive = !infoPanelIsActive;
            infoPanelGO.SetActive(infoPanelIsActive);
        }
    }
}
