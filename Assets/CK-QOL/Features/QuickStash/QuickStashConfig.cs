using CK_QOL.Core.Config;
using CoreLib.Data.Configuration;

namespace CK_QOL.Features.QuickStash
{
	/// <summary>
	///     Configuration class for the <see cref="QuickStash" /> feature, handling key binding, enabled state, range, and max chests.
	///     This class uses <see cref="ConfigBase{TFeature}" /> to manage the configuration settings for QuickStash.
	/// </summary>
	internal sealed class QuickStashConfig : ConfigBase<QuickStash>
	{
		internal ConfigEntry<float> MaxRange { get; private set; }
		internal ConfigEntry<int> MaxChests { get; private set; }

		internal override void Initialize(QuickStash feature)
		{
			base.Initialize(feature);

			MaxRange = ApplyMaxRange();
			MaxChests = ApplyMaxChests();
		}

		private ConfigEntry<float> ApplyMaxRange()
		{
			var acceptableValues = new AcceptableValueRange<float>(1f, 50f);
			var description = new ConfigDescription("The maximum range to detect nearby chests.", acceptableValues);
			var definition = new ConfigDefinition(Feature.Name, nameof(Feature.MaxRange));

			return Config.Bind(definition, 50f, description);
		}

		private ConfigEntry<int> ApplyMaxChests()
		{
			var acceptableValues = new AcceptableValueRange<int>(1, 50);
			var description = new ConfigDescription("The maximum number of chests to include.", acceptableValues);
			var definition = new ConfigDefinition(Feature.Name, nameof(Feature.MaxChests));

			return Config.Bind(definition, 50, description);
		}
	}
}