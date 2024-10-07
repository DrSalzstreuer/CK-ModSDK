using CK_QOL.Core.Config;

namespace CK_QOL.Features.NoDeathPenalty
{
	/// <summary>
	///     Configuration class for the <see cref="NoDeathPenalty" /> feature, handling the enabled state.
	///     This class uses <see cref="ConfigBase{TFeature}" /> to manage the configuration settings for NoDeathPenalty.
	/// </summary>
	internal sealed class NoDeathPenaltyConfig : ConfigBase<NoDeathPenalty>
	{
		protected override bool DefaultIsEnabled => false;
	}
}