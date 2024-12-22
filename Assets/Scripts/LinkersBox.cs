using System;
using UnityEngine;
using UnityEngine.UI;

public class LinkersBox : MonoBehaviour
{
 
}

[Serializable]
public struct FadeImageLinker
{
    public Image Image;
    public float Alpha;
}

public enum ItemType
{
    Weapon,
    Food,
    Props,
    None
}

public enum GridSellState
{
    Free,
    Busy,
    None
}

public enum InventoryState
{
    IsOpen,
    IsClose,
    IsFilled,
    IsDragItem,
    IsPutItem,
    IsExtractItem,
    None
}

[Serializable]
public class InventoryEvent
{
    public string ItemId;
    public string EventType;

    public InventoryEvent(string id, string eventType)
    {
        this.ItemId = id;
        this.EventType = eventType;
    }
}
