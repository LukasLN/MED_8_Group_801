using UnityEngine;

namespace AstroMath
{
    public class ToggleHologramCanvas : MonoBehaviour
    {
        bool canvasIsActive;
        [SerializeField] GameObject hologramCanvasGO;

        public void ToggleCanvas()
        {
            canvasIsActive = !canvasIsActive;
            hologramCanvasGO.SetActive(canvasIsActive);
        }
    }
}