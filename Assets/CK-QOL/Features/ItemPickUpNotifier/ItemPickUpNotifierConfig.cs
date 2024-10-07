using CK_QOL.Core.Config;
using CoreLib.Data.Configuration;

namespace CK_QOL.Features.ItemPickUpNotifier
{
	/// <summary>
	///     Configuration class for the <see cref="ItemPickUpNotifier" /> feature, handling the enabled state and aggregate delay.
	///     This class uses <see cref="ConfigBase{TFeature}" /> to manage the configuration settings for ItemPickUpNotifier.
	/// </summary>
	internal sealed class ItemPickUpNotifierConfig : ConfigBase<ItemPickUpNotifier>
	{
		protected override bool DefaultIsEnabled => true;
		internal ConfigEntry<float> AggregateDelay { get; private set; }

		internal override void Initialize(ItemPickUpNotifier feature)
		{
			base.Initialize(feature);
			
			AggregateDelay = ApplyAggregateDelay();
		}

		private ConfigEntry<float> ApplyAggregateDelay()
		{
			var acceptableValues = new AcceptableValueRange<float>(1f, 10f);
			var description = new ConfigDescription("The delay in seconds to aggregate picked-up items before displaying the notification.", acceptableValues);
			var definition = new ConfigDefinition(Feature.Name, nameof(Feature.AggregateDelay));

			return Config.Bind(definition, 2f, description);
		}
	}
}