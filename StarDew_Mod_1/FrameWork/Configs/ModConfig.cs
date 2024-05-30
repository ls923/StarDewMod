using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarDew_Mod_1.FrameWork.Configs
{
    public class ModConfig
    {
        /// <summary>
        /// 显示菜单按钮
        /// </summary>
        public SButton KeyBinding { get; set; } = SButton.O;

        /// <summary>
        /// 获得NPC祝福 最低NPC 好感等级
        /// </summary>
        public int MinimumFriendshipLevelForBirthdayWish = 1;

        /// <summary>
        /// The minimum amount of friendship needed with all villagers that would be present to get the saloon birthday party;
        /// 参加沙龙需要达到好感的村民最低数量 
        /// </summary>
        public int MinimumFriendshipLevelForCommunityBirthdayParty = 2;


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
