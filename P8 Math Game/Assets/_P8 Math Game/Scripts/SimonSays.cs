using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroMath
{
    public class SimonSays : MonoBehaviour
    {
        List<int> Sequence = new List<int>();
        List<GameObject> Buttons = new List<GameObject>();
        int round = 0;
        bool hasWon = false;
        bool StartButtonDisabled = false;
        bool SimonButtonsDisabled = false;

        void RoundManger()
        {
            round++;
            if(round > 5)
            {
                hasWon = true;
                ResetSequence();
            }
            else
            {
                StartButtonDisabled = true;
                generateRandomSequence(round+2);
                StartCoroutine(SimonCounts());
            }

        }


        void CheckAnswer(int ButtonValue)
        {
            if (ButtonValue == Sequence[0])
            {
                Sequence.Remove(0);
                if (Sequence.Count == 0)
                {
                    RoundManger();
                }
            }
            else 
            {
                ResetSequence();
                hasWon = false;
                generateRandomSequence(2);
            }

        }

        void generateRandomSequence(int SequenceLength)
        {
            for(int i = 0; i < SequenceLength; i++)
            {
                Sequence.Add(Random.Range(0, 6));
            }
        }

        void ResetSequence()
        {
            Sequence.Clear();
            round = 0;
        }


        IEnumerator SimonCounts()
        {
            for(int i = 0; i < Sequence.Count + 1; i++)
            {
                SimonButtonsDisabled = true;
                yield return new WaitForSeconds(1);
                Buttons[Sequence[i]].GetComponent<Image>().color = new Color32(139, 255, 0, 100);
                //play sound
                yield return new WaitForSeconds(1);
                Buttons[Sequence[i]].GetComponent<Image>().color = new Color32(255, 255, 255, 100);
            }
            SimonButtonsDisabled = false;
            yield return new WaitForSeconds(0);
        }

    }
}
