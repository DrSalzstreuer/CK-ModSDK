using CK_QOL_Collection.Core.Configuration;

namespace CK_QOL_Collection.Features
{
    /// <summary>
    /// Provides a base implementation for features within the CK_QOL_Collection mod.
    /// Implements the <see cref="IFeature"/> interface and provides default behavior for feature execution and updating.
    /// </summary>
    internal abstract class FeatureBase : IFeature
    {
        /// <summary>
        /// Gets the configuration object for this feature.
        /// </summary>
        protected IFeatureConfiguration Configuration { get; }

        /// <summary>
        /// Gets the key binding object for this feature, if applicable.
        /// </summary>
        protected IFeatureKeyBind KeyBind { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureBase"/> class.
        /// </summary>
        /// <param name="name">The name of the feature.</param>
        protected FeatureBase(string name)
        {
            Name = name;
            Configuration = ConfigurationManager.GetFeatureConfiguration(name);
            KeyBind = CreateKeyBind();

            KeyBind?.RegisterKeyBind(); // Register the key binding if it exists.
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public bool IsEnabled => Configuration != null && Configuration.Enabled;

        /// <inheritdoc />
        public virtual bool CanExecute() => IsEnabled;

        /// <inheritdoc />
        public virtual void Execute() { }

        /// <inheritdoc />
        public virtual void Update() { }

        /// <summary>
        /// Creates the key binding object for the feature.
        /// </summary>
        /// <returns>The key binding object if applicable; otherwise, null.</returns>
        protected virtual IFeatureKeyBind CreateKeyBind()
        {
            // To be overridden by derived classes if they have a key binding.
            return null;
        }
    }
}