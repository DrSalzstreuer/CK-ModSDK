using CK_QOL_Collection.Core.Configuration;
using CoreLib.Data.Configuration;
using Rewired;

namespace CK_QOL_Collection.Features.QuickStash
{
    /// <summary>
    ///     Configuration for the 'Quick Stash' feature.
    /// </summary>
    internal class Configuration : IFeatureConfiguration
    {
        private ConfigEntry<bool> _enabledEntry;
        private ConfigEntry<float> _distanceEntry;
        private ConfigEntry<string> _quickStashKeyBindNameEntry;

        public string SectionName => "QuickStash";

        /// <inheritdoc />
        public bool Enabled => _enabledEntry.Value;

        /// <summary>
        ///     Gets the configured distance for detecting nearby chests.
        /// </summary>
        public float Distance => _distanceEntry.Value;

        /// <summary>
        ///     Gets the key binding name for the 'Quick Stash' action.
        /// </summary>
        public string QuickStashKeyBindName => _quickStashKeyBindNameEntry.Value;

        /// <inheritdoc />
        public void BindSettings(ConfigFile configFile)
        {
            var enabledAcceptableValues = new AcceptableValueList<bool>(true, false);
            var enabledDescription = new ConfigDescription("Enable the 'Quick Stash' feature?", enabledAcceptableValues);
            _enabledEntry = configFile.Bind(SectionName, nameof(Enabled), true, enabledDescription);
            
            var distanceAcceptableValues = new AcceptableValueRange<float>(5f, 50f);
            var distanceDescription = new ConfigDescription("Maximum distance to search for nearby chests.", distanceAcceptableValues);
            _distanceEntry = configFile.Bind(SectionName, nameof(Distance), 20f, distanceDescription);
            
            var keyBindDescription = new ConfigDescription("Key binding for the Quick Stash action.");
            _quickStashKeyBindNameEntry = configFile.Bind(SectionName, nameof(QuickStashKeyBindName), "QuickStashKey", keyBindDescription);
        }
    }
}
