using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DemiInventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private InventoryState _state;
        [SerializeField] private InventoryBackpack _backpack;
        [SerializeField] private InventoryGrid _grid;
        [SerializeField] private Image _draggableCacheImage;
        [SerializeField] private Transform _extractPoint;

        private InventoryObjectPool _inventoryObjects;
        private InventoryItem _currentDragItem;
        private bool _firstEnterState = true;
        private GridSell _currentSell;
        
        public InventoryState State => _state;
        public Transform ExtractPoint => _extractPoint;

        public UnityEvent<string, string> OnItemInventoryAdded; 
        public UnityEvent<string, string> OnItemInventoryRemoved; 

        private void Start()
        {
            GlobalManager.Instance.SetDependence(this);
            _inventoryObjects = GlobalManager.Instance.InventoryObjectPool;
        }

        private void OnEnable()
        {
            _backpack.StateChanged += SetState;
        }

        private void OnDisable()
        {
            _backpack.StateChanged -= SetState;
        }

        public void FixedExecution()
        {
            switch (_state)
            {
                case InventoryState.IsOpen:
                {
                    OpenState();
                }
                    break;
                case InventoryState.IsClose:
                {
                    CloseState();
                }
                    break;
                case InventoryState.IsFilled:
                {
                    FilledState();
                }
                    break;
                case InventoryState.IsDragItem:
                {
                    DragItemState();
                }
                    break;
                case InventoryState.IsPutItem:
                {
                    PutItemState();
                }
                    break;
                case InventoryState.IsExtractItem:
                {
                    ExtractItemState();
                }
                    break;
            }
        }

        //==== States ====
        private void OpenState()
        {
            if (_firstEnterState)
            {
                _backpack.FadeImages(true);
                _grid.FadeImages(false);
                _firstEnterState = false;
            }
        }

        private void CloseState()
        {
            if (_firstEnterState)
            {
                _backpack.FadeImages(false);
                _grid.FadeImages(true);
                _firstEnterState = false;

                //If the cursor is above the cell with the item when released, then I proceed to extract it.
                _currentSell = _grid.GetBusyCellToExtractItem();

                if (!ReferenceEquals(_grid.GetBusyCellToExtractItem(), null))
                {
                    _currentSell = _grid.GetBusyCellToExtractItem();

                    SetState(InventoryState.IsExtractItem);
                }
                /*else
                {
                    SetState(InventoryState.IsFilled);
                }*/
            }
        }

        private void DragItemState()
        {
            //If the cursor has moved closer to the backpack, then we put the object in it, if it is not occupied, or return it to its place.
            if (IsMouseOver(_backpack.Rect))
            {
                //Debug.Log(_grid.GetFreeSell(_currentDragItem));
                if (!ReferenceEquals(_grid.GetFreeSell(_currentDragItem), null))
                {
                    _currentSell = _grid.GetFreeSell(_currentDragItem);
                    SetState(InventoryState.IsPutItem);
                }
                else
                {
                    SetState(InventoryState.IsFilled);
                }
            }
        }

        private void PutItemState()
        {
            if (_firstEnterState)
            {
                _currentDragItem?.PutInInventory(_backpack);
                _currentSell?.PutInInventory(_currentDragItem);
                _backpack.SetIcon(_currentSell.Type, _currentDragItem.GetItemIcon());

                OnItemInventoryAdded?.Invoke(_currentDragItem.GetItemID().ToString(), "ItemAdd");

                _firstEnterState = false;
            }
        }

        private void ExtractItemState()
        {
            if (_firstEnterState)
            {
                _currentDragItem = _currentSell.InventoryItem;
                _currentDragItem?.ExtractOfInventory(_backpack);
                _currentSell?.ExtractOfInventory(_currentDragItem);
                RedrawIconInBackpack(_currentSell.Type);
                
                OnItemInventoryRemoved?.Invoke(_currentDragItem.GetItemID().ToString(), "ItemRemove");
                
                _firstEnterState = false;
            }
        }

        private void FilledState()
        {
            if (_firstEnterState)
            {
                _backpack.FilledImage();
                
                _currentDragItem.ReturnOrigin();
                _firstEnterState = false;
            }
        }

        //==== Set & Get ====
        private void SetState(InventoryState targetState)
        {
            if (_state == targetState) return;

            _state = targetState;
            _firstEnterState = true;
        }

        private void SetDragItemState(InventoryItem item)
        {
            if (_state == InventoryState.IsDragItem) return;
            
            _state = InventoryState.IsDragItem;
            _firstEnterState = true;
            _currentDragItem = item;
        }

        public Transform GetDraggableCacheTransform()
        {
            return _draggableCacheImage.transform;
        }
        
        public Image GetDraggableCacheImage()
        {
            return _draggableCacheImage;
        }
        
        public void AddSubscription<T>(T obj)
        {
            if (obj is InventoryItem item)
            {
                item.OnDraggable += SetDragItemState;
                item.NonDraggable += SetState;
            }
        }

        public void RemoveSubscription<T>(T obj)
        {
            if (obj is InventoryItem item)
            {
                item.OnDraggable -= SetDragItemState;
                item.NonDraggable -= SetState;
            }
        }
        
        //==================
        private bool IsMouseOver(RectTransform rect)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rect, Input.mousePosition, null, out Vector2 localMousePos
            );

            return rect.rect.Contains(localMousePos);
        }

        private void RedrawIconInBackpack(ItemType type)
        {
            _backpack.SetIcon(type, _grid.GetIconFromItemType(type));
        }
        
    }
}