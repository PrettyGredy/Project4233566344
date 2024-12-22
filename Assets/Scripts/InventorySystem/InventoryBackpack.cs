using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DemiInventory
{
    public class InventoryBackpack : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private GameObject _divHolder;
        [SerializeField] private List<BackpackSell> _sells = new List<BackpackSell>();

        private Image _backpackImage;
        private List<FadeImageLinker> _fadeImages = new List<FadeImageLinker>();
        private RectTransform _rect;
        
        public RectTransform Rect => _rect;

        private void Start()
        {
            _rect = GetComponent<RectTransform>();
            _backpackImage = GetComponent<Image>();

            GetAllSell();
            GetAllImages();
        }
        
        public Action<InventoryState> StateChanged; 

        public void OnPointerUp(PointerEventData eventData)
        {
            StateChanged?.Invoke(InventoryState.IsClose);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            StateChanged?.Invoke(InventoryState.IsOpen);
        }
        
        public void FadeImages(bool enable)
        {
            if (enable)
            {
                _backpackImage.DOFade(0, 0.5f).OnComplete(() => { _backpackImage.enabled = false; });
            }
            else
            {
                DOTween.Kill(_backpackImage);
                _backpackImage.enabled = true;
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
        
        public void FilledImage()
        {
            _backpackImage.DOColor(Color.magenta, 0.3f).OnComplete(() =>
            {
                _backpackImage.DOColor(Color.white, 0.8f);
            });
        }
        
        //==== Set & Get ====
        private void GetAllSell()
        {
            var sells = transform.GetComponentsInChildren<BackpackSell>();
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

        public RectTransform GetRectSell(ItemType type)
        {
            foreach (var sell in _sells)
            {
                if (sell.Type == type)
                {
                    return sell.Rect;
                }
            }

            return null;
        }

        public void SetIcon(ItemType type, Sprite icon)
        {
            foreach (var sell in _sells)
            {
                if (sell.Type == type)
                {
                    sell.SetIcon(icon);
                }
            }
        }
        //===================
        
    }
}

