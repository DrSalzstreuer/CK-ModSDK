using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreLib.Data.Configuration;
using PugMod;

namespace CK_QOL_Collection.Core.Configuration
{
    /// <summary>
    ///     Handles the configuration settings for the CK_QOL_Collection mod.
    ///     Provides functionality to initialize and manage settings for various features.
    /// </summary>
    internal static class ConfigurationManager
    {
        /// <summary>
        ///     The prefix used for all keybinds associated with this mod.
        /// </summary>
        internal const string KeybindPrefix = "CK_QOL";

        /// <summary>
        ///     Dictionary to hold feature configuration sections dynamically.
        /// </summary>
        private static readonly Dictionary<string, IFeatureConfiguration> FeatureConfigurations = new();

        /// <summary>
        ///     Gets the configuration file used to store and manage settings for the mod.
        /// </summary>
        internal static ConfigFile ConfigFile { get; private set; }

        /// <summary>
        ///     Initializes the configuration settings for the mod.
        /// </summary>
        /// <param name="modInfo">The loaded mod information.</param>
        /// <returns>The initialized <see cref="ConfigFile" /> instance containing the mod's configuration.</returns>
        internal static ConfigFile Initialize(LoadedMod modInfo)
        {
            ConfigFile = new ConfigFile($"{Entry.Name}/{Entry.Name}.cfg", true, modInfo);

            // Dynamically load feature configurations
            LoadFeatureConfigurations();

            // Bind all feature configurations
            foreach (var featureConfig in FeatureConfigurations.Values)
            {
                featureConfig.BindSettings(ConfigFile);
            }

            return ConfigFile;
        }

        /// <summary>
        ///     Dynamically loads feature configurations into the FeatureConfigurations dictionary.
        /// </summary>
        private static void LoadFeatureConfigurations()
        {
            var featureConfigTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IFeatureConfiguration).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in featureConfigTypes)
            {
                var instance = (IFeatureConfiguration)Activator.CreateInstance(type);
                FeatureConfigurations[instance.SectionName] = instance;
            }
        }

        /// <summary>
        ///     Gets a specific feature configuration by its section name.
        /// </summary>
        /// <param name="sectionName">The section name of the feature configuration.</param>
        /// <returns>The corresponding <see cref="IFeatureConfiguration" /> instance.</returns>
        internal static IFeatureConfiguration GetFeatureConfiguration(string sectionName)
        {
            return FeatureConfigurations.GetValueOrDefault(sectionName);
        }
    }
}