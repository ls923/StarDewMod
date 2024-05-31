using StardewModdingAPI;
using static StarDew_Mod_1.FrameWork.Gifts.GiftIDS;

namespace StarDew_Mod_1.FrameWork.Configs
{
    public class ModConfig
    {
        /// <summary>
        /// 显示UI按钮
        /// </summary>
        public SButton KeyBinding { get; set; } = SButton.O;

        /// <summary>
        /// 是否显示 UI
        /// </summary>
        public bool ShowUI { get;  set; }  = true;

        /// <summary>
        /// 获得NPC祝福 最低NPC 好感等级
        /// </summary>
        public int MinimumFriendshipLevelForBirthdayWish = 1;

        /// <summary>
        /// The minimum amount of friendship needed with all villagers that would be present to get the saloon birthday party;
        /// 参加沙龙需要达到好感的村民最低数量 
        /// </summary>
        public int MinimumFriendshipLevelForCommunityBirthdayParty = 2;

        /// <summary>
        /// 生日时父亲给的钱
        /// </summary>
        public int DadBirthdayYearMoneyGivenAmount = 161107;

        /// <summary>
        /// 生日时母亲给的礼物
        /// </summary>
        public int momBirthdayItemGive = (int)GiftId.PinkCake;

        /// <summary>
        ///  生日时母亲给的礼物数量
        /// </summary>
        public int momBirthdayItemGiveStackSize = 1;



        public ModConfig() { }

        public static ModConfig InitConfigs()
        {
            if (ConfigManager.Instance.IsConfigExist("ModConfig.json"))
            {
                return ConfigManager.Instance.ReadConfig<ModConfig>("ModConfig.json");
            }
            else
            {
                ModConfig Config = new ModConfig();
                ConfigManager.Instance.WriteConfig("ModConfig.json", Config);
                return Config;
            }
        }
    }
}
