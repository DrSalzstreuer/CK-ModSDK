using CK_QOL.Core.Features;

namespace CK_QOL.Features.Wormhole
{
	/// <summary>
	///     The Wormhole feature allows players to teleport to other players by clicking on their map markers.
	///     This feature is configurable and requires a specific number of Ancient Gemstones to be spent on each teleportation.
	/// </summary>
	/// <remarks>
	///     The feature utilizes a configurable system where the required amount of Ancient Gemstones can be adjusted via the <see cref="WormholeConfig" /> class.
	///     By default, the player needs two Ancient Gemstones per teleportation, but this can be changed in the configuration.
	/// </remarks>
	internal sealed class Wormhole : FeatureBase<Wormhole, WormholeConfig>
	{
		#region IFeature

		public override string Name => nameof(Wormhole);
		public override string DisplayName => "Wormhole";
		public override string Description => "Allows the player to teleport to other players.";
		public override FeatureType FeatureType => FeatureType.Client;

		#endregion IFeature

		#region Configuration

		/// <summary>
		///     Gets the number of Ancient Gemstones required for each teleportation.
		///     This value is configurable and defaults to 3 if not set in the configuration.
		/// </summary>
		internal int RequiredAncientGemstones => Config.RequiredAncientGemstones.Value;

		internal bool AllMarkersAllowed => Config.AllMarkersAllowed.Value;

		#endregion Configuration
	}
}