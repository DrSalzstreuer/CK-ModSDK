using CK_QOL.Core.Config;
using CoreLib.Data.Configuration;

namespace CK_QOL.Features.QuickSummon
{
	/// <summary>
	///     Configuration class for the <see cref="QuickSummon" /> feature, handling key binding, enabled state, and equipment slot.
	///     This class uses <see cref="ConfigBase{TFeature}" /> to manage the configuration settings for QuickSummon.
	/// </summary>
	internal sealed class QuickSummonConfig : ConfigBase<QuickSummon>
	{
		protected override bool DefaultIsEnabled => true;
		internal ConfigEntry<int> EquipmentSlotIndex { get; private set; }

		internal override void Initialize(QuickSummon feature)
		{
			base.Initialize(feature);

			EquipmentSlotIndex = ApplyEquipmentSlotIndex();
		}

		private ConfigEntry<int> ApplyEquipmentSlotIndex()
		{
			var acceptableValues = new AcceptableValueRange<int>(0, 9);
			var description = new ConfigDescription("The equipment slot index for summoning tomes.", acceptableValues);
			var definition = new ConfigDefinition(Feature.Name, nameof(Feature.EquipmentSlotIndex));

			return Config.Bind(definition, 0, description);
		}
	}
}