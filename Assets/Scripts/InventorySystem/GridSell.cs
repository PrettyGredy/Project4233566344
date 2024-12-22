using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DemiInventory
{
    public class GridSell : InventorySell, IInventoryStorable
    {
        [SerializeField] private int _sellID;
        private GameObject _obj;
        private InventoryItem _inventoryItem;
        private int _itemId;
        public InventoryItem InventoryItem => _inventoryItem;
        public int SellID => _sellID;
        public int ItemId => _itemId;


        private void Start()
        {
            rect = GetComponent<RectTransform>();
        }

        public void PutInInventory<T>(T par)
        {
            if (par is InventoryItem item)
            {
                _inventoryItem = item;
                _obj = item.gameObject;
                icon.enabled = true;
                icon.sprite = item.GetItemIcon();
                sellState = GridSellState.Busy;
                _itemId = item.GetItemID();
            }
        }

        public void ExtractOfInventory<T>(T item)
        {
            _inventoryItem = null;
            _obj = null;
            icon.enabled = false;
            icon.sprite = null;
            sellState = GridSellState.Free;
            _itemId = 0;
        }
    }
}