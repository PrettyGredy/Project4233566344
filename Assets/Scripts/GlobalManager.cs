using System.Reflection;
using DemiInventory;
using UnityEngine;
using VInspector;

public class GlobalManager : MonoBehaviour
{
    [SerializeField] private float _gameSpeed;
    [SerializeField] private Transform _mainPlayer;
    [SerializeField] private Canvas _canvasMain;
    [SerializeField] private Camera _mainCamera;


    private InventoryObjectPool _inventoryObjectPool;
    private InventoryManager _inventoryManager;

    public static GlobalManager Instance = null;

    ///Properties
    public Camera MainCamera => _mainCamera;
    public Transform MainPlayer => _mainPlayer;
    public Canvas CanvasMain => _canvasMain;
    public InventoryObjectPool InventoryObjectPool => _inventoryObjectPool;
    public InventoryManager InventoryManager => _inventoryManager;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        _inventoryManager?.FixedExecution();
    }

    private void LateUpdate()
    {
    }

    
    public void SetDependence<T>(T dependency) where T : MonoBehaviour
    {
        //Using reflection to gain access to a private list of fields
        FieldInfo[] fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            if (field.FieldType.IsInstanceOfType(dependency))
            {
                field.SetValue(this, dependency);
            }
        }
    }

    [Button]
    private void ChangeGameSpeed()
    {
        Time.timeScale = _gameSpeed;
    }
}
