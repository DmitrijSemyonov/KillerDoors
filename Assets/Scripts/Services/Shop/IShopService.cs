namespace KillerDoors.Services.ShopSpace
{
    public interface IShopService : IService
    {
        void BuyItem(ShopItem item);
        void Init();
    }
}