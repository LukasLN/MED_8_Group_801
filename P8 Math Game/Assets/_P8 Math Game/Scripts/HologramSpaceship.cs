using UnityEngine;

namespace AstroMath
{
    public class HologramSpaceship : MonoBehaviour
    {
        bool infoPanelIsActive;
        [SerializeField] GameObject infoPanelGO;

        [HideInInspector] public MathProblem mathProblem;

        public void SetPositions()
        {
            infoPanelGO.GetComponent<InfoPanel>().spaceshipPositionText.text = $"({mathProblem.spaceshipPosition.x})\n" +
                                                                               $"({mathProblem.spaceshipPosition.y})\n" +
                                                                               $"({mathProblem.spaceshipPosition.z}";

            infoPanelGO.GetComponent<InfoPanel>().dockPositionText.text = $"({mathProblem.parkingPosition.x})\n" +
                                                                          $"({mathProblem.parkingPosition.y})\n" +
                                                                          $"({mathProblem.parkingPosition.z}";
        }

        public void ToggleCanvas()
        {
            infoPanelIsActive = !infoPanelIsActive;
            infoPanelGO.SetActive(infoPanelIsActive);
        }
    }
}
