using CK_QOL.Core.Config;

namespace CK_QOL.Features.ShiftClick
{
	/// <summary>
	///     Configuration class for the <see cref="ShiftClick" /> feature, handling the key binding and enabled state.
	///     This class uses <see cref="ConfigBase{TFeature}" /> to manage the configuration settings for ShiftClick.
	/// </summary>
	internal sealed class ShiftClickConfig : ConfigBase<ShiftClick>
	{
		protected override bool DefaultIsEnabled => true;
	}
}