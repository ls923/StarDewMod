using Microsoft.Xna.Framework;
using StarDew_Mod_1.FrameWork.EventSystem;
using StardewModdingAPI;
using StardewValley;

namespace StarDew_Mod_1.FrameWork.Core
{
    public class GameManager : Singleton<GameManager>
    {

        public GameManager() { }

        public void Init()
        {
            AddEvent();
        }

        private void AddEvent()
        {
            EventManager.Instance.AddEvent<SButton>(ClientEvent.PRESS_BUTTON, OnBtnPressed);
        }

        private void OnBtnPressed(SButton button)
        {
            switch (button)
            {
                case SButton.L:
                    var location = Game1.getLocationFromName("Farm");
                    var currentLocation = Game1.player.currentLocation;
                    if (currentLocation != null)
                    {
                        currentLocation.dropObject(new StardewValley.Object($"{107}", 1), new Vector2(Game1.player.Position.X, Game1.player.Position.Y), Game1.viewport, true, null);
                    }
                    break;
            }
        }
    }
}
