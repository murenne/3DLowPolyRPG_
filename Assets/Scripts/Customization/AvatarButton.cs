using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class TabPair
{
    public string name;
    public AvatarPartType partType;
    public Toggle toggle;
    public GameObject contentPage;
}

public class AvatarButton : MonoBehaviour
{
    public GameObject tabsPanel;
    public GameObject contentsPanel;
    public GameObject okButton;
    public string nextSceneName = "Wild";
    public List<TabPair> tabPairList = new List<TabPair>();

    void Awake()
    {
        InitializeTabPairs();

        SetupListeners();
    }

    void Start()
    {
        if (tabPairList.Count > 0)
        {
            tabPairList[0].toggle.isOn = true;
            tabPairList[0].toggle.Select();

            SwitchToPage(tabPairList[0].contentPage);
        }
    }

    /// <summary>
    /// initialize tab and content page
    /// </summary>
    private void InitializeTabPairs()
    {
        if (tabsPanel == null || contentsPanel == null)
        {
            return;
        }

        int tabCount = tabsPanel.transform.childCount;
        int pageCount = contentsPanel.transform.childCount;

        if (tabCount != pageCount)
        {
            Debug.LogWarning($"warning: number of Toggle ({tabCount})is diffent from number of Page ({pageCount})");
        }

        int pairCount = Mathf.Min(tabCount, pageCount);

        // link tab to content page
        for (int i = 0; i < pairCount; i++)
        {
            Toggle currentToggle = tabsPanel.transform.GetChild(i).GetComponent<Toggle>();
            GameObject currentPage = contentsPanel.transform.GetChild(i).gameObject;
            string[] currentToggleName = currentToggle.name.Split('_');

            if (currentToggle == null || currentPage == null)
            {
                return;
            }

            foreach (AvatarPartType partType in System.Enum.GetValues(typeof(AvatarPartType)))
            {
                if (partType.ToString() == currentToggleName[0])
                {
                    TabPair pair = new TabPair();
                    pair.name = currentToggle.name;
                    pair.partType = partType;
                    pair.toggle = currentToggle;
                    pair.contentPage = currentPage;

                    tabPairList.Add(pair);
                }
            }
        }
    }

    /// <summary>
    /// set up tab and button listener
    /// </summary>
    private void SetupListeners()
    {
        foreach (TabPair pair in tabPairList)
        {
            pair.toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    SwitchToPage(pair.contentPage);
                }
            });

            foreach (Transform eqTransform in pair.contentPage.transform)
            {
                Toggle equipmentToggle = eqTransform.GetComponent<Toggle>();
                if (equipmentToggle == null)
                {
                    continue;
                }

                int equipmentIndex = eqTransform.GetSiblingIndex();

                equipmentToggle.onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        Debug.Log($"you click No.{equipmentIndex} of {pair.partType}");
                        CustomizationManager.Instance.ChangePart(pair.partType, equipmentIndex);

                        // play animation
                        CustomizationManager.Instance.PlayEquipAnimation(pair.partType);
                    }
                });
            }
        }

        if (okButton != null)
        {
            Button btn = okButton.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(OnOkButtonClicked);
            }
        }
    }

    /// <summary>
    /// click tab and switch to page
    /// </summary>
    private void SwitchToPage(GameObject pageToShow)
    {
        foreach (TabPair pair in tabPairList)
        {
            bool isActive = (pair.contentPage == pageToShow);
            if (pair.contentPage.activeSelf != isActive)
            {
                pair.contentPage.SetActive(isActive);
            }
        }
    }

    /// <summary>
    /// click ok button event
    /// </summary>
    private void OnOkButtonClicked()
    {
        // save parts to static
        CustomizationManager.Instance.SaveCurrentPartsToStorage();

        // change scene
        SceneManager.LoadScene(nextSceneName);
    }
}
