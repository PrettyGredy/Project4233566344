using System;
using UnityEngine;
using UnityEngine.UIElements;
using VInspector;
using Random = System.Random;

namespace DemiInventory
{
    [CreateAssetMenu(fileName = "item", menuName = "Inventory/New Item", order = 0)]
    public class SOInventoryItem : ScriptableObject
    {
        public int ID;
        public string Name;
        public GameObject Prefab;
        public Sprite Icon;
        public float Weight;
        public ItemType Type;


        [Button]
        private void GenerateId()
        {
            Random random = new Random();
            ID = random.Next(100000, 1000000);
        }
    }
}