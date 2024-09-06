using CK_QOL_Collection.Core.Configuration;
using CoreLib.RewiredExtension;
using Rewired;

namespace CK_QOL_Collection.Features.QuickStash
{
    /// <summary>
    /// Represents the key bindings for the Quick Stash feature.
    /// </summary>
    internal class QuickStashKeyBind : IFeatureKeyBind
    {
        private const string KeyBindPrefix = "CK_QOL";
        private const string FeatureName = "QuickStash";

        public string KeyBindName => KeyBindPrefix + "-" + FeatureName;
        public string KeyBindDescription => "Quick Stash";
        public KeyboardKeyCode DefaultKey => KeyboardKeyCode.A;
        public ModifierKey DefaultModifier => ModifierKey.Control;

        /// <inheritdoc />
        public void RegisterKeyBind()
        {
            RewiredExtensionModule.AddKeybind(KeyBindName, KeyBindDescription, DefaultKey, DefaultModifier);
        }
    }
}