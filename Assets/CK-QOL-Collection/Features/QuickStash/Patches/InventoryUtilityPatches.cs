using System;
using System.Collections.Generic;
using HarmonyLib;
using I2.Loc;
using Inventory;
using QFSW.QC;
using Unity.Entities;
using UnityEngine;

namespace CK_QOL_Collection.Features.QuickStash.Patches
{
    [HarmonyPatch(typeof(InventoryUtility))]
    internal static class InventoryUtilityPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(InventoryUtility.AddAmount), typeof(InventoryHandlerShared), typeof(Entity), typeof(int), typeof(ObjectID), typeof(int))]
        private static void AddAmount(InventoryHandlerShared inventoryHandlerShared, Entity inventory, int index, ObjectID objectID, int amount)
        {
            Manager.ui.ShowDiscoveredItemText(new List<string> { $"-CK-QOL-Items/{Enum.GetName(typeof(ObjectID), objectID)}" }, Rarity.Poor);
        }
    }
}