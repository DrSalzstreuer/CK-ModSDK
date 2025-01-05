using CK_QOL.Core;
using CK_QOL.Core.Features;
using CoreLib.RewiredExtension;
using Rewired;

namespace CK_QOL.Features.QuickSummon
{
	internal sealed class QuickSummon : FeatureBase<QuickSummon, QuickSummonConfig>
	{
		private int _fromSlotIndex = -1;
		private int _previousSlotIndex = -1;
		private ObjectID _tomeID = ObjectID.None;

		public QuickSummon()
		{
			SetupKeyBindings();
		}

		public override bool CanExecute()
		{
			return base.CanExecute() && Entry.RewiredPlayer != null && Manager.main.player != null && !(Manager.input?.textInputIsActive ?? false);
		}

		public override void Update()
		{
			if (!CanExecute())
			{
				return;
			}

			// Map key bindings to tome IDs
			if (Entry.RewiredPlayer.GetButtonDown(KeyBindNameX))
			{
				_tomeID = ObjectID.TomeOfRange;
				Execute();
			}
			if (Entry.RewiredPlayer.GetButtonDown(KeyBindNameC))
			{
				_tomeID = ObjectID.TomeOfFire;
				Execute();
			}
			if (Entry.RewiredPlayer.GetButtonDown(KeyBindNameV))
			{
				_tomeID = ObjectID.TomeOfPoison;
				Execute();
			}
			if (Entry.RewiredPlayer.GetButtonDown(KeyBindNameI))
			{
				_tomeID = ObjectID.TomeOfMelee;
				Execute();
			}
			if (Entry.RewiredPlayer.GetButtonDown(KeyBindNameO))
			{
				_tomeID = ObjectID.TomeOfOrbit;
				Execute();
			}
			if (Entry.RewiredPlayer.GetButtonDown(KeyBindNameP))
			{
				_tomeID = ObjectID.TomeOfRadiation;
				Execute();
			}

			// Handle releasing key bindings
			if (Entry.RewiredPlayer.GetButtonUp(KeyBindNameX) ||
				Entry.RewiredPlayer.GetButtonUp(KeyBindNameC) ||
				Entry.RewiredPlayer.GetButtonUp(KeyBindNameV) ||
				Entry.RewiredPlayer.GetButtonUp(KeyBindNameI) ||
				Entry.RewiredPlayer.GetButtonUp(KeyBindNameO) ||
				Entry.RewiredPlayer.GetButtonUp(KeyBindNameP))
			{
				_tomeID = ObjectID.None;
				SwapBackToPreviousSlot();
			}
		}

		public override void Execute()
		{
			var player = Manager.main.player;

			if (TryFindSummonTome(player))
			{
				CastSummonSpell(player);
			}
		}

		private bool TryFindSummonTome(PlayerController player)
		{
			_previousSlotIndex = player.equippedSlotIndex;
			_fromSlotIndex = -1;

			if (IsSummonTome(player.playerInventoryHandler.GetObjectData(EquipmentSlotIndex), _tomeID))
			{
				_fromSlotIndex = EquipmentSlotIndex;
				return true;
			}

			var playerInventorySize = player.playerInventoryHandler.size;
			for (var playerInventoryIndex = 0; playerInventoryIndex < playerInventorySize; playerInventoryIndex++)
			{
				if (IsSummonTome(player.playerInventoryHandler.GetObjectData(playerInventoryIndex), _tomeID))
				{
					_fromSlotIndex = playerInventoryIndex;
					return true;
				}
			}

			return false;
		}

		private bool IsSummonTome(ObjectDataCD objectData, ObjectID tomeID)
		{
			return objectData.objectID == tomeID;
		}

		private void CastSummonSpell(PlayerController player)
		{
			if (_fromSlotIndex != EquipmentSlotIndex)
			{
				player.playerInventoryHandler.Swap(player, _fromSlotIndex, player.playerInventoryHandler, EquipmentSlotIndex);
			}

			player.EquipSlot(EquipmentSlotIndex);

			var inputHistory = EntityUtility.GetComponentData<ClientInputHistoryCD>(player.entity, player.world);
			inputHistory.secondInteractUITriggered = true;
			EntityUtility.SetComponentData(player.entity, player.world, inputHistory);

			if (_fromSlotIndex != EquipmentSlotIndex && player.playerInventoryHandler.GetObjectData(_fromSlotIndex).objectID != ObjectID.None)
			{
				player.playerInventoryHandler.Swap(player, _fromSlotIndex, player.playerInventoryHandler, EquipmentSlotIndex);
			}

			EntityUtility.SetComponentData(player.entity, player.world, inputHistory);
		}

		private void SwapBackToPreviousSlot()
		{
			if (_previousSlotIndex == -1)
			{
				return;
			}

			Manager.main.player.EquipSlot(_previousSlotIndex);
		}

		#region IFeature

		public override string Name => nameof(QuickSummon);

		public override string DisplayName => "Quick Summon";

		public override string Description => "Quickly equips or switches the preferred summoning tome, casts a summon spell, and swaps back to the previous item.";

		public override FeatureType FeatureType => FeatureType.Client;

		#endregion IFeature

		#region Configurations

		internal string KeyBindNameX => $"{ModSettings.ShortName}_{Name}-TomeOfTheDark";
		internal string KeyBindNameC => $"{ModSettings.ShortName}_{Name}-TomeOfAshes";
		internal string KeyBindNameV => $"{ModSettings.ShortName}_{Name}-TomeOfPestilence";
		internal string KeyBindNameI => $"{ModSettings.ShortName}_{Name}-TomeOfTheDead";
		internal string KeyBindNameO => $"{ModSettings.ShortName}_{Name}-TomeOfTheDeep";
		internal string KeyBindNameP => $"{ModSettings.ShortName}_{Name}-TomeOfDecay";

		internal int EquipmentSlotIndex => Config.EquipmentSlotIndex.Value;

		public void SetupKeyBindings()
		{
			// Existing tomes with default hotkeys
			RewiredExtensionModule.AddKeybind(KeyBindNameX, "Quick Summon Tome of the Dark", KeyboardKeyCode.X);
			RewiredExtensionModule.AddKeybind(KeyBindNameC, "Quick Summon Tome of the Dead", KeyboardKeyCode.None);
			RewiredExtensionModule.AddKeybind(KeyBindNameV, "Quick Summon Tome of the Deep", KeyboardKeyCode.None);
			// New tomes without default hotkeys (user can assign them later)
			RewiredExtensionModule.AddKeybind(KeyBindNameI, "Quick Summon Tome of Ashes", KeyboardKeyCode.None);
			RewiredExtensionModule.AddKeybind(KeyBindNameO, "Quick Summon Tome of Pestilence", KeyboardKeyCode.None);
			RewiredExtensionModule.AddKeybind(KeyBindNameP, "Quick Summon Tome of Decay", KeyboardKeyCode.None);
		}
	}

		#endregion Configurations
	}

