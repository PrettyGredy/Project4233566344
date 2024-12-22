using System.Collections.Generic;
using UnityEngine;

namespace DemiInventory
{
    public class InventoryObjectPool : MonoBehaviour
    {
        private List<InventoryItem> _inventoryItems = new List<InventoryItem>();

        private void Awake()
        {
            GlobalManager.Instance.SetDependence(this);
        }

        //==== Set & Get ====
        public void SetItem(InventoryItem item)
        {
            _inventoryItems.Add(item);
        }

        public InventoryItem SpawnObj(GameObject prefab)
        {
            var temp = Instantiate(prefab, transform).GetComponent<InventoryItem>();
            SetItem(temp);
            return temp;
        }

        public List<InventoryItem> GetItems()
        {
            List<InventoryItem> items = new List<InventoryItem>();

            foreach (var item in _inventoryItems)
            {
                items.Add(item);
            }
            
            return items;
        }
        //===================
    }
}