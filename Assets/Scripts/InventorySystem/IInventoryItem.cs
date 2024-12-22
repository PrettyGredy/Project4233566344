using UnityEngine;

namespace DemiInventory
{
    public interface IInventoryItem
    {
        public GameObject GetItemPrefab();
        public Sprite GetItemIcon();
        public ItemType GetItemType();
    }
}