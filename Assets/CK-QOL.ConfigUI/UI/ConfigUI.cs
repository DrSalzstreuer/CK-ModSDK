using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CK_QOL.ConfigUI.UI.Elements;
using CoreLib.Data.Configuration;
using UnityEngine;
using UnityEngine.UI;

namespace CK_QOL.ConfigUI.UI
{
	public class ConfigUI : MonoBehaviour
	{
		[Header("UI Elements")] 
		public GameObject uiContainerElement;
		public GameObject modContainerElement;
		public GameObject saveButtonElement;

		[Header("Container Prefabs")] 
		public GameObject modPrefab;
		public GameObject sectionPrefab;
		public GameObject configPrefab;

		[Header("Input Prefabs")] 
		public GameObject inputFieldPrefab;
		public GameObject dropDownPrefab;
		public GameObject sliderPrefab;
		public GameObject togglePrefab;

		private readonly Dictionary<string, List<ConfigFile>> _configFiles = new Dictionary<string, List<ConfigFile>>();

		public void ToggleUI()
		{
			uiContainerElement.SetActive(!uiContainerElement.activeSelf);
			StartCoroutine(RebuildLayoutNextFrames());
		}

		public void ShowUI()
		{
			uiContainerElement.SetActive(true);
			StartCoroutine(RebuildLayoutNextFrames());
		}

		public void HideUI()
		{
			uiContainerElement.SetActive(false);
			StartCoroutine(RebuildLayoutNextFrames());
		}

		private void Awake()
		{
			saveButtonElement.GetComponent<Button>().onClick.AddListener(SaveConfig);
		}

		private void Start()
		{
			LoadConfigs();
			RenderUI();
			HideUI();
		}

		private void LoadConfigs()
		{
			foreach (var configFile in ConfigFile.AllConfigFilesReadOnly)
			{
				var modName = ConfigFile.GetDirectoryName(configFile.ConfigFilePath);

				if (_configFiles.TryGetValue(modName, out var configFiles))
				{
					configFiles.Add(configFile);
				}
				else
				{
					_configFiles.Add(modName, new List<ConfigFile> { configFile });
				}
			}
		}

