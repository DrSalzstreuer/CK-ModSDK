using CK_QOL_Collection.Core;
using CK_QOL_Collection.Core.Configuration;
using CK_QOL_Collection.Core.Helpers;
using Rewired;

namespace CK_QOL_Collection.Features.QuickStash
{
    /// <summary>
    /// Represents the Quick Stash feature of the mod.
    /// This feature allows players to quickly stash items into nearby chests.
    /// </summary>
    internal class Feature : FeatureBase
    {
        private readonly Configuration _config;
        private readonly Player _rewiredPlayer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class.
        /// Sets up input handling for the Quick Stash feature.
        /// </summary>
        public Feature()
            : base(nameof(QuickStash))
        {
            _config = (Configuration)Configuration;
            _rewiredPlayer = Entry.RewiredPlayer;
        }

        /// <inheritdoc />
        public override bool CanExecute() => base.CanExecute()
            && _rewiredPlayer != null
            && Manager.main.currentSceneHandler.isInGame
            && Manager.main.player?.playerInventoryHandler != null;

        /// <inheritdoc />
        public override void Execute()
        {
            if (!CanExecute())
            {
                return;
            }

            var player = Manager.main.player;
            var maxDistance = _config.Distance;
            var nearbyChests = ChestHelper.GetNearbyChests(maxDistance);

            // Iterate through the nearby chests and attempt to quick stash items.
            foreach (var chest in nearbyChests)
            {
                var inventoryHandler = chest.inventoryHandler;
                if (inventoryHandler == null)
                {
                    continue;
                }

                // Perform the quick stash action.
                player.playerInventoryHandler.QuickStack(player, inventoryHandler);
            }
        }

        /// <inheritdoc />
        public override void Update()
        {
            if (!CanExecute())
            {
                return;
            }

            // Check if the Quick Stash key binding has been pressed.
            if (_rewiredPlayer.GetButtonDown(_config.QuickStashKeyBindName))
            {
                Execute();
            }
        }

        /// <inheritdoc />
        protected override IFeatureKeyBind CreateKeyBind()
        {
            return new QuickStashKeyBind();
        }
    }
}