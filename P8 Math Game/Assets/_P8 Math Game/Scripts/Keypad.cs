using UnityEngine;
using TMPro;

public class Keypad : MonoBehaviour
{
    [SerializeField] Transform pinCodeSlotsTF; //TF = Transform

    public string[] enteredCode;
    [SerializeField] TMP_Text[] slotTexts;

    [SerializeField] int currentSlotIndex;

    private void Start()
    {
        enteredCode = new string[pinCodeSlotsTF.childCount];
		slotTexts   = new TMP_Text[pinCodeSlotsTF.childCount];

        for (int i = 0; i < slotTexts.Length; i++)
        {
			slotTexts[i] = pinCodeSlotsTF.GetChild(i).GetChild(1).GetComponent<TMP_Text>();
        }
    }

    public void PressNumber(string number)
	{
		//check if can add more numbers
		if (currentSlotIndex >= enteredCode.Length)
		{
			Debug.LogWarning($"Can't enter anymore numbers! Max code length of {enteredCode.Length} reached!");
			return;
        }

		AddNumber(number);

  //      string tempCurString = myText.text;

		//string tempNewString = tempCurString + number;
		//myText.text = tempNewString;
	}

	void AddNumber(string number)
	{
		enteredCode[currentSlotIndex] = number;
		slotTexts[currentSlotIndex].text = number;

        currentSlotIndex++;
    }

	public void PressBackspace()
	{
		if(currentSlotIndex <= 0)
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

	//public void ClickBackspace()
	//{
	//	string tempGetString = myText.text;
	//	if (tempGetString.Length > 0)
	//	{
	//		string tempString = tempGetString.Substring(0, tempGetString.Length - 1);
	//		myText.text = tempString;
	//	}
	//}

	public void PressEnter()
	{

	}
}