using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : Singleton<QuestUI>, IGetTooltipCanvas
{
    [Header("Elements")]
    [SerializeField] private GameObject _questPanel;
    public ItemTooltip tooltip;
    private bool _isOpen;

    [Header("Quest Name")]
    [SerializeField] private RectTransform _questNameListTransform;
    [SerializeField] private QuestNameButton _questNameButtonPrefab;

    [Header("Text Content")]
    [SerializeField] private Text _questDescriptionText;

    [Header("Requirement")]
    [SerializeField] private RectTransform _requireItemListTransform;
    [SerializeField] private QuestRequirement _requireItemPrefab;

    [Header("Reward")]
    [SerializeField] private RectTransform _rewardPanelTransform;
    [SerializeField] private Item _rewardItemSlotPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _isOpen = !_isOpen;
            _questPanel.SetActive(_isOpen);
            _questDescriptionText.text = "";
            SetUpQuestList();

            if (!_isOpen)
            {
                tooltip.gameObject.SetActive(false);
            }
        }
    }

    public void SetUpQuestList()
    {
        // initialize
        foreach (Transform item in _questNameListTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in _rewardPanelTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in _requireItemListTransform)
        {
            Destroy(item.gameObject);
        }

        // instantiate every quests in quest list
        foreach (var quest in QuestManager.Instance.questList)
        {
            var newQuest = Instantiate(_questNameButtonPrefab, _questNameListTransform);
            newQuest.SetupQuestNameButton(quest._questData);
            newQuest.QuestContentText = _questDescriptionText; // setup text component and blank first
        }
    }

    /// <summary>
    /// setup require item list
    /// </summary>
    /// <param name="questData"></param>
    public void SetupRequireItemList(QuestData_SO questData)
    {
        // initialize
        foreach (Transform item in _requireItemListTransform)
        {
            Destroy(item.gameObject);
        }

        // instantiate
        foreach (var requireItem in questData.questRequireItemList)
        {
            var Item = Instantiate(_requireItemPrefab, _requireItemListTransform);
            Item.SetupRequirement(requireItem.Name, requireItem.requireAmount, requireItem.currentAmount);
        }
    }

    /// <summary>
    /// setup reward item list
    /// </summary>
    /// <param name="questData"></param>
    public void SetupRewardItemList(QuestData_SO questData)
    {
        // initialize
        foreach (Transform item in QuestUI.Instance._rewardPanelTransform)
        {
            Destroy(item.gameObject);
        }

        // instantiate
        foreach (var rewardItem in questData.questRewardItemList)
        {
            var item = Instantiate(_rewardItemSlotPrefab, _rewardPanelTransform);
            item.SetupItemInfo(rewardItem.itemData, rewardItem.itemAmount);
        }
    }

    public ItemTooltip GetItemTooltip()
    {
        return tooltip;
    }
}
