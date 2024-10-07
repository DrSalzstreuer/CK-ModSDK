using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CK_QOL.ConfigUI.UI.Elements;
using CoreLib.Data.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace CK_QOL.ConfigUI.UI
{
	[HarmonyPatch]
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
		private static bool _isMenuOpen;

		[HarmonyPrefix]
		[HarmonyPatch(typeof(Cursor), "set_visible")]
		public static bool Cursor_set_visible(bool value)
		{
			return value || _isMenuOpen != true;
		}

		public void ToggleUI()
		{
			if (uiContainerElement.activeSelf)
			{
				HideUI();
			}
			else
			{
				ShowUI();
			}
		}

		public void ShowUI()
		{
			_isMenuOpen = true;

			uiContainerElement.SetActive(true);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			StartCoroutine(RebuildLayout());
		}

		public void HideUI()
		{
			_isMenuOpen = false;

			uiContainerElement.SetActive(false);
			StartCoroutine(RebuildLayout());
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

		private void Update()
		{
			if (Input.GetMouseButton(0))
			{
				Manager.input.DisableInput(1);
			}
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
			var modIndex = 0;

			foreach (var mod in _configFiles)
			{
				var useAlternativeColor = modIndex % 2 == 0;

				RenderMod(mod.Key, mod.Value, useAlternativeColor);
				modIndex++;
			}

			SetupMainCanvas();
			StartCoroutine(RebuildLayout());
		}

		private void RenderMod(string modName, List<ConfigFile> configFiles, bool useAlternativeColor = false)
		{
			var modElement = Instantiate(modPrefab, modContainerElement.transform).GetComponent<ModElement>();
			modElement.nameElement.GetComponent<Text>().text = modName;

			if (useAlternativeColor)
			{
				var currentColor = modElement.GetComponent<Image>().color;
				modElement.GetComponent<Image>().color = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
			}

			foreach (var configFile in configFiles)
			{
				var groupedEntries = configFile.Entries.GroupBy(entry => entry.Key.Section);
				foreach (var sectionGroup in groupedEntries)
				{
					RenderSection(modElement, sectionGroup.Key, sectionGroup);
				}
			}
		}

		private void RenderSection(ModElement modElement, string sectionName, IEnumerable<KeyValuePair<ConfigDefinition, ConfigEntryBase>> entries)
		{
			var sectionElement = Instantiate(sectionPrefab, modElement.sectionContainerElement.transform).GetComponent<SectionElement>();
			sectionElement.nameElement.GetComponent<Text>().text = sectionName;

			foreach (var entry in entries)
			{
				RenderConfig(sectionElement, entry.Key, entry.Value);
			}
		}

		private void RenderConfig(SectionElement sectionElement, ConfigDefinition key, ConfigEntryBase configEntry)
		{
			var configElement = Instantiate(configPrefab, sectionElement.configContainerElement.transform).GetComponent<ConfigElement>();
			var defaultValueText = configEntry.DefaultValue != null ? $" ({configEntry.DefaultValue})" : string.Empty;
			configElement.nameElement.GetComponent<Text>().text = $"{key.Key}{defaultValueText}";
			configElement.descriptionElement.GetComponent<Text>().text = configEntry.Description.Description;

			CreateInputElement(configElement, configEntry);
		}

		private void CreateInputElement(ConfigElement configElement, ConfigEntryBase configEntry)
		{
			if (configEntry.SettingType == typeof(bool))
			{
				CreateToggle(configElement, configEntry);
			}
			else if (configEntry.Description == ConfigDescription.Empty || configEntry.Description.AcceptableValues == null)
			{
				CreateInputField(configElement, configEntry);
			}
			else
			{
				CreateSpecificInputElement(configElement, configEntry);
			}
		}

		private void CreateInputField(ConfigElement configElement, ConfigEntryBase configEntry)
		{
			var inputElement = Instantiate(inputFieldPrefab, configElement.inputContainerElement.transform).GetComponent<InputField>();
			inputElement.gameObject.AddComponent<InputValidator>().ConfigEntry = configEntry;
			inputElement.text = configEntry.GetSerializedValue();
		}

		private void CreateSpecificInputElement(ConfigElement configElement, ConfigEntryBase configEntry)
		{
			var acceptableValues = configEntry.Description.AcceptableValues;

			switch (acceptableValues)
			{
				case AcceptableValueRange<int> intRange:
					CreateIntSlider(configElement, configEntry, intRange);

					break;

				case AcceptableValueRange<float> floatRange:
					CreateFloatSlider(configElement, configEntry, floatRange);

					break;

				default:
					CreateDropdown(configElement, configEntry);

					break;
			}
		}

		private void CreateIntSlider(ConfigElement configElement, ConfigEntryBase configEntry, AcceptableValueRange<int> intRange)
		{
			var inputElementContainer = Instantiate(sliderPrefab, configElement.inputContainerElement.transform);
			var currentValueElement = inputElementContainer.transform.Find("CurrentValue").GetComponent<Text>();
			var inputElement = inputElementContainer.transform.Find("Slider").GetComponent<Slider>();

			inputElement.minValue = intRange.MinValue;
			inputElement.maxValue = intRange.MaxValue;
			inputElement.wholeNumbers = true;
			inputElement.value = Convert.ToSingle(configEntry.GetSerializedValue());
			currentValueElement.text = configEntry.GetSerializedValue();

			inputElement.onValueChanged.AddListener(value =>
			{
				currentValueElement.text = value.ToString(CultureInfo.InvariantCulture);
				configEntry.SetSerializedValue(value.ToString(CultureInfo.InvariantCulture));
			});
		}

		private void CreateFloatSlider(ConfigElement configElement, ConfigEntryBase configEntry, AcceptableValueRange<float> floatRange)
		{
			var inputElementContainer = Instantiate(sliderPrefab, configElement.inputContainerElement.transform);
			var currentValueElement = inputElementContainer.transform.Find("CurrentValue").GetComponent<Text>();
			var inputElement = inputElementContainer.transform.Find("Slider").GetComponent<Slider>();

			inputElement.minValue = floatRange.MinValue;
			inputElement.maxValue = floatRange.MaxValue;
			inputElement.wholeNumbers = false;
			inputElement.value = Convert.ToSingle(configEntry.GetSerializedValue());
			currentValueElement.text = configEntry.GetSerializedValue();

			inputElement.onValueChanged.AddListener(value =>
			{
				currentValueElement.text = value.ToString(CultureInfo.InvariantCulture);
				configEntry.SetSerializedValue(value.ToString(CultureInfo.InvariantCulture));
			});
		}

		private void CreateToggle(ConfigElement configElement, ConfigEntryBase configEntry)
		{
			var inputElement = Instantiate(togglePrefab, configElement.inputContainerElement.transform).GetComponent<Toggle>();
			inputElement.isOn = Convert.ToBoolean(configEntry.GetSerializedValue());

			inputElement.onValueChanged.AddListener(value => { configEntry.SetSerializedValue(value.ToString(CultureInfo.InvariantCulture)); });
		}

		private void CreateDropdown(ConfigElement configElement, ConfigEntryBase configEntry)
		{
			var inputElement = Instantiate(dropDownPrefab, configElement.inputContainerElement.transform).GetComponent<Dropdown>();

			if (configEntry.SettingType.IsEnum)
			{
				var values = Enum.GetNames(configEntry.SettingType);
				inputElement.options.AddRange(values.Select(value => new Dropdown.OptionData(value)));

				var currentValue = configEntry.GetSerializedValue();
				inputElement.value = Array.IndexOf(values, currentValue);

				inputElement.onValueChanged.AddListener(index =>
				{
					var selectedValue = values[index];
					configEntry.SetSerializedValue(selectedValue.ToString(CultureInfo.InvariantCulture));
				});
			}
			else
			{
				var valuesString = configEntry.Description.AcceptableValues.ToDescriptionString();
				var values = valuesString.Trim().Replace("# Acceptable values: ", "").Split(',');

				inputElement.options.AddRange(values.Select(value => new Dropdown.OptionData(value.Trim())));

				var currentValue = configEntry.GetSerializedValue();
				inputElement.value = Array.IndexOf(values, currentValue);

				inputElement.onValueChanged.AddListener(index =>
				{
					var selectedValue = values[index];
					configEntry.SetSerializedValue(selectedValue.ToString(CultureInfo.InvariantCulture));
				});
			}
		}

		private void SetupMainCanvas()
		{
			var mainCanvas = FindObjectOfType<Canvas>();
			if (mainCanvas == null)
			{
				var canvasObject = new GameObject("ConfigUI_Canvas");
				mainCanvas = canvasObject.AddComponent<Canvas>();
				mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
				mainCanvas.sortingOrder = 1337;
				var mainCanvasScaler = canvasObject.AddComponent<CanvasScaler>();
				mainCanvasScaler.referenceResolution = new Vector2(1280, 720);
				mainCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
				canvasObject.AddComponent<GraphicRaycaster>();
				
				DontDestroyOnLoad(mainCanvas);
			}

			transform.SetParent(mainCanvas.transform, false);
			FindObjectOfType<WindowDragHandler>().Initialize(mainCanvas);
		}

		public IEnumerator RebuildLayout()
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