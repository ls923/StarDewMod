using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarDew_Mod_1.FrameWork.Configs;
using StarDew_Mod_1.FrameWork.EventSystem;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Minigames;

namespace StarDew_Mod_1.FrameWork.Core
{
    public class GameManager : Singleton<GameManager>
    {

        private readonly Lazy<Texture2D> Texture = new(CreateTexture);

        public GameManager() { }

        public void Init()
        {
            AddEvent();
        }

        private void AddEvent()
        {
            EventManager.Instance.AddEvent<SButton>(ClientEvent.PRESS_BUTTON, OnBtnPressed);
        }

        public void Update()
        {

        }

        #region DrawUI

        /// <summary>
        /// 创建一个Texture2D
        /// </summary>
        private static Texture2D CreateTexture()
        {
            Texture2D texture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            texture.SetData(new[] { Color.White });
            return texture;
        }

        /// <summary>在屏幕上绘制UI
        /// <param name="batch">The sprite batch being drawn.</param>
        /// <param name="font">The font with which to render text.</param>
        /// </summary>
        public void DrawOverlay(SpriteBatch batch, SpriteFont font)
        {
            var viewport = Game1.uiMode ? Game1.uiViewport : Game1.viewport;
            int mouseX = Game1.getMouseX();
            int mouseY = Game1.getMouseY();

            // 在鼠标处绘制
            {
                string[] lines = GetDebugInfo().ToArray();

                // get text
                string text = string.Join(Environment.NewLine, lines.Where(p => p != null)!);
                Vector2 textSize = font.MeasureString(text);
                const int scrollPadding = 5;

                // calculate scroll position
                Rectangle rect = new(325, 318, 6, 2);

                int width = (int)(textSize.X + (scrollPadding * 2) + (rect.Width * Game1.pixelZoom * 2));
                int height = (int)(textSize.Y + (scrollPadding * 2) + (rect.Height * Game1.pixelZoom * 2));
                int x = MathHelper.Clamp(mouseX - width, 0, viewport.Width - width);
                int y = MathHelper.Clamp(mouseY, 0, viewport.Height - height);

                // draw
                DrawContentBox(batch, Texture.Value, new Vector2(x, y), textSize, out Vector2 contentPos, out _, padding: scrollPadding);
                batch.DrawString(font, text, new Vector2(contentPos.X, contentPos.Y), Color.Black);
            }

            // draw cursor crosshairs
            //batch.Draw(Texture.Value, new Rectangle(0, mouseY - 1, viewport.Width, 3), Color.Black * 0.5f);
            //batch.Draw(Texture.Value, new Rectangle(mouseX - 1, 0, 3, viewport.Height), Color.Black * 0.5f);
        }

        public static void DrawContentBox(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Vector2 contentSize, out Vector2 contentPos, out Rectangle bounds, int padding)
        {
            Rectangle background = new(334, 321, 1, 1);
            Rectangle top = new(331, 318, 1, 2);
            Rectangle bottom = new(327, 334, 1, 2);
            Rectangle left = new(325, 320, 6, 1);
            Rectangle right = new(344, 320, 6, 1);
            Rectangle topLeft = new(325, 318, 6, 2);
            Rectangle topRight = new(344, 318, 6, 2);
            Rectangle bottomLeft = new(325, 334, 6, 2);
            Rectangle bottomRight = new(344, 334, 6, 2);

            var borderWidth = topLeft.Width * Game1.pixelZoom;
            var borderHeight = topLeft.Height * Game1.pixelZoom;
            var innerWidth = (int)(contentSize.X + padding * 2);
            var innerHeight = (int)(contentSize.Y + padding * 2);
            var outerWidth = innerWidth + borderWidth * 2;
            var outerHeight = innerHeight + borderHeight * 2;

            int x = (int)position.X;
            int y = (int)position.Y;


            // draw scroll background
            spriteBatch.Draw(texture, new Rectangle(x + borderWidth, y + borderHeight, innerWidth, innerHeight), background, Color.White);

            // draw borders
            spriteBatch.Draw(texture, new Rectangle(x + borderWidth, y, innerWidth, borderHeight), top, Color.White);
            spriteBatch.Draw(texture, new Rectangle(x + borderWidth, y + borderHeight + innerHeight, innerWidth, borderHeight), bottom, Color.White);
            spriteBatch.Draw(texture, new Rectangle(x, y + borderHeight, borderWidth, innerHeight), left, Color.White);
            spriteBatch.Draw(texture, new Rectangle(x + borderWidth + innerWidth, y + borderHeight, borderWidth, innerHeight), right, Color.White);

            // draw corners
            spriteBatch.Draw(texture, new Rectangle(x, y, borderWidth, borderHeight), topLeft, Color.White);
            spriteBatch.Draw(texture, new Rectangle(x, y + borderHeight + innerHeight, borderWidth, borderHeight), bottomLeft, Color.White);
            spriteBatch.Draw(texture, new Rectangle(x + borderWidth + innerWidth, y, borderWidth, borderHeight), topRight, Color.White);
            spriteBatch.Draw(texture, new Rectangle(x + borderWidth + innerWidth, y + borderHeight + innerHeight, borderWidth, borderHeight), bottomRight, Color.White);

            // set out params
            contentPos = new Vector2(x + borderWidth + padding, y + borderHeight + padding);
            bounds = new Rectangle(x, y, outerWidth, outerHeight);
        }

        /// <summary>Get debug info for the current context.</summary>
        private IEnumerable<string> GetDebugInfo()
        {
            // location
            if (Game1.currentLocation != null)
            {
                Vector2 tile = Game1.currentCursorTile;

                yield return $"坐标: {tile.X}, {tile.Y}";
                yield return $"地图:  {Game1.currentLocation.Name}";

                // Absolute position => Tile position
                var tilePosX = Math.Floor(Game1.player.Position.X / Game1.tileSize);
                var tilePosY = Math.Floor(Game1.player.Position.Y / Game1.tileSize);

                // Tilemap dimensions
                var tileWidth = Math.Floor((float)Game1.player.currentLocation.Map.DisplayWidth / Game1.tileSize);
                var tileHeight = Math.Floor((float)Game1.player.currentLocation.Map.DisplayHeight / Game1.tileSize);


                yield return $"pos:  {Game1.player.Position.X},{Game1.player.Position.Y}";
                yield return $"tilePos: {tilePosX},{tilePosY}";
                yield return $"Tilemap dimensions: {tileWidth},{tileHeight}";

            }

            // menu
            if (Game1.activeClickableMenu != null)
            {
                Type menuType = Game1.activeClickableMenu.GetType();
                Type? submenuType = this.GetSubmenu(Game1.activeClickableMenu)?.GetType();
                string? vanillaNamespace = typeof(TitleMenu).Namespace;

                yield return $"菜单: {(menuType.Namespace == vanillaNamespace ? menuType.Name : menuType.FullName)}";
                if (submenuType != null)
                    yield return $"子菜单: {(submenuType.Namespace == vanillaNamespace ? submenuType.Name : submenuType.FullName)}";
            }

            // minigame
            if (Game1.currentMinigame != null)
            {
                Type minigameType = Game1.currentMinigame.GetType();
                string? vanillaNamespace = typeof(AbigailGame).Namespace;

                yield return $"小游戏: {(minigameType.Namespace == vanillaNamespace ? minigameType.Name : minigameType.FullName)}";
            }

            // event
            if (Game1.CurrentEvent != null)
            {
                Event curEvent = Game1.CurrentEvent;
                double progress = curEvent.CurrentCommand / (double)curEvent.eventCommands.Length;

                if (curEvent.isFestival)
                    yield return $"节日: {curEvent.FestivalName}";

                yield return $"事件ID: {curEvent.id}";

                if (!curEvent.isFestival && curEvent.CurrentCommand >= 0 && curEvent.CurrentCommand < curEvent.eventCommands.Length)
                    yield return $"事件脚本: {curEvent.GetCurrentCommand()} ({(int)(progress * 100)}%)";
            }

            // music
            if (Game1.currentSong is { Name: not null, IsPlaying: true })
                yield return $"音乐: {Game1.currentSong.Name}";
        }

        /// <summary>Get the submenu for the current menu, if any.</summary>
        /// <param name="menu">The submenu.</param>
        private IClickableMenu? GetSubmenu(IClickableMenu menu)
        {
            return menu switch
            {
                GameMenu gameMenu => gameMenu.pages[gameMenu.currentTab],
                TitleMenu => TitleMenu.subMenu,
                _ => null
            };
        }


        #endregion

        #region Event

        private void OnBtnPressed(SButton button)
        {
            if (ConfigManager.Instance.ModCfg.KeyBinding == button)
            {
                ConfigManager.Instance.ToggleShowUI();
            }
            switch (button)
            {
                case SButton.L:
                    var currentLocation = Game1.player.currentLocation;
                    if (currentLocation != null)
                    {
                        var tilePosX = Math.Floor(Game1.player.Position.X / Game1.tileSize);
                        var tilePosY = Math.Floor(Game1.player.Position.Y / Game1.tileSize);
                        var obj = new StardewValley.Object($"{107}", 1);
                        var hasDrop = currentLocation.dropObject(obj, new Vector2((float)((tilePosX - 1) * Game1.tileSize), (float)(tilePosY * Game1.tileSize)), Game1.viewport, true, null);
                        if (hasDrop)
                        {
                            HappyBirthdayMod.Instance.Monitor.Log($"在 {currentLocation.Name} [{tilePosX - 1},{tilePosY}] 创建了一个 掉落物品 {obj.Name}", LogLevel.Info);
                        }
                    }
                    break;
                case SButton.P:
                    if (Context.IsWorldReady)
                    {
                        if (Game1.currentSong is { Name: not null, IsPlaying: true, IsPaused: false })
                        {
                            Game1.currentSong.Pause();
                            HappyBirthdayMod.Instance.Monitor.Log($"暂停播放 {Game1.currentSong.Name}", LogLevel.Debug);
                        }
                        else if (Game1.currentSong is { Name: not null, IsPaused: true })
                        {
                            Game1.currentSong.Play();
                            HappyBirthdayMod.Instance.Monitor.Log($"恢复播放 {Game1.currentSong.Name}", LogLevel.Debug);
                        }
                    }
                    break;
            }
        }

        #endregion
    }
}
