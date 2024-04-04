using UnityEngine;

namespace AstroMath
{
    public class HologramSpaceship : MonoBehaviour
    {
        bool infoPanelIsActive;
        public GameObject infoPanelGO;

        public void ToggleCanvas()
        {
            infoPanelIsActive = !infoPanelIsActive;
            infoPanelGO.SetActive(infoPanelIsActive);
        }
    }
}
