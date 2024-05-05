using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AstroMath
{
    public class SimonSays : MonoBehaviour
    {
        [SerializeField] InfoPanel infoPanel;

        #region Rounds
        [Header("Rounds")]
        [SerializeField] int currentRound;
        [SerializeField] int numberOfRounds;
        #endregion

        #region Sequence
        [Header("Sequence")]
        [SerializeField] int sequenceStartLength;
        [SerializeField] int sequenceCurrentLength;
        [SerializeField] int sequenceIncreaseAmount;
        int activeLightUpIndexInSequence;
        [SerializeField] int activeInputIndexInSequence;
        [SerializeField] List<int> sequence = new List<int>();
        #endregion

        #region Timing
        [Header("Timing")]
        [SerializeField] bool isLightedUp;
        [SerializeField] bool isWaitingForNextLightUp;
        [SerializeField] bool isWaitingForInput;
        [SerializeField] bool hasStartedInput;

        [SerializeField] float lightUpTimer;
        [SerializeField] float timeBetweenLightUpTimer;
        [SerializeField] float waitForInputTimer;

        [SerializeField] float lightUpDuration;
        [SerializeField] float timeBetweenLightUp;
        [SerializeField] float waitForInputDuration;
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
        [SerializeField] Transform sequenceLedsParent;
        Image[] sequenceLEDs;
        [SerializeField] Image waitImage;
        [SerializeField] Image pressImage;

        [SerializeField] Color waitLedOnColor;
        [SerializeField] Color waitLedOffColor;
        [SerializeField] Color waitLedDisabledColor;

        [SerializeField] Color pressLedOnColor;
        [SerializeField] Color pressLedOffColor;
        [SerializeField] Color pressLedDisabledColor;
        [SerializeField] Color pressLedWrongColor;
        #endregion

        #region Symbols
        [Header("Symbols")]
        [SerializeField] bool useRandomSymbols;
        [SerializeField] string[] symbols;
        string[] symbolsToUse;
        #endregion

        #region Animation
        [Header("Animation")]
        Animator gateAnimator;
        #endregion

        #region Assessment
        [Header("Assessment")]
        [SerializeField] GameObject assessmentGO;
        #endregion

        AudioPlayer audioPlayer;
        bool isPlayingBlinking;

        private void Start()
        {
            gateAnimator = GetComponent<Animator>();
            audioPlayer = GetComponent<AudioPlayer>();

            #region Rounds
            currentRound = 1;
            #endregion

            #region Timing
            lightUpTimer = lightUpDuration;
            timeBetweenLightUpTimer = timeBetweenLightUp;
            waitForInputTimer = waitForInputDuration;

            isLightedUp = false;
            isWaitingForNextLightUp = true;
            isWaitingForInput = false;
            #endregion

            #region Setting Buttons Array
            buttons = new Button[buttonsParent.childCount];
            for (int i = 0; i < buttonsParent.childCount; i++)
            {
                buttons[i] = buttonsParent.GetChild(i).GetComponent<Button>();
            }
            TurnOffAllButtonLights();
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

            #region Setting LEDs Array
            sequenceLEDs = new Image[sequenceLedsParent.childCount];
            for (int i = 0; i < sequenceLedsParent.childCount; i++)
            {
                sequenceLEDs[i] = sequenceLedsParent.GetChild(i).GetComponent<Image>();
            }
            TurnOffAllSequenceLEDs();
            TurnOnWaitLED();
            #endregion
        }

        private void Update()
        {
            if (isLightedUp) //if a button is lit up
            {
                DisableAllButtons();

                //> wait the time the button should be lit up
                lightUpTimer -= Time.deltaTime; //the timer is counting down
                if (lightUpTimer <= 0) //if we reach zero
                {
                    lightUpTimer = lightUpDuration; //we reset the timer
                    isLightedUp = false; //we set this to false, meaning that no button is to be lit up right now
                    TurnOffAllButtonLights(); //we turn off all button lights

                    activeLightUpIndexInSequence++; //we increase the active button index

                    if (activeLightUpIndexInSequence >= sequence.Count) //if we reach the end of the sequence
                    {
                        activeLightUpIndexInSequence = 0; //we reset the active button index
                        
                        isWaitingForInput = true; //we set this to true, meaning that we are waiting for the player to input the sequence
                        TurnOnActiveSequenceLEDsAsPressLEDColor();

                    }
                    else //if we haven't reached the end of the sequence
                    {
                        isWaitingForNextLightUp = true;
                    }
                }
            }
            else if(isWaitingForNextLightUp) //if waiting for the next button to light up
            {
                DisableAllButtons();

                //> wait the time in-between light ups
                timeBetweenLightUpTimer -= Time.deltaTime;
                if(timeBetweenLightUpTimer <= 0)
                {
                    TurnOnActiveButtonLight(activeLightUpIndexInSequence);

                    isLightedUp = true;
                    isWaitingForNextLightUp = false;
                    timeBetweenLightUpTimer = timeBetweenLightUp;
                }
            }
            else if(isWaitingForInput) //if waiting for the player to input the sequence
            {
                EnableAllButtons();

                waitForInputTimer -= Time.deltaTime;

                //> turn off all the sequenceLEDs
                if (waitForInputTimer <= 0)
                {
                    waitForInputTimer = waitForInputDuration;
                    isWaitingForInput = false;
                    isWaitingForNextLightUp = true;
                    isPlayingBlinking = false;
                    TurnOffAllSequenceLEDs();
                    TurnOnWaitLED();
                    DisableAllButtons();
                }
                else
                {
                    TurnOnPressLED();

                    //> blink the sequenceLEDs
                    if(waitForInputTimer <= waitForInputDuration / 2)
                    {
                        if(isPlayingBlinking == false)
                        {
                            isPlayingBlinking = true;
                            audioPlayer.PlaySoundEffect("Blinking");
                        }

                        if (waitForInputTimer % 0.5f < 0.25f)
                        {
                            TurnOffActiveSequenceLEDsAsPressLEDColor();
                        }
                        else
                        {
                            TurnOnActiveSequenceLEDsAsPressLEDColor();
                        }
                        //TurnOnActiveSequenceLEDs();
                        //TurnOffActiveSequenceLEDs();
                    }
                }
            }
        }

        void TurnOnPressLED()
        {
            pressImage.color = pressLedOnColor;
            waitImage.color = waitLedDisabledColor;
        }

        void TurnOnWaitLED()
        {
            pressImage.color = pressLedDisabledColor;
            waitImage.color = waitLedOnColor;
        }

        public void PressSymbol(int buttonIndex)
        {
            //audioPlayer.PlaySoundEffect("ButtonPress");
            PlayBeep("SimonSaysBeep" + (buttonIndex + 1));

            if (hasStartedInput == false) //if we haven't started inputting the sequence yet
            {
                hasStartedInput = true;
                TurnOffAllSequenceLEDsAsPressLEDColor();
            }

            isLightedUp = false;
            isWaitingForNextLightUp = false;
            isWaitingForInput = false;
            waitForInputTimer = waitForInputDuration;

            if (buttonIndex == sequence[activeInputIndexInSequence]) //if our input is CORRECT
            {
                sequenceLEDs[activeInputIndexInSequence].color = pressLedOnColor; //we turn on the next LED

                activeInputIndexInSequence++; //we increase the active input index

                if (activeInputIndexInSequence >= sequence.Count) //if we reach the end of the sequence
                {
                    audioPlayer.PlaySoundEffect("Correct");

                    hasStartedInput = false;
                    ShowCorrect(); //we show that inputtet sequence is correct
                    sequenceCurrentLength += sequenceIncreaseAmount; //we increase the sequence length
                    activeInputIndexInSequence = 0; //we reset the active input index
                    currentRound++; //we increase the current round

                    CloseGate();
                }
            }
            else //if our input is WRONG
            {
                audioPlayer.PlaySoundEffect("Wrong");

                sequenceLEDs[activeInputIndexInSequence].color = pressLedWrongColor; //we turn on the next LED, but this was a mistake so we turn it red

                activeInputIndexInSequence = 0;
                hasStartedInput = false;
                ShowWrong();

                CloseGate();
            }
        }

        void OpenGate()
        {
            if (currentRound > numberOfRounds) //if we reached the end of the game
            {
                assessmentGO.SetActive(true); //we show that the game is over

                switch(infoPanel.unlockingType)
                {
                    case 0:
                        infoPanel.ShowDirectionVector();
                        break;
                    case 1:
                        infoPanel.ShowTScalar();
                        //unhide shoot/go buttons
                        break;
                    case 2:
                        infoPanel.ShowTScalar();
                        break;
                }
            }
            else //if we are still going
            {
                ClearSequence(); //we clear the sequence
                CreateSequence(); //we create a new sequence
                TurnOffAllButtonLights(); //we turn off all button lights
                TurnOffAllSequenceLEDs(); //we turn off all sequenceLEDs
                isPlayingBlinking = false;

                if (useRandomSymbols)
                {
                    SelectRandomSymbols();
                    SetButtonsSymbol();
                }

                isWaitingForNextLightUp = true;

                gateAnimator.SetTrigger("Open");
            }
        }

        void CloseGate()
        {
            gateAnimator.SetTrigger("Close");
            TurnOnWaitLED();
        }

        void TurnOnActiveButtonLight(int activeIndex)
        {
            buttons[sequence[activeIndex]].image.color = buttonLightUpColor;
            sequenceLEDs[activeIndex].color = waitLedOnColor;
            PlayBeep("SimonSaysBeep" + (sequence[activeIndex] + 1));
        }

        void TurnOffAllButtonLights()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                TurnOffButtonLight(i);
            }
        }

        void TurnOnActiveSequenceLEDs()
        {
            for (int i = 0; i < sequenceLEDs.Length; i++)
            {
                if (i < sequence.Count)
                {
                    sequenceLEDs[i].color = waitLedOnColor;
                }
            }
        }

        void TurnOffActiveSequenceLEDs()
        {
            for (int i = 0; i < sequenceLEDs.Length; i++)
            {
                if (i < sequence.Count)
                {
                    sequenceLEDs[i].color = waitLedOffColor;
                }
            }
        }

        void TurnOnActiveSequenceLEDsAsPressLEDColor()
        {
            for (int i = 0; i < sequenceLEDs.Length; i++)
            {
                if (i < sequence.Count)
                {
                    sequenceLEDs[i].color = pressLedOnColor;
                }
            }
        }

        void TurnOffActiveSequenceLEDsAsPressLEDColor()
        {
            for (int i = 0; i < sequenceLEDs.Length; i++)
            {
                if (i < sequence.Count)
                {
                    sequenceLEDs[i].color = pressLedOffColor;
                }
            }
        }

        void TurnOffAllSequenceLEDs()
        {
            for(int i = 0; i < sequenceLEDs.Length; i++)
            {
                if(i < sequence.Count)
                {
                    sequenceLEDs[i].color = waitLedOffColor;
                }
                else
                {
                    sequenceLEDs[i].color = waitLedDisabledColor;
                }
            }
        }

        void TurnOffAllSequenceLEDsAsPressLEDColor()
        {
            for (int i = 0; i < sequenceLEDs.Length; i++)
            {
                if (i < sequence.Count)
                {
                    sequenceLEDs[i].color = pressLedOffColor;
                }
                else
                {
                    sequenceLEDs[i].color = pressLedDisabledColor;
                }
            }
        }

        void TurnOffButtonLight(int index)
        {
            buttons[index].image.color = buttonDefaultColor;
        }

        void ShowCorrect()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].image.color = buttonCorrectColor;
            }
        }

        void ShowWrong()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].image.color = buttonWrongColor;
            }
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

        void EnableAllButtons()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = true;
            }
        }

        void DisableAllButtons()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = false;
            }
        }

        public void PlayBeep(string beepName)
        {
            audioPlayer.PlaySoundEffect(beepName);
        }
    }
}