using System;
using CoreLib.Data.Configuration;
using UnityEngine;
using UnityEngine.UI;

namespace CK_QOL.ConfigUI.UI
{
	public class InputValidator : MonoBehaviour
	{
		private InputField _inputField;
		private string _previousValidInput = "";

		public ConfigEntryBase ConfigEntry;

		private void Awake()
		{
			_inputField = GetComponent<InputField>();
			_previousValidInput = _inputField.text;

			_inputField.onValueChanged.AddListener(OnValueChanged);
			_inputField.onEndEdit.AddListener(OnEndEdit);
		}

		private void OnValueChanged(string userInput)
		{
			try
			{
				TomlTypeConverter.ConvertToValue(userInput, ConfigEntry.SettingType);
				_inputField.image.color = Color.white;
				_inputField.textComponent.color = Color.black;
			}
			catch (Exception)
			{
				_inputField.image.color = Color.red;
				_inputField.textComponent.color = Color.white;
			}
		}

		private void OnEndEdit(string userInput)
		{
			try
			{
				TomlTypeConverter.ConvertToValue(userInput, ConfigEntry.SettingType);
				ConfigEntry.SetSerializedValue(userInput);
				_inputField.image.color = Color.white;
				_inputField.textComponent.color = Color.black;
			}
			catch (Exception)
			{
				_inputField.text = _previousValidInput;
				_inputField.caretPosition = _previousValidInput.Length;
				_inputField.image.color = Color.red;
				_inputField.textComponent.color = Color.white;
			}
		}
	}
}