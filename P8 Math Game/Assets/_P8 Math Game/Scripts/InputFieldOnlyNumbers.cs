using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

namespace AstroMath
{
    public class InputFieldOnlyNumbers : MonoBehaviour
    {
        TMP_InputField inputField;
        public int number;

        private void Start()
        {
            inputField = GetComponent<TMP_InputField>();
            inputField.onValueChanged.AddListener(OnInputValueChanged);
        }

        void OnInputValueChanged(string newValue)
        {
            string cleanedValue = Regex.Replace(newValue, @"[^0-9\-]", "");

            inputField.text = cleanedValue;
            number = int.Parse(inputField.text);
        }

        public void IncreaseNumber()
        {
            OnInputValueChanged((number + 1).ToString());
        }

        public void DecreaseNumber()
        {
            OnInputValueChanged((number - 1).ToString());
        }
    }
}