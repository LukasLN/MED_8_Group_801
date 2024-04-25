using UnityEngine;
using TMPro;

namespace AstroMath
{
    public class StartButton : MonoBehaviour
    {
        [SerializeField] float timeBeforeStart;
        bool hasStartedCountdown;
        float timer;
        int seconds;

        #region GUI Elements
        [Header("GUI Elements")]
        [SerializeField] TMP_Text countdownText;
        #endregion

        private void Start()
        {
            timer = timeBeforeStart;
            seconds = (int)timer;

            HologramController.instance.timeToChange = timeBeforeStart;

            countdownText.text = seconds.ToString();
        }

        private void Update()
        {
            if(hasStartedCountdown)
            {
                timer -= Time.deltaTime;
                seconds = (int)timer;

                countdownText.text = seconds.ToString();
            }

            if(timer <= 0)
            {
                MathProblemManager.instance.CreateMathProblem();
                countdownText.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }

        public void StartGame()
        {
            hasStartedCountdown = true;
            countdownText.gameObject.SetActive(true);
            HologramController.instance.Toggle();
        }
    }
}