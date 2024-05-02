using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AstroMath;

public class Keypad : MonoBehaviour
{
	[SerializeField] InfoPanel infoPanel;

    #region Booleans
    [Header("Booleans")]
    [SerializeField] bool isSolved, isOccupied;
    #endregion

    #region Slots
    [Header("Slots")]
    [SerializeField] Transform pinCodeSlotsTF; //TF = Transform
    [SerializeField] TMP_Text[] slotTexts;
    [SerializeField] int currentSlotIndex;
    #endregion

    #region Code
    [Header("Code")]
    [SerializeField] string correctCode;
    public string[] enteredCode;
    #endregion

    #region Target
    [Header("Target")]
    [SerializeField] GameObject targetNameGroupGO;
	[SerializeField] TMP_Text targetNameText;
    [SerializeField] Image targetImage;
    [SerializeField] Sprite[] targetSprites;
    #endregion

    #region Assessment
    [Header("Assessment")]
    [SerializeField] float assessmentVisibilityTime;
	[SerializeField] GameObject assessmentBackgroundImageGO;
    [SerializeField] GameObject correctImageGO;
    [SerializeField] GameObject wrongImageGO;
    #endregion

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
            correctImageGO.SetActive(true); //telling the user they entered the correct pin code
            targetNameGroupGO.SetActive(true); //showing the target name
			infoPanel.ShowStartPosition(); //showing the start position of the spaceship
            if (infoPanel.alsoUnlockDirectionVectorWithPinCode) { infoPanel.ShowDirectionVector(); } //showing the direction vector if the pin code is also supposed to unlock that

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

    public void Clear()
    {
        #region Booleans
        isSolved = false;
        isOccupied = false;
        #endregion

        #region Slots
        for (int i = 0; i < slotTexts.Length; i++)
        {
            slotTexts[i].text = "";
        }
        currentSlotIndex = 0;
        #endregion

        #region Code
        correctCode = "";
        enteredCode = new string[pinCodeSlotsTF.childCount];
        #endregion

        #region Target
        targetNameGroupGO.SetActive(false);
        #endregion

        #region Assessment
        assessmentBackgroundImageGO.SetActive(false);
        correctImageGO.SetActive(false);
        wrongImageGO.SetActive(false);
        #endregion
    }

    public void SetCorectCode(string code)
    {
        correctCode = code;
    }

    public void SetTargetNameText(string targetName)
    {
        targetNameText.text = targetName;
    }

    public void SetTargetImageSprite(int index) //0 = Parking Spot, 1 = Asteroid
    {
        targetImage.sprite = targetSprites[index];
    }
}