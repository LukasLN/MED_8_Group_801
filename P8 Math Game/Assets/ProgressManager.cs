using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroMath
{
    public class ProgressManager : MonoBehaviour
    {
        public static ProgressManager instance;
        //[SerializeField] GameObject bigRedButton;
        [SerializeField] AudioPlayer holoAudioplayer;
        public int step = 0;

        // Update is called once per frame

        void Awake()
        { 
            instance = this;    
        }
            void Update()
        {
            switch (step)
            {
                case 0: //red button not pressed

                    break;
                case 1: //red button pressed
                    holoAudioplayer.PlaySoundEffect("Tutorial1",true);
                    break;
                case 2: //correct pincode inserted
                    holoAudioplayer.PlaySoundEffect("Tutorial2", true);
                    break;
                case 3: //complete minigame
                    holoAudioplayer.PlaySoundEffect("Tutorial3", true);
                    break;
                case 4: //flight initiated
                    holoAudioplayer.PlaySoundEffect("Flight", true);
                    break;
                case 5: // wrong direction vector
                    holoAudioplayer.PlaySoundEffect("Success", true);
                    break;
                case 6: // wrong direction vector
                    holoAudioplayer.PlaySoundEffect("Failed", true);
                    break;
            }
        }
    }
}