		private void RenderUI()
		{
			foreach (var mod in _configFiles)
			{
				var modElement = Instantiate(modPrefab, modContainerElement.transform).GetComponent<ModElement>();
				modElement.nameElement.GetComponent<Text>().text = mod.Key;

				foreach (var configFile in mod.Value)
				{
					var groupedConfigFileEntries = configFile.Entries.GroupBy(entry => entry.Key.Section);
					foreach (var groupedConfigFileEntry in groupedConfigFileEntries)
					{
						var sectionElement = Instantiate(sectionPrefab, modElement.sectionContainerElement.transform).GetComponent<SectionElement>();
						sectionElement.nameElement.GetComponent<Text>().text = groupedConfigFileEntry.Key;

						foreach (var (key, configEntry) in groupedConfigFileEntry)
						{
							var configElement = Instantiate(configPrefab, sectionElement.configContainerElement.transform).GetComponent<ConfigElement>();
							configElement.nameElement.GetComponent<Text>().text = $"{key.Key} {(configEntry.DefaultValue != null ? " (" + configEntry.DefaultValue + ")" : "")}";
							configElement.descriptionElement.GetComponent<Text>().text = configEntry.Description.Description;

							if (configEntry.Description == ConfigDescription.Empty || configEntry.Description.AcceptableValues == null)
							{
								var inputElement = Instantiate(inputFieldPrefab, configElement.inputContainerElement.transform).GetComponent<InputField>();
								inputElement.text = configEntry.BoxedValue.ToString();

								inputElement.onValueChanged.AddListener(value => configEntry.SetSerializedValue(value.ToString(CultureInfo.InvariantCulture)));
							}
							else if (configEntry.Description.AcceptableValues != null)
							{
								switch (configEntry.Description.AcceptableValues)
								{
									case AcceptableValueRange<int> intRange:
									{
										var inputElementContainer = Instantiate(sliderPrefab, configElement.inputContainerElement.transform);
										var currentValueElement = inputElementContainer.transform.Find("CurrentValue").GetComponent<Text>();
										var inputElement = inputElementContainer.transform.Find("Slider").GetComponent<Slider>();
										inputElement.value = float.Parse(configEntry.BoxedValue.ToString());
										currentValueElement.text = configEntry.BoxedValue.ToString();
										inputElement.minValue = intRange.MinValue;
										inputElement.maxValue = intRange.MaxValue;
										inputElement.wholeNumbers = true;

										inputElement.onValueChanged.AddListener(value =>
										{
											currentValueElement.text = value.ToString(CultureInfo.InvariantCulture);
											configEntry.SetSerializedValue(value.ToString(CultureInfo.InvariantCulture));
										});

										break;
									}
									case AcceptableValueRange<float> floatRange:
									{
										var inputElementContainer = Instantiate(sliderPrefab, configElement.inputContainerElement.transform);
										var currentValueElement = inputElementContainer.transform.Find("CurrentValue").GetComponent<Text>();
										var inputElement = inputElementContainer.transform.Find("Slider").GetComponent<Slider>();
										inputElement.value = float.Parse(configEntry.BoxedValue.ToString());
										inputElementContainer.transform.Find("CurrentValue").GetComponent<Text>().text = configEntry.BoxedValue.ToString();
										inputElement.minValue = floatRange.MinValue;
										inputElement.maxValue = floatRange.MaxValue;
										inputElement.wholeNumbers = false;

										inputElement.onValueChanged.AddListener(value =>
										{
											currentValueElement.text = value.ToString(CultureInfo.InvariantCulture);
											configEntry.SetSerializedValue(value.ToString(CultureInfo.InvariantCulture));
										});

										break;
									}
									case AcceptableValueList<bool> boolList:
									{
										var inputElement = Instantiate(togglePrefab, configElement.inputContainerElement.transform).GetComponent<Toggle>();
										inputElement.isOn = bool.Parse(configEntry.BoxedValue.ToString());

										inputElement.onValueChanged.AddListener(value => configEntry.SetSerializedValue(value.ToString(CultureInfo.InvariantCulture)));

										break;
									}
									default:
									{
										var inputElement = Instantiate(dropDownPrefab, configElement.inputContainerElement.transform).GetComponent<Dropdown>();

										if (configEntry.SettingType.IsEnum)
										{
											var values = Enum.GetNames(configEntry.SettingType);

											inputElement.options.AddRange(values.Select(value => new Dropdown.OptionData(value)));
											inputElement.value = Array.IndexOf(values, configEntry.BoxedValue);

											inputElement.onValueChanged.AddListener(value => configEntry.SetSerializedValue(values[value].ToString(CultureInfo.InvariantCulture)));
										}
										else
										{
											var valuesString = configEntry.Description.AcceptableValues.ToDescriptionString();
											var values = valuesString.Trim().Replace("# Acceptable values: ", "").Split(',');

											inputElement.options.AddRange(values.Select(value => new Dropdown.OptionData(value)));
											inputElement.value = Array.IndexOf(values, configEntry.BoxedValue);

											inputElement.onValueChanged.AddListener(value => configEntry.SetSerializedValue(values[value].ToString(CultureInfo.InvariantCulture)));
										}

										break;
									}
								}
							}
						}
					}
				}
			}

			var mainCanvas = FindObjectOfType<Canvas>();
			if (mainCanvas == null)
			{
				var canvasObject = new GameObject("ConfigUI_Canvas");
				mainCanvas = canvasObject.AddComponent<Canvas>();
				mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
				mainCanvas.sortingOrder = 1337;
				canvasObject.AddComponent<CanvasScaler>();
				canvasObject.AddComponent<GraphicRaycaster>();
			}

			transform.SetParent(mainCanvas.transform, false);			
			StartCoroutine(RebuildLayoutNextFrames());
		}

		private IEnumerator RebuildLayoutNextFrames()
		{
			yield return new WaitForEndOfFrame();
			
			Canvas.ForceUpdateCanvases();
			
			yield return new WaitForEndOfFrame();

			LayoutRebuilder.ForceRebuildLayoutImmediate(modContainerElement.GetComponent<RectTransform>());

			yield return new WaitForEndOfFrame();

			uiContainerElement.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
		}

		private void SaveConfig()
		{
			foreach (var configFile in _configFiles.Values.SelectMany(configFiles => configFiles))
			{
				configFile.Save();
			}
		}
	}
}