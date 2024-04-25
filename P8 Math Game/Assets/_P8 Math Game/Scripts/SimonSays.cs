using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroMath
{
    public class SimonSays : MonoBehaviour
    {
        List<int> Sequence = new List<int>();
        [SerializeField] List<GameObject> Buttons = new List<GameObject>();
        int round = 0;
        bool hasWon = false;
        bool StartButtonDisabled = false;
        bool SimonButtonsDisabled = false;

        public void RoundManager()
        {
            round++;
            if(round > 5)
            {
                hasWon = true;
                Debug.Log("Has Won");
                ResetSequence();
            }
            else
            {
                StartButtonDisabled = true;
                generateRandomSequence(round+2);
                StartCoroutine(SimonCounts());
            }

        }


        public void CheckAnswer(int ButtonValue)
        {
            if (ButtonValue == Sequence[0])
            {
                //Debug.Log("Sequence" + Sequence[0]);
                Sequence.RemoveAt(0);
                //Debug.Log("Sequence2" + Sequence[0]);
                //Debug.Log("Removed" + Sequence.Count);
                if (Sequence.Count == 0)
                {
                    Debug.Log("NewRound");
                    RoundManager();
                }
            }
            else 
            {   Debug.Log("Did not Remove");
                ResetSequence();
                hasWon = false;
                generateRandomSequence(2);
            }

        }

        void generateRandomSequence(int SequenceLength)
        {
            for(int i = 0; i < SequenceLength; i++)
            {
                Sequence.Add(Random.Range(0, 5));
            }
        }

        void ResetSequence()
        {
            Sequence.Clear();
            round = 0;
        }


        IEnumerator SimonCounts()
        {
            for(int i = 0; i < Sequence.Count; i++)
            {
                SimonButtonsDisabled = true;
                yield return new WaitForSeconds(1);
                Buttons[Sequence[i]].GetComponent<Image>().color = new Color32(139, 255, 0, 255);
                //play sound
                yield return new WaitForSeconds(1);
                Buttons[Sequence[i]].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
            SimonButtonsDisabled = false;
            yield return new WaitForSeconds(0);
        }

    }
}
