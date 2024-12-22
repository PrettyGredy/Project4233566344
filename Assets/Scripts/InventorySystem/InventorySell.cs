using UnityEngine;
using UnityEngine.UI;

namespace DemiInventory
{
    public abstract class InventorySell : MonoBehaviour
    {
        [SerializeField] protected Image icon;
        [SerializeField] protected ItemType type;
        [SerializeField] protected GridSellState sellState;
        
        protected RectTransform rect;
        
        public Image Icon => icon;

        public ItemType Type => type;
        public GridSellState SellState => sellState;
        public RectTransform Rect => rect;
        
        private void Start()
        {
            rect = GetComponent<RectTransform>();
        }
        
        
    }
}