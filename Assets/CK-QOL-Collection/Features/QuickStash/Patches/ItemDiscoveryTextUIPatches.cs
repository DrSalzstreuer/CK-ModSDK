using HarmonyLib;
using UnityEngine;

namespace CK_QOL_Collection.Features.QuickStash.Patches
{
    [HarmonyPatch(typeof(ItemDiscoveryTextUI))]
    internal static class ItemDiscoveryTextUIPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(ItemDiscoveryTextUI.Activate), typeof(string), typeof(Rarity), typeof(ItemDiscoveryUI))]
        private static bool ShowDiscoveredItem(ItemDiscoveryTextUI __instance, ref Color ___color, TimerSimple ___activeTimer, string text, Rarity rarity, ItemDiscoveryUI itemDiscoveryUI)
        {
            if (!text.StartsWith("-CK-QOL-"))
            {
                // Continue with the original method execution.
                return true;
            }

            text = $"Picked Up: {text.Replace("-CK-QOL-", string.Empty)}";
            
            __instance.itemDiscoveryUI = itemDiscoveryUI;
            itemDiscoveryUI.activeTexts.Add(__instance);
            //__instance.pugText.formatFields = new[] { text };
            __instance.pugText.Render(text);
            ___color = Manager.text.GetRarityColor(rarity);
            __instance.pugText.SetTempColor(___color);
            ___activeTimer.Start();
            __instance.gameObject.SetActive(value: true);

            // Skip the original method execution.
            return false;
        }
    }
}