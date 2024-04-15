using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MyKeyboard : MonoBehaviour
{

	public TMP_Text myText;     // drag your text object on here
	private bool shiftOn = false;

	public void ClickLetter(string letterClicked)
	{
		string tempCurString = myText.text;

		if (shiftOn)
			letterClicked = letterClicked.ToUpper();

		string tempNewString = tempCurString + letterClicked;
		myText.text = tempNewString;
	}

	public void ClickBackspace()
	{
		string tempGetString = myText.text;
		if (tempGetString.Length > 0)
		{
			string tempString = tempGetString.Substring(0, tempGetString.Length - 1);
			myText.text = tempString;
		}
	}

	public void PressShift()
	{
		shiftOn = !shiftOn;
	}
}