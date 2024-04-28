using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AstroMath
{
    public class SimonSays : MonoBehaviour
    {
        #region Rounds
        [SerializeField] int currentRound;
        [SerializeField] int numberOfRounds;
        #endregion

        #region Sequence
        [Header("Sequence")]
        [SerializeField] int sequenceStartLength;
        [SerializeField] int sequenceCurrentLength;
        [SerializeField] int sequenceIncreaseAmount;
        [SerializeField] int activeIndexInSequence;
        [SerializeField] List<int> sequence = new List<int>();
        #endregion

        #region Timing
        [Header("Timing")]
        [SerializeField] bool isLightedUp;
        [SerializeField] bool isWaitingForNextLightUp;
        [SerializeField] float lightUpDuration;
        [SerializeField] float timeBetweenLightUp;
        [SerializeField] float lightUpTimer;
        [SerializeField] float timeBetweenLightUpTimer;
        #endregion

        #region Buttons
        [Header("Buttons")]
        [SerializeField] Transform buttonsParent;
        Button[] buttons;

        [SerializeField] Color buttonDefaultColor;
        [SerializeField] Color buttonLightUpColor;
        [SerializeField] Color buttonCorrectColor;
        [SerializeField] Color buttonWrongColor;
        #endregion

        #region LEDs
        [Header("LEDs")]
        [SerializeField] Transform ledsParent;
        Image[] LEDs;

        [SerializeField] Color ledOnColor;
        [SerializeField] Color ledOffColor;
        [SerializeField] Color ledDisabledColor;
        #endregion

        #region Symbols
        [Header("Symbols")]
        [SerializeField] bool useRandomSymbols;
        [SerializeField] string[] symbols;
        string[] symbolsToUse;
        #endregion

        private void Start()
        {
            #region Rounds
            currentRound = 1;
            #endregion

            #region Timing
            lightUpTimer = lightUpDuration;
            timeBetweenLightUpTimer = timeBetweenLightUp;

            isLightedUp = false;
            isWaitingForNextLightUp = true;
            #endregion

            #region Setting Buttons Array
            buttons = new Button[buttonsParent.childCount];
            for (int i = 0; i < buttonsParent.childCount; i++)
            {
                buttons[i] = buttonsParent.GetChild(i).GetComponent<Button>();
            }
            TurnOffAllButtonLights();
            #endregion

            #region Setting LEDs Array
            LEDs = new Image[ledsParent.childCount];
            for (int i = 0; i < ledsParent.childCount; i++)
            {
                LEDs[i] = ledsParent.GetChild(i).GetComponent<Image>();
            }
            TurnOffAllLEDs();
            #endregion

            #region Picking Random Symbols
            if (useRandomSymbols)
            {
                SelectRandomSymbols();
                SetButtonsSymbol();
            }
            #endregion

            #region Setting Sequence
            sequenceCurrentLength = sequenceStartLength;
            CreateSequence();
            #endregion
        }

        private void Update()
        {
            //wait the time in between light ups


            //wait the time the button is lit up

            if (isLightedUp)
            {
                lightUpTimer -= Time.deltaTime;
                if (lightUpTimer <= 0)
                {
                    activeIndexInSequence++;
                    if (activeIndexInSequence >= sequence.Count)
                    {
                        activeIndexInSequence = 0;
                        TurnOffAllLEDs();
                    }

                    TurnOffAllButtonLights();

                    lightUpTimer = lightUpDuration;
                    isLightedUp = false;
                    isWaitingForNextLightUp = true;
                }
            }
            else if(isWaitingForNextLightUp)
            {
                timeBetweenLightUpTimer -= Time.deltaTime;
                if(timeBetweenLightUpTimer <= 0)
                {
                    TurnOnActiveButtonLight(activeIndexInSequence);
                    isLightedUp = true;
                    isWaitingForNextLightUp = false;
                    timeBetweenLightUpTimer = timeBetweenLightUp;
                }
            }
        }

        void TurnOnActiveButtonLight(int activeIndex)
        {
            buttons[sequence[activeIndex]].image.color = buttonLightUpColor;
            LEDs[activeIndex].color = ledOnColor;
        }

        void TurnOffAllButtonLights()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                TurnOffButtonLight(i);
            }
        }

        void TurnOffAllLEDs()
        {
            for(int i = 0; i < LEDs.Length; i++)
            {
                if(i < sequence.Count)
                {
                    LEDs[i].color = ledOffColor;
                }
                else
                {
                    LEDs[i].color = ledDisabledColor;
                }
            }
        }

        void TurnOffButtonLight(int index)
        {
            buttons[index].image.color = buttonDefaultColor;
        }

        void UpdateButtonLights()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i == sequence[activeIndexInSequence])
                {
                    buttons[i].GetComponent<Image>().color = buttonLightUpColor;
                }
                else
                {
                    buttons[i].GetComponent<Image>().color = buttonDefaultColor;
                }
            }
        }

        void GoThroughSequence()
        {

        }

        void CreateSequence()
        {
            for (int i = 0; i < sequenceCurrentLength; i++)
            {
                sequence.Add(Random.Range(0, buttons.Length));
            }
        }

        void ClearSequence()
        {
            sequence.Clear();
        }

        void SelectRandomSymbols()
        {
            symbolsToUse = new string[buttons.Length];
            List<string> list = symbols.ToList();

            for (int i = 0; i < symbolsToUse.Length; i++)
            {
                var index = Random.Range(0, list.Count);
                symbolsToUse[i] = list[index];
                list.RemoveAt(index);
            }
        }

        void SetButtonsSymbol()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponentInChildren<TMP_Text>().text = symbolsToUse[i];
                buttons[i].gameObject.name = $"{i} {symbolsToUse[i]} Button";
            }
        }
    }
}