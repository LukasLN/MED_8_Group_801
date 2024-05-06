using UnityEngine;
using TMPro;

namespace AstroMath
{
    public class WristWatch : MonoBehaviour
    {
        public static WristWatch instance;

        [SerializeField] TextMeshProUGUI timerText;
        [SerializeField] float remainingTime; //we use 600 secs as default
        [SerializeField] TMP_Text solvedText;
        [SerializeField] TMP_Text failedText;
        [SerializeField] TMP_Text scoreText;
        [SerializeField] GameObject endScreen;
        AudioPlayer audioPlayer;
        int solvedCounter = 0;
        int failedCounter = 0;
        bool countDownHasPlayed = false;
        public bool countingDown = false;

        #region Info Panel
        [Header("Info Panel")]
        [SerializeField] GameObject infoPanelGO;
        [SerializeField] bool infoPanelIsVisible;
        InfoPanel infoPanel;
        #endregion

        void Start()
        {
            audioPlayer = GetComponent<AudioPlayer>();
            if(infoPanelGO != null)
            {
                infoPanel = infoPanelGO.GetComponent<InfoPanel>();
            }
        }

        void Awake()
        {
            instance = this;    
        }

        void Update()
        {
            if (countingDown)
            {
                if (remainingTime > 0)
                {
                    remainingTime -= Time.deltaTime;
                }

                if (remainingTime < 10 && !countDownHasPlayed)
                {
                    //play da countdown audio
                    audioPlayer.PlaySoundEffect("countDown");
                    countDownHasPlayed = true;
                }
                else if (remainingTime < 0)
                {
                    audioPlayer.StopAudio();
                    countingDown = false;
                    remainingTime = 0;
                    //Game over
                    timerText.color = Color.red;

                    scoreText.SetText(solvedCounter.ToString()+" Missioner fuldført"+"\n"+ "\n" + failedCounter.ToString()+" Missioner fejlet");
                    endScreen.SetActive(true);
                }
                int minutes = Mathf.FloorToInt(remainingTime / 60);
                int seconds = Mathf.FloorToInt(remainingTime % 60);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }

        }

        public void IncrementSolved()
        {
            solvedCounter++;
            solvedText.SetText(solvedCounter.ToString());
        }

        public void IncrementFailed()
        {
            failedCounter++;
            failedText.SetText(failedCounter.ToString());
        }

        public void ToggleInfoPanel()
        {
            infoPanelIsVisible = !infoPanelIsVisible;
            infoPanelGO.SetActive(infoPanelIsVisible);
        }
        public void PauseCountDown()
        {
            countingDown = false;
        }
        public void ResumeCountDown()
        {
            countingDown = true;
        }
    }
}