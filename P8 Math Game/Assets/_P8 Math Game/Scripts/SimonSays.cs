using Meta.Voice.Audio;
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
        [SerializeField] Color onColor;
        [SerializeField] Color offColor;
        int round = 0;
        [SerializeField] int numberOfRoundsToWin;
        bool isSolved = false;
        bool StartButtonDisabled = false;
        bool SimonButtonsDisabled = false;

        [SerializeField] bool isOccupied;

        [SerializeField] InfoPanel infoPanel;

        #region Assessment
        [SerializeField] float assessmentVisibilityTime;
        [SerializeField] GameObject assessmentBackgroundImageGO;
        [SerializeField] GameObject correctImageGO;
        [SerializeField] GameObject wrongImageGO;
        #endregion

        AudioPlayer audioPlayer;

        private void Start()
        {
            audioPlayer = GetComponent<AudioPlayer>();
        }

        public void RoundManager()
        {
            round++;
            if(round > numberOfRoundsToWin)
            {
                assessmentBackgroundImageGO.SetActive(true);
                correctImageGO.SetActive(true);

                infoPanel.ShowDirectionVector();
                isSolved = true;
            }
            else
            {
                StartButtonDisabled = true;
                GenerateRandomSequence(round+2);
                StartCoroutine(SimonCounts());
            }
        }

        public void CheckAnswer(int ButtonValue)
        {
            if (isSolved == true)
            {
                Debug.LogWarning("The mini game has already been solved!");
                return;
            }

            if (isOccupied == true)
            {
                Debug.LogWarning("Can't press buttons right now! Wait for X to disappear!");
                return;
            }

            if (ButtonValue == Sequence[0])
            {
                //audioPlayer.PlaySoundEffect("Correct");
                //

                //Debug.Log("Sequence" + Sequence[0]);
                Sequence.RemoveAt(0);
                //Debug.Log("Sequence2" + Sequence[0]);
                //Debug.Log("Removed" + Sequence.Count);
                if (Sequence.Count == 0)
                {
                    //show checkmark
                    assessmentBackgroundImageGO.SetActive(true);
                    correctImageGO.SetActive(true);
                    StartCoroutine(WaitBeforeHideImage(correctImageGO));

                    Debug.Log("NewRound");
                    RoundManager();
                }
            }
            else 
            {
                //show cross
                wrongImageGO.SetActive(true);
                StartCoroutine(WaitBeforeHideImage(wrongImageGO));
                //audioPlayer.PlaySoundEffect("Incorrect");
                //

                Debug.Log("Did not Remove");
                ResetSequence();
                RoundManager();
            }
        }

        IEnumerator WaitBeforeHideImage(GameObject imageToShow)
        {
            yield return new WaitForSeconds(assessmentVisibilityTime);
            isOccupied = false;
            imageToShow.SetActive(false);
            assessmentBackgroundImageGO.SetActive(false);
        }

        void GenerateRandomSequence(int SequenceLength)
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

        IEnumerator SimonCounts() //for "lighting up" the buttons
        {
            isOccupied = true;
            for(int i = 0; i < Sequence.Count; i++)
            {
                SimonButtonsDisabled = true;
                yield return new WaitForSeconds(1);
                Buttons[Sequence[i]].GetComponent<Image>().color = onColor;
                //play sound
                yield return new WaitForSeconds(1);
                Buttons[Sequence[i]].GetComponent<Image>().color = offColor;
            }
            SimonButtonsDisabled = false;
            yield return new WaitForSeconds(0);
            isOccupied = false;
        }
    }
}
