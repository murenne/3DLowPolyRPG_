using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private Text _itemNameText;
    [SerializeField] private Text _itemInfomationText;
    RectTransform _rectTransform;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetupTooltip(ItemData_SO item)
    {
        _itemNameText.text = item.itemName;
        _itemInfomationText.text = item.description;
    }

    void OnEnable()
    {
        UpdatePosition();
    }

    void Update()
    {
        UpdatePosition();
    }

    /// <summary>
    /// auto to change tooltip position
    /// </summary>
    public void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;

        Vector3[] corners = new Vector3[4];
        _rectTransform.GetWorldCorners(corners);
        float width = corners[3].x - corners[0].x;
        float height = corners[1].y - corners[0].y;

        if (mousePos.y < height * 1.5f)
        {
            _rectTransform.position = mousePos + Vector3.up * height * 0.6f;
        }

        else if (Screen.width - mousePos.x > width * 1.5f)
        {
            _rectTransform.position = mousePos + Vector3.right * width * 0.6f;
        }
        else
        {
            _rectTransform.position = mousePos + Vector3.left * width * 0.6f;
        }
    }
}
