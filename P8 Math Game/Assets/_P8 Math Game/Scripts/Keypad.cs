using System.Collections;
using UnityEngine;
using TMPro;
using AstroMath;

public class Keypad : MonoBehaviour
{
	[SerializeField] bool isSolved, isOccupied;

	[SerializeField] InfoPanel infoPanel;

    [SerializeField] Transform pinCodeSlotsTF; //TF = Transform
	
	public string correctCode;
    public string[] enteredCode;

	[SerializeField] GameObject targetIDGroupGO;
	public TMP_Text targetIDText;

	[SerializeField] TMP_Text[] slotTexts;
    [SerializeField] int currentSlotIndex;

	[SerializeField] float assessmentVisibilityTime;
	[SerializeField] GameObject assessmentBackgroundImageGO;
    [SerializeField] GameObject correctImageGO;
    [SerializeField] GameObject wrongImageGO;

    AudioPlayer audioPlayer;

    private void Start()
    {
        enteredCode = new string[pinCodeSlotsTF.childCount];
		slotTexts   = new TMP_Text[pinCodeSlotsTF.childCount];

        for (int i = 0; i < slotTexts.Length; i++)
        {
			slotTexts[i] = pinCodeSlotsTF.GetChild(i).GetChild(1).GetComponent<TMP_Text>();
        }

        audioPlayer = GetComponent<AudioPlayer>();
    }

    public void PressNumber(string number)
	{
        audioPlayer.PlaySoundEffect(number);

        if (isSolved == true)
		{
			Debug.LogWarning("The code has already been solved!");
			return;
		}

        if(isOccupied == true)
        {
            Debug.LogWarning("Can't enter code right now! Wait for X to disappear!");
            return;
        }

        if (currentSlotIndex >= enteredCode.Length)
		{
			Debug.LogWarning($"Can't enter anymore numbers! Max code length of {enteredCode.Length} reached!");
			return;
        }

        AddNumber(number);
	}

	void AddNumber(string number)
	{
		enteredCode[currentSlotIndex] = number;
		slotTexts[currentSlotIndex].text = number;

        currentSlotIndex++;
    }

	public void PressBackspace()
	{
        audioPlayer.PlaySoundEffect("Backspace");

        if (isSolved == true)
        {
            Debug.LogWarning("The code has already been solved!");
            return;
        }

        if (isOccupied == true)
        {
            Debug.LogWarning("Can't enter code right now! Wait for X to disappear!");
            return;
        }

        if (currentSlotIndex <= 0)
		{
			Debug.LogWarning("Can't remove anymore numbers! No numbers to remove!");
            return;
        }

        RemoveNumber();
    }

	void RemoveNumber()
	{
        currentSlotIndex--;

        enteredCode[currentSlotIndex] = "";
		slotTexts[currentSlotIndex].text = "";
    }

	public void PressEnter()
	{
        if (isSolved == true)
        {
            Debug.LogWarning("The code has already been solved!");
            return;
        }

        string concatenatedCode = string.Join("", enteredCode);

        if (concatenatedCode == correctCode)
		{
            //Debug.Log("Correct Code Entered!");
            isSolved = true;
            correctImageGO.SetActive(true);
            targetIDGroupGO.SetActive(true);
			infoPanel.ShowStartPosition();

            audioPlayer.PlaySoundEffect("Correct");
            ProgressManager.instance.step = 2;
        }
        else
		{
            //Debug.Log("Incorrect Code Entered!");
            isOccupied = true;
            assessmentBackgroundImageGO.SetActive(true);
            wrongImageGO.SetActive(true);
            StartCoroutine(WaitBeforeHideImage(wrongImageGO));

            audioPlayer.PlaySoundEffect("Wrong");
        }
	}

	IEnumerator WaitBeforeHideImage(GameObject imageToShow)
	{
        yield return new WaitForSeconds(assessmentVisibilityTime);
        isOccupied = false;
        imageToShow.SetActive(false);
        assessmentBackgroundImageGO.SetActive(false);
    }
}