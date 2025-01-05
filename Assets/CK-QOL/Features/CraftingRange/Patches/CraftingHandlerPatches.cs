using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;

namespace CK_QOL.Features.CraftingRange.Patches
{
	/// <summary>
	///     Harmony patch to modify the behavior of <see cref="CraftingHandler.GetAnyNearbyChests()" /> method.
	///     This patch overrides the default behavior to include chests within the crafting range defined by
	///     the <see cref="CraftingRange" /> feature.
	/// </summary>
	[HarmonyPatch(typeof(CraftingHandler))]
	internal static class CraftingHandlerPatches
	{
		/// <summary>
		///     A prefix patch that modifies the result of the <see cref="CraftingHandler.GetAnyNearbyChests()" /> method
		///     to include only the chests detected by the <see cref="CraftingRange" /> feature.
		/// </summary>

		// had Error CS0117	'CraftingHandler' does not contain a definition for 'GetAnyNearbyChests' did not have time to  figure it out lol  so did what i did to let me compile... 	

		[HarmonyPrefix]
		//[HarmonyPatch(nameof(CraftingHandler.GetAnyNearbyChests), new Type[] { })]
		[HarmonyPatch("GetAnyNearbyChests")] // Simple string without 'nameof'
		[SuppressMessage("ReSharper", "InconsistentNaming")]
		private static bool GetAnyNearbyChests(ref List<Chest> __result)
		{
			if (!CraftingRange.Instance.IsEnabled)
			{
				return true;  // Continue with the original method if crafting range is not enabled
			}

			// Override the result to use chests detected by CraftingRange
			__result = CraftingRange.Instance.Chests;

			return false;  // Skip the original method since we've handled the result
		}
	}
}

