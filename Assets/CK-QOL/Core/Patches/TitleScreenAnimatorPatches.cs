using CK_QOL.Core.Helpers;
using HarmonyLib;

namespace CK_QOL.Core.Patches
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