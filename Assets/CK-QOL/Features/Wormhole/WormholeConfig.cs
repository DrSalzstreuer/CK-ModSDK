using CK_QOL.Core.Config;
using CoreLib.Data.Configuration;

namespace CK_QOL.Features.Wormhole
{
	/// <summary>
	///     Provides configuration options for the Wormhole feature, allowing the user to define the number of Ancient Gemstones
	///     required for each teleportation.
	/// </summary>
	internal class WormholeConfig : ConfigBase<Wormhole>
	{
		protected override bool DefaultIsEnabled => true;
		internal ConfigEntry<int> RequiredAncientGemstones { get; private set; }
		internal ConfigEntry<bool> AllMarkersAllowed { get; private set; }


		internal override void Initialize(Wormhole feature)
		{
			base.Initialize(feature);

			RequiredAncientGemstones = ApplyRequiredAncientGemstones();
			AllMarkersAllowed = ApplyAllMarkersAllowed();
		}

		private ConfigEntry<int> ApplyRequiredAncientGemstones()
		{
			var acceptableValues = new AcceptableValueRange<int>(0, 25);
			var description = new ConfigDescription("The amount of Ancient Gemstones required for each teleportation.", acceptableValues);
			var definition = new ConfigDefinition(Feature.Name, nameof(Feature.RequiredAncientGemstones));

			return Config.Bind(definition, 5, description);
		}

		private ConfigEntry<bool> ApplyAllMarkersAllowed()
		{
			var description = new ConfigDescription("Are all markers allowed for teleportation?");
			var definition = new ConfigDefinition(Feature.Name, nameof(Feature.AllMarkersAllowed));

			return Config.Bind(definition, false, description);
		}
	}
}