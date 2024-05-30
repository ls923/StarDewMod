using StardewValley;

namespace StarDew_Mod_1.FrameWork.Gifts
{
    public class GiftInfo
    {
        public int MinRequiredHearts;
        public int MaxRequiredHearts;
        public string ObjectID;
        public int MinAmount;
        public int MaxAmount;

        public const int MAX_HEARTS = 20;
        public GiftInfo() { }

        public GiftInfo(GiftIDS.GiftId id, int hearts, int min, int max)
        {
            ObjectID = "StardewValley.Object." + Enum.GetName(typeof(GiftIDS.GiftId), (int)id);
            MinRequiredHearts = hearts;
            MaxRequiredHearts = MAX_HEARTS;
            MinAmount = min;
            MaxAmount = max;
        }

        public Item GetOne()
        {
            Item item = GiftManager.Instance.RegisteredGifts[ObjectID].getOne();
            if (MinAmount != MaxAmount)
            {
                item.Stack = Game1.random.Next(MinAmount, MaxAmount);
            }
            else
            {
                item.Stack = MinAmount;
            }
            return item;
        }

        public virtual bool Equals(GiftInfo other)
        {
            return
                other.MinRequiredHearts.Equals(MinRequiredHearts) &&
                other.MaxRequiredHearts.Equals(MaxRequiredHearts) &&
                other.ObjectID.Equals(ObjectID) &&
                other.MinAmount.Equals(MinAmount) &&
                other.MaxAmount.Equals(MaxAmount);
        }

    }
}
