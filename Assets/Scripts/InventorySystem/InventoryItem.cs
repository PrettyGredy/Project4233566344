using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DemiInventory
{
    public class InventoryItem : MonoBehaviour, IInventoryStorable, IDragHandler, IBeginDragHandler, IPointerClickHandler, IPointerUpHandler, IInventoryItem
    {
        [SerializeField] private SOInventoryItem _soItem;

        private InventoryManager _inventoryManager;
        private MeshRenderer _mesh;
        private Transform _draggableIconTransform;
        private Image _draggableIconImage;
        private Vector3 _originalPos;
        private bool _inInventory = false;

        public Action<InventoryItem> OnDraggable;
        public Action<InventoryState> NonDraggable;

        private void Start()
        {
            _inventoryManager = GlobalManager.Instance.InventoryManager;
            _draggableIconTransform = _inventoryManager.GetDraggableCacheTransform();
            _draggableIconImage = _inventoryManager.GetDraggableCacheImage();
            _mesh = GetComponent<MeshRenderer>();
            PutInPool();
            _inventoryManager.AddSubscription(this);
        }

        private void OnDestroy()
        {
            _inventoryManager.RemoveSubscription(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _mesh.enabled = false;
            _draggableIconTransform.gameObject.SetActive(true);
            _originalPos = Input.mousePosition;
            OnDraggable?.Invoke(this);
            _draggableIconImage.sprite = _soItem.Icon;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            _draggableIconTransform.position = Input.mousePosition;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_inInventory)
            {
                return;
            }
            
            NonDraggable?.Invoke(InventoryState.None);
            ReturnOrigin();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        //==== Set & Get ====
        public int GetItemID()
        {
            return _soItem.ID;
        }
        
        public GameObject GetItemPrefab()
        {
            return _soItem.Prefab;
        }

        public Sprite GetItemIcon()
        {
            return _soItem.Icon;
        }

        public ItemType GetItemType()
        {
            return _soItem.Type;
        }
        //==================
        
        public void PutInInventory<T>(T par)
        {
            if (par is InventoryBackpack backpack)
            {
                _inInventory = true;
                
                RectTransform cellRect = backpack.GetRectSell(_soItem.Type);
                var draggableIconRect = _draggableIconTransform as RectTransform;
                Color tempColor = _draggableIconImage.color;
                Vector2 tempSize = draggableIconRect.sizeDelta;
                
                _draggableIconImage.DOFade(0, 1f).OnComplete(() =>
                {
                    _draggableIconImage.color = tempColor;
                    gameObject.SetActive(false);
                });
                
                draggableIconRect.DOSizeDelta(cellRect.sizeDelta, 0.5f).OnComplete(() =>
                {
                    draggableIconRect.sizeDelta = tempSize;
                });
                
                _draggableIconTransform.DOMove(cellRect.position, 0.5f).OnComplete(() =>
                {
                    _draggableIconTransform.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                });
            }
        }

        public void ExtractOfInventory<T>(T par)
        {
            if (par is InventoryBackpack backpack)
            {
                _inInventory = false;
                gameObject.SetActive(true);
                _draggableIconTransform.gameObject.SetActive(true);
                _draggableIconImage.sprite = _soItem.Icon;
                _draggableIconTransform.position = backpack.transform.position;
                
                var extractPointPos = GlobalManager.Instance.InventoryManager.ExtractPoint as RectTransform;
                transform.position = GetPositionUnderIcon(extractPointPos.position);
                
                _draggableIconImage.DOFade(1, 0.5f);
                
                _draggableIconTransform.DOMove(extractPointPos.position, 1f).OnComplete(() =>
                {
                    _mesh.enabled = true;
                    _draggableIconTransform.gameObject.SetActive(false);
                    gameObject.SetActive(true);
                });
            }
        }

        public void ReturnOrigin()
        {
            _draggableIconTransform.DOMove(_originalPos, 1f).OnComplete(() =>
            {
                _mesh.enabled = true;
                _draggableIconTransform.gameObject.SetActive(false);
            });
        }
        
        private void PutInPool()
        {
            GlobalManager.Instance.InventoryObjectPool.SetItem(this);
            transform.SetParent(GlobalManager.Instance.InventoryObjectPool.transform);
        }

        private Vector3 GetPositionUnderIcon(Vector3 uiElement)
        {
            Vector3 position = default;
            var uiCamera = GlobalManager.Instance.MainCamera;

            RaycastHit hit = new RaycastHit();
            Ray ray = uiCamera.ScreenPointToRay(uiElement);
            if (Physics.Raycast(ray,out hit, 1000))
            {
                position = new Vector3(hit.point.x, hit.point.y + 0.2f, hit.point.z);
            }

            return position;
        }
    }
}