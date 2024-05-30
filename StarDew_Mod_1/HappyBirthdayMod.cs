using StarDew_Mod_1.FrameWork.Configs;
using StarDew_Mod_1.FrameWork.Gifts;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace StarDew_Mod_1
{
    public class HappyBirthdayMod :Mod
    {
        private Event _lastEvent = null;

        public static HappyBirthdayMod Instance;

     

        /// <summary>
        /// mod 入口
        /// </summary>
        /// <param name="helper"></param>
        public override void Entry(IModHelper helper)
        {
            Instance = this;
            ConfigManager.Instance.InitConfig();

            // 游戏事件监听
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            helper.Events.Input.ButtonPressed += OnButtonPressed;

            helper.Events.Display.RenderingActiveMenu += OnRenderingMenu;

            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;

            
        }

        /// <summary>
        /// 游戏加载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            //游戏加载完毕 执行mod 初始化
            //BirthdayMessagesManager.Init();
            GiftManager.Instance.Init();

        }

        [EventPriority(EventPriority.High)]
        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (_lastEvent != null && Game1.CurrentEvent == null)
                this.Monitor.Log($"Event {_lastEvent.id} just ended!");

            _lastEvent = Game1.CurrentEvent;
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            Monitor.Log($"{Game1.player.Name} DayStarted {Game1.year}/{Game1.season}/{Game1.dayOfMonth} []", LogLevel.Debug);
        }

        private void OnRenderingMenu(object? sender, RenderingActiveMenuEventArgs e)
        {
            //Monitor.Log($"{Game1.player.Name} rendering {e.SpriteBatch}.", LogLevel.Debug);
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet

            if (!Context.IsWorldReady)
                return;

            // print button presses to the console window
            this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
        }

        internal void RemoveUpdate(Action onUpdate)
        {
           // throw new NotImplementedException();//
        }

        internal void AddUpdate(Action onUpdate)
        {
           // throw new NotImplementedException();
        }
    }
}
