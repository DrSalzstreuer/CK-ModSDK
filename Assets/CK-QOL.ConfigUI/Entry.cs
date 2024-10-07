using System.Linq;
using CK_QOL.ConfigUI.Core;
using CoreLib;
using CoreLib.RewiredExtension;
using CoreLib.Util.Extensions;
using PugMod;
using Rewired;
using UnityEngine;

namespace CK_QOL.ConfigUI
{
	public class Entry : IMod
	{
		internal static LoadedMod ModInfo { get; private set; }
		internal static AssetBundle AssetBundle => ModInfo.AssetBundles.First();
		internal static Player RewiredPlayer { get; private set; }

		public void EarlyInit()
		{
			InitializeModInfo();
			LoadModules();
		}

		public void Init()
		{
			ModLogger.Info("Mod successfully initialized.");
		}

		public void Shutdown()
		{
			ModLogger.Warn("Mod shutdown initiated.");
		}

		public void ModObjectLoaded(Object obj)
		{
			if (obj is not GameObject gameObject)
			{
				return;
			}
		}

		public void Update()
		{
			if (RewiredPlayer.GetButtonDown(ModSettings.ShortName))
			{
				var configUI = Object.FindAnyObjectByType<UI.ConfigUI>();
				configUI?.ToggleUI();
			}
		}

		private void InitializeModInfo()
		{
			ModLogger.Info($"{ModSettings.Name} v{ModSettings.Version} by {ModSettings.Author} with special thanks to {ModSettings.SpecialThanks}");

			ModInfo = this.GetModInfo();
			if (ModInfo is null)
			{
				ModLogger.Error("Failed to load mod information.");
				Shutdown();
			}
		}

		private static void LoadModules()
		{
			CoreLibMod.LoadModule(typeof(RewiredExtensionModule));

			RewiredExtensionModule.rewiredStart += () => RewiredPlayer = ReInput.players.GetPlayer(0);
			RewiredExtensionModule.AddKeybind(ModSettings.ShortName, "Config UI", KeyboardKeyCode.Y, ModifierKey.Control);
		}
	}
}