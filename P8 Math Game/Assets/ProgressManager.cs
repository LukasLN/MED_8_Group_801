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

        public void PlayTutorialLine(int index)
        {
            if(index > 2)
            {
                Debug.Log("Index out of range");
                return;
            }

            step = index;
            holoAudioplayer.PlaySoundEffect("Tutorial" + index);
        }

        public void NextTutoriaLine()
        {
            step++;

            if (step > 2)
            {
                Debug.Log("No more tutorial lines to playback");
                return;
            }

            PlayTutorialLine(step);
        }


        void Update()
        {
            
            //    case 4: //flight initiated
            //        holoAudioplayer.PlaySoundEffect("Flight");
            //        break;
            //    case 5: // wrong direction vector
            //        holoAudioplayer.PlaySoundEffect("Success");
            //        break;
            //    case 6: // wrong direction vector
            //        holoAudioplayer.PlaySoundEffect("Failed");
            //        break;
            //}
        }
    }
}
