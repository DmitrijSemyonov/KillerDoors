using KillerDoors.Services.ShopSpace;
using UnityEngine;
using UnityEngine.UI;

namespace KillerDoors.UI.Windows.Shop.Views
{
    public class BuyShopItemButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private ShopItem _item;
        private IShopService _shop;

        public void Construct(IShopService shopService)
        {
            _shop = shopService;
            _button.onClick.AddListener(BuyItem);
        }

        public void BuyItem() => 
            _shop.BuyItem(_item);
    }
}