using static StarDew_Mod_1.FrameWork.Gifts.GiftIDS;

namespace StarDew_Mod_1.FrameWork.Configs
{
    public class MailConfig
    {
        public int DadBirthdayYearMoneyGivenAmount = 161107;
        public int momBirthdayItemGive = (int)GiftId.PinkCake;
        public int momBirthdayItemGiveStackSize = 1;

        public MailConfig() { }

        public static MailConfig InitConfigs()
        {
            if (ConfigManager.Instance.IsConfigExist("MailConfig.json"))
            {
                return ConfigManager.Instance.ReadConfig<MailConfig>("MailConfig.json");
            }
            else
            {
                MailConfig Config = new MailConfig();
                ConfigManager.Instance.WriteConfig("MailConfig.json", Config);
                return Config;
            }
        }
    }
}
