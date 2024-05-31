using StarDew_Mod_1.FrameWork.Core;
using StardewValley;
using static StarDew_Mod_1.FrameWork.Gifts.GiftIDS;

namespace StarDew_Mod_1.FrameWork.Gifts
{
    public class GiftManager : Singleton<GiftManager>
    {

        public Item BirthdayGiftToReceive;

        public Dictionary<string, List<GiftInfo>> NpcBirthdayGifts; // 所有NPC要给玩家的生日礼物数据
        public Dictionary<string, List<GiftInfo>> SpouseBirthdayGifts; // 配偶生日礼物数据

        public Dictionary<string, Item> RegisteredGifts; // 已注册的礼物

        public List<GiftInfo> DefaultBirthdayGifts; // 默认礼物数据

        public event EventHandler<string> OnBirthdayGiftRegistered;
        public event EventHandler PostAllBirthdayGiftsRegistered;


        public GiftManager()
        {
            NpcBirthdayGifts = new Dictionary<string, List<GiftInfo>>();
            SpouseBirthdayGifts = new Dictionary<string, List<GiftInfo>>();
            DefaultBirthdayGifts = new List<GiftInfo>();
            RegisteredGifts = new Dictionary<string, Item>();

            InitGiftIds();

        }

        private void InitGiftIds()
        {
            foreach (var id in GetAllId())
            {
                Item i = new StardewValley.Object(((int)id).ToString(), 1);
                string uniqueID = "StardewValley.Object." + Enum.GetName(typeof(GiftId), (int)id);
                HappyBirthdayMod.Instance.Monitor.Log($"Added gift with id: {uniqueID}",StardewModdingAPI.LogLevel.Trace);

                if (RegisteredGifts.ContainsKey(uniqueID)) continue;
                RegisteredGifts.Add(uniqueID, i);

                if (OnBirthdayGiftRegistered != null)
                {
                    OnBirthdayGiftRegistered.Invoke(this, uniqueID);
                }
            }

            List<string> registeredGiftKeys = RegisteredGifts.Keys.ToList();
            registeredGiftKeys.Sort();
            HappyBirthdayMod.Instance.Helper.Data.WriteJsonFile<List<string>>(Path.Combine("ModAssets", "Gifts", "RegisteredGifts" + ".json"), RegisteredGifts.Keys.ToList());

        }

        /// <summary>
        /// 枚举序列化
        /// </summary>
        /// <returns></returns>
        private List<GiftId> GetAllId()
        {
            GiftId[] ids = (GiftId[])Enum.GetValues(typeof(GiftId));
            return ids.ToList();
        }

        internal void Init()
        {
           
        }
    }
}
