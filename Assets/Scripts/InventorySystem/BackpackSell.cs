using DG.Tweening;
using UnityEngine;

namespace DemiInventory
{
    public class BackpackSell : InventorySell
    {
        public void SetIcon(Sprite icon)
        {
            if (!ReferenceEquals(icon, null))
            {
                base.icon.enabled = true;
                base.icon.sprite = icon;
                base.icon.DOFade(1, 0.5f);
            }
            else
            {
                base.icon.enabled = false;
            }
        }
    }
}