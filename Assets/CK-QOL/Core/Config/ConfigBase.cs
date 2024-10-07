using CK_QOL.Core.Features;
using CoreLib.Data.Configuration;

namespace CK_QOL.Core.Config
{
	/// <summary>
	///     Abstract base class for managing feature configuration settings.
	///     Handles loading the configuration file for a feature and applying common configuration properties.
	/// </summary>
	/// <typeparam name="TFeature">The type of the feature being configured.</typeparam>
	internal abstract class ConfigBase<TFeature> where TFeature : IFeature
	{
		protected virtual bool DefaultIsEnabled => true;
		protected ConfigFile Config { get; private set; }
		protected TFeature Feature { get; private set; }
		internal ConfigEntry<bool> IsEnabled { get; private set; }
		
		internal virtual void Initialize(TFeature feature)
		{
			Feature = feature;
			Config = new ConfigFile($"{ModSettings.ShortName}/{Feature.Name}.cfg", true, Entry.ModInfo);
			IsEnabled = ApplyIsEnabled();
		}
		
		private ConfigEntry<bool> ApplyIsEnabled()
		{
			var description = new ConfigDescription($"Enable the '{Feature.DisplayName}' ({Feature.FeatureType}) feature? {Feature.Description}");
			var definition = new ConfigDefinition(Feature.Name, nameof(Feature.IsEnabled));

			return Config.Bind(definition, DefaultIsEnabled, description);
		}
	}
}