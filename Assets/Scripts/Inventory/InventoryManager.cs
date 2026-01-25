using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>, IGetTooltipCanvas
{
    public class DragItemInfo
    {
        public Slot originalHolder;
        public RectTransform originalParent;
    }

    [Header("Inventory Data")]
    [SerializeField] private InventoryData_SO _bagInventoryDataTemplate;
    public InventoryData_SO RuntimeBagInventoryData { get; set; }
    [SerializeField] private InventoryData_SO _actionBarInventoryDataTemplate;
    public InventoryData_SO RuntimeActionBarInventoryData { get; set; }
    [SerializeField] private InventoryData_SO _equipmentInventoryDataTemplate;
    public InventoryData_SO RuntimeEquipmentInventoryData { get; set; }

    [Header("Container")]
    [SerializeField] private SlotContainer _bagInventoryContainer;
    public SlotContainer BagInventoryContainer => _bagInventoryContainer;
    [SerializeField] private SlotContainer _actionBarInventoryContainer;
    public SlotContainer ActionBarInventoryContainer => _actionBarInventoryContainer;
    [SerializeField] private SlotContainer _equipmentInventoryContainer;
    public SlotContainer EquipmentInventoryContainer => _equipmentInventoryContainer;

    [Header("Drag Canvas")]
    [SerializeField] private Canvas _dragCanvas;
    public Canvas DragCanvas => _dragCanvas;
    public DragItemInfo CurrentDragItemInfo { get; set; }

    [Header("UI Panel")]
    [SerializeField] private GameObject _bagPanel;
    [SerializeField] private GameObject _statusPanel;
    bool _isOpen = false;

    [Header("Status Text")]
    [SerializeField] private Text _healthText;
    [SerializeField] private Text _attackText;

    [Header("Tooltip")]
    [SerializeField] private ItemTooltip _tooltip;
    public ItemTooltip Tooltip => _tooltip;

    protected override void Awake()
    {
        base.Awake();
        if (_bagInventoryDataTemplate != null)
        {
            RuntimeBagInventoryData = Instantiate(_bagInventoryDataTemplate);
        }
        if (_actionBarInventoryDataTemplate != null)
        {
            RuntimeActionBarInventoryData = Instantiate(_actionBarInventoryDataTemplate);
        }
        if (_equipmentInventoryDataTemplate != null)
        {
            RuntimeEquipmentInventoryData = Instantiate(_equipmentInventoryDataTemplate);
        }
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        LoadAllInventoryData();
        BagInventoryContainer.RefreshSlots();
        ActionBarInventoryContainer.RefreshSlots();
        EquipmentInventoryContainer.RefreshSlots();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            _isOpen = !_isOpen;
            _bagPanel.SetActive(_isOpen);
            _statusPanel.SetActive(_isOpen);

            // pause
            Time.timeScale = _isOpen ? 0 : 1;
        }

        UpdateStatusText(GameManager.Instance.PlayerStatus.MaxHealth, (int)GameManager.Instance.PlayerStatus.RuntimeAttackData.minAttackPoint, (int)GameManager.Instance.PlayerStatus.RuntimeAttackData.maxAttackPoint);
    }

    /// <summary>
    /// save inventory data
    /// </summary>
    public void SaveAllInventoryData()
    {
        SaveManager.Instance.Save(RuntimeBagInventoryData.name, RuntimeBagInventoryData);
        SaveManager.Instance.Save(RuntimeActionBarInventoryData.name, RuntimeActionBarInventoryData);
        SaveManager.Instance.Save(RuntimeEquipmentInventoryData.name, RuntimeEquipmentInventoryData);
    }

    /// <summary>
    /// load inventory data
    /// </summary>
    public void LoadAllInventoryData()
    {
        SaveManager.Instance.Load(RuntimeBagInventoryData.name, RuntimeBagInventoryData);
        SaveManager.Instance.Load(RuntimeActionBarInventoryData.name, RuntimeActionBarInventoryData);
        SaveManager.Instance.Load(RuntimeEquipmentInventoryData.name, RuntimeEquipmentInventoryData);
    }

    /// <summary>
    /// update status text
    /// </summary>
    public void UpdateStatusText(int health, int minattack, int maxattack)
    {
        _healthText.text = health.ToString();
        _attackText.text = minattack + " - " + maxattack;
    }

    /// <summary>
    /// check if mouse is in the inventory ui area
    /// </summary>
    public bool CheckMouseInInventoryUIArea(SlotContainer slotContainer, Vector3 mousePosition)
    {
        // check every slot in this ui
        for (int i = 0; i < slotContainer.SlotArray.Length; i++)
        {
            // get every slot area
            RectTransform rectTransform = (RectTransform)slotContainer.SlotArray[i].transform;

            // check if this mouse position in this slot area
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition))
            {
                return true;
            }
        }

        return false;
    }

    public ItemTooltip GetItemTooltip()
    {

        return Tooltip;
    }
}
