using CK_QOL.ConfigUI.Core.Helpers;
using HarmonyLib;

namespace CK_QOL.ConfigUI.Core.Patches
{
	[HarmonyPatch(typeof(TitleScreenAnimator))]
	public class TitleScreenAnimatorPatches
	{
		[HarmonyPostfix]
		[HarmonyPatch(nameof(OpenMenu))]
		public static void OpenMenu()
		{
			AssetHelper.LoadAndInstantiatePrefab("ConfigUI");
		}
	}
}