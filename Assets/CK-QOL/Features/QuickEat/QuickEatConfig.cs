using CK_QOL.Core.Config;
using CoreLib.Data.Configuration;

namespace CK_QOL.Features.QuickEat
{
	/// <summary>
	///     Configuration class for the <see cref="QuickEat" /> feature, handling key binding, enabled state, and equipment slot.
	///     This class uses <see cref="ConfigBase{TFeature}" /> to manage the configuration settings for QuickEat.
	/// </summary>
	internal sealed class QuickEatConfig : ConfigBase<QuickEat>
	{
		protected override bool DefaultIsEnabled => true;
		internal ConfigEntry<int> EquipmentSlotIndex { get; private set; }

		internal override void Initialize(QuickEat feature)
		{
			base.Initialize(feature);

			EquipmentSlotIndex = ApplyEquipmentSlotIndex();
		}

		private ConfigEntry<int> ApplyEquipmentSlotIndex()
		{
			var acceptableValues = new AcceptableValueRange<int>(0, 9);
			var description = new ConfigDescription("The equipment slot index for eatable items.", acceptableValues);
			var definition = new ConfigDefinition(Feature.Name, nameof(Feature.EquipmentSlotIndex));

			return Config.Bind(definition, 8, description);
		}
	}
}