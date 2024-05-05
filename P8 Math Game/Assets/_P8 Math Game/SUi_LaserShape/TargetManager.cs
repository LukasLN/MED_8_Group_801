using System.Collections.Generic;
using UnityEngine;

namespace AstroMath
{
    public class TargetManager : MonoBehaviour
    {
        [SerializeField] List<GameObject> targets = new List<GameObject>();
        public int currentStep = 0;

        [SerializeField] InteractableSpaceship spaceship;

        public void NextTarget()
        {
            currentStep++;

            switch (currentStep)
            {
                case 1: // target 1
                    targets[0].SetActive(false);
                    targets[1].SetActive(true);
                    break;
                case 2: // target 2
                    targets[1].SetActive(false);
                    targets[2].SetActive(true);
                    break;
                case 3: // target 3
                    targets[2].SetActive(false);
                    targets[3].SetActive(true);
                    break;
                case 4: // target 1
                    targets[3].SetActive(false);
                    targets[1].SetActive(true);
                    break;
                case 5: // target 0
                    targets[1].SetActive(false);
                    targets[0].SetActive(true);
                    break;
                case 6: // target 3
                    targets[0].SetActive(false);
                    targets[3].SetActive(true);
                    break;
                case 7: // target 2
                    targets[3].SetActive(false);
                    targets[2].SetActive(true);
                    break;
                case 8: // target 0
                    targets[2].SetActive(false);
                    targets[0].SetActive(true);
                    break;
                case 9: // target 3
                    targets[0].SetActive(false);
                    targets[3].SetActive(true);
                    break;
                case 10: // target 1
                    targets[3].SetActive(false);
                    targets[1].SetActive(true);
                    break;
                case 11: // target 2
                    targets[1].SetActive(false);
                    targets[2].SetActive(true);
                    break;
                case (12): // Done 
                    targets[2].SetActive(false);
                    targets[4].SetActive(true);
                    break;
            }
        }
    }
}