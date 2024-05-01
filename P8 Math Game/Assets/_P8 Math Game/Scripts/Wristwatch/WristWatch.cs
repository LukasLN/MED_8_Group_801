using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Meta.Voice.Audio;
using Unity.VisualScripting;

namespace AstroMath
{
    public class WristWatch : MonoBehaviour
    {
        public static WristWatch instance;

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    [SerializeField] TMP_Text solvedText;
    AudioPlayer audioPlayer;
    int solvedCounter = 0;
    bool countDownHasplayed = false;

    
        void Start()
        {
            audioPlayer = GetComponent<AudioPlayer>();
        }
        void Awake()
        {
            instance = this;    
        }
        void Update()
        {
             if (remainingTime > 0 ) 
             {
                remainingTime -= Time.deltaTime;
             }
             else if (remainingTime < 10 && !countDownHasplayed)
            {
                //play da countdown audio
                audioPlayer.PlaySoundEffect("countDown");
            }
             else if ( remainingTime < 0 )
             {
                remainingTime = 0;
                //Game over
                timerText.color = Color.red;
             }
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        }

        public void IncrementSolved()
        {
            solvedCounter++;
            solvedText.SetText(solvedCounter.ToString());
        }
    }
}
