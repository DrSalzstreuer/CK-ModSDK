using System.Collections.Generic;
using HarmonyLib;
using I2.Loc;
using UnityEngine;

namespace CK_QOL_Collection.Features.QuickStash.Patches
{
    [HarmonyPatch(typeof(ItemDiscoveryUI))]
    internal static class ItemDiscoveryUIPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(ItemDiscoveryUI.ShowDiscoveredItem), typeof(List<string>), typeof(Rarity))]
        private static bool ShowDiscoveredItem(ItemDiscoveryUI __instance, ref List<string> texts, Rarity rarity)
        {
            if (texts.Count is < 1 or > 1 && texts[0].StartsWith("-CK-QOL-"))
            {
                // Continue with the original method execution.
                return true;
            }

            var term = texts[0].Replace("-CK-QOL-", string.Empty);
            var text = $"-CK-QOL-{LocalizationManager.GetTranslation(term)}";
            
            int discoveryTextsIndex;
            for (discoveryTextsIndex = 0; discoveryTextsIndex < __instance.discoveryTexts.Count; discoveryTextsIndex++)
            {
                if (__instance.discoveryTexts[discoveryTextsIndex].gameObject.activeSelf)
                {
                    continue;
                }
                
                __instance.discoveryTexts[discoveryTextsIndex].Activate(text, rarity, __instance);
                break;
            }
            
            if (discoveryTextsIndex == __instance.discoveryTexts.Count)
            {
                var itemDiscoveryTextUI = Object.Instantiate(__instance.discoveryTextPrefab, __instance.container);
                __instance.discoveryTexts.Add(itemDiscoveryTextUI);
                itemDiscoveryTextUI.Activate(text, rarity, __instance);
            }
            
            var zero = Vector3.zero;
            for (var activeTextsCount = __instance.activeTexts.Count - 1; activeTextsCount >= 0; activeTextsCount--)
            {
                if (!__instance.activeTexts[activeTextsCount].gameObject.activeSelf)
                {
                    continue;
                }
                
                __instance.activeTexts[activeTextsCount].transform.localPosition = zero;
                zero += new Vector3(0f, __instance.activeTexts[activeTextsCount].pugText.dimensions.height, 0f);
            }
            
            // Skip the original method execution.
            return false;
        }
    }
}