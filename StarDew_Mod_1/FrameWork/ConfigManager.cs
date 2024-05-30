using StardewModdingAPI;
using System;

namespace StarDew_Mod_1.FrameWork
{

    public class ConfigManager : Singleton<ConfigManager>
    {
        public MailConfig MailCfg;
        public ModConfig ModCfg;

        public ConfigManager()
        {
        }

        public virtual void InitConfig()
        {
            ModCfg = ModConfig.InitConfigs();
            MailCfg = MailConfig.InitConfigs();
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="ConfigName"></param>
        /// <returns></returns>
        public bool IsConfigExist(string ConfigName)
        {
            string configPath = GetConfigPath(true, ConfigName);
            return File.Exists(configPath);
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="fullPath">是否是绝对路径</param>
        /// <param name="configName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string GetConfigPath(bool fullPath, string configName)
        {
            Directory.CreateDirectory(Path.Combine(HappyBirthdayMod.Instance.Helper.DirectoryPath, "Configs"));
            if (fullPath)
            {
                return Path.Combine(HappyBirthdayMod.Instance.Helper.DirectoryPath, "Configs", configName);
            }
            return Path.Combine("Configs", configName);
        }

        public virtual T? ReadConfig<T>(string configName) where T : class
        {
            var config = HappyBirthdayMod.Instance.Helper.Data.ReadJsonFile<T>(path: GetConfigPath(false, configName));
            if (config == null) return default;
            return config;
        }

        public virtual void WriteConfig(string configName, object config)
        {
            HappyBirthdayMod.Instance.Helper.Data.WriteJsonFile(GetConfigPath(false, configName), config);
        }

    }
}
