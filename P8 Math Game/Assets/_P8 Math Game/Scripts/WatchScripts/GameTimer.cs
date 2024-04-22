using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AstroMath
{
    public class GameTimer : MonoBehaviour
    {
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
        void Start()
        {
        
        }
        void Update()
        {
             if (remainingTime > 0 ) 
             {
                remainingTime -= Time.deltaTime;
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
    }
}
