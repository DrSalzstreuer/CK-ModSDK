using CK_QOL.Core.Config;
using CoreLib.Data.Configuration;

namespace CK_QOL.Features.CraftingRange
{
	/// <summary>
	///     Provides configuration options for the "Crafting Range" feature, allowing customization of maximum range
	///     and the number of chests that can be included in the crafting range.
	/// </summary>
	internal class CraftingRangeConfig : ConfigBase<CraftingRange>
	{
		protected override bool DefaultIsEnabled => true;
		internal ConfigEntry<float> MaxRange { get; private set; }
		internal ConfigEntry<int> MaxChests { get; private set; }

		internal override void Initialize(CraftingRange feature)
		{
			base.Initialize(feature);
			
			MaxRange = ApplyMaxRange();
			MaxChests = ApplyMaxChests();
		}

		private ConfigEntry<float> ApplyMaxRange()
		{
			var acceptableValues = new AcceptableValueRange<float>(1f, 50f);
			var description = new ConfigDescription("Maximum range to determine nearby chests.", acceptableValues);
			var definition = new ConfigDefinition(Feature.Name, nameof(Feature.MaxRange));

			return Config.Bind(definition, 25f, description);
		}

		private ConfigEntry<int> ApplyMaxChests()
		{
			var acceptableValues = new AcceptableValueRange<int>(1, 8);
			var description = new ConfigDescription("Maximum number of chests to include in crafting range. More then 8 will break the game, because of a game restriction.", acceptableValues);
			var definition = new ConfigDefinition(Feature.Name, nameof(Feature.MaxChests));

			return Config.Bind(definition, 8, description);
		}
	}
}