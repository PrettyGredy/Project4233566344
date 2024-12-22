using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DemiInventory
{
    public class InventoryGrid : MonoBehaviour
    {
        [SerializeField] private GameObject _divHolder;
        [SerializeField] private List<GridSell> _sells = new List<GridSell>();

        private List<FadeImageLinker> _fadeImages = new List<FadeImageLinker>();
        
        private void Start()
        {
            GetAllSell();
            GetAllImages();
            FadeImages(true);
            _divHolder.SetActive(false);
        }
        
        public void FadeImages(bool enable)
        {
            if (enable)
            {
                DOVirtual.DelayedCall(0.5f, () => _divHolder.SetActive(false));
            }
            else
            {
                _divHolder.SetActive(true);
            }
            
            foreach (var image in _fadeImages)
            {
                if (enable)
                {
                    image.Image.DOFade(0, 0.5f);
                }
                else
                {
                    image.Image.DOFade(image.Alpha, 0.5f);
                }
            }
        }

        public GridSell GetFreeSell(InventoryItem item)
        {
            List<GridSell> tempSells = new List<GridSell>();
            
            foreach (var sell in _sells)
            {
                if (sell.Type == item.GetItemType() && sell.SellState == GridSellState.Free)
                {
                    tempSells.Add(sell);
                }
            }
            
            if (tempSells.Count == 0)
            {
                return null;
            }
            
            return tempSells[0];
        }

        public Sprite GetIconFromItemType(ItemType type)
        {
            List<GridSell> tempSells = new List<GridSell>();
            

            for (int i = 0; i < _sells.Count; i++)
            {
                if (_sells[i].Type == type && _sells[i].SellState == GridSellState.Busy)
                {
                    tempSells.Add(_sells[i]);
                }
            }
            
            if (tempSells.Count == 0)
            {
                return null;
            }

            return tempSells[^1].Icon.sprite;
        }

        public GridSell GetSellByID(int id)
        {
            foreach (var sell in _sells)
            {
                if (sell.SellID == id)
                {
                    return sell;
                }
            }

            return null;
        }
        
        private void GetAllSell()
        {
            var sells = transform.GetComponentsInChildren<GridSell>();
            foreach (var sell in sells)
            {
                _sells.Add(sell);
            }
        }

        private void GetAllImages()
        {
            var images = transform.GetComponentsInChildren<Image>();
            foreach (var image in images)
            {
                FadeImageLinker fil = new FadeImageLinker();
                fil.Image = image;
                fil.Alpha = image.color.a;
                _fadeImages.Add(fil);
            }
        }

        public GridSell GetBusyCellToExtractItem()
        {
            foreach (var cell in _sells)
            {
                if (IsMouseOver(cell.Rect))
                {
                    if (cell.SellState == GridSellState.Busy)
                    {
                        return cell;
                    }
                }
            }
            return null;
        }

        private bool IsMouseOver(RectTransform rect)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rect, Input.mousePosition, null, out Vector2 localMousePos
            );

            return rect.rect.Contains(localMousePos);
        }
    }
}