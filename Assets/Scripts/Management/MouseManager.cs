using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class MouseManager : Singleton<MouseManager>
{
    [Header("Texture")]
    [SerializeField] private Texture2D _arrowTexture;
    [SerializeField] private Texture2D _targetTexture;
    [SerializeField] private Texture2D _attackTexture;
    [SerializeField] private Texture2D _doorwayTexture;
    [SerializeField] private Texture2D _pointTexture;

    RaycastHit hitInfo;
    public event Action<GameObject> onMouseClicked;
    public event Action<GameObject> onEnemyClicked;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        SetCursorTexture();

        if (!IsInteractWithUI())
        {
            ProcessMouseClick();
        }
    }

    /// <summary>
    /// set cursor texture
    /// </summary>
    void SetCursorTexture()
    {

        if (IsInteractWithUI())
        {
            // if you want to use the default arrow texture
            // Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            Cursor.SetCursor(_arrowTexture, new Vector2(0, 0), CursorMode.Auto);
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            // change texture
            switch (hitInfo.collider.gameObject.tag)
            {
                default:
                    Cursor.SetCursor(_arrowTexture, new Vector2(0, 0), CursorMode.Auto);
                    break;

                case "Item":
                    Cursor.SetCursor(_targetTexture, new Vector2(16, 16), CursorMode.Auto);
                    break;

                case "Enemy":
                    Cursor.SetCursor(_attackTexture, new Vector2(16, 16), CursorMode.Auto);
                    break;

                case "Portal":
                    Cursor.SetCursor(_doorwayTexture, new Vector2(16, 16), CursorMode.Auto);
                    break;

                case "NPC":
                    Cursor.SetCursor(_pointTexture, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }

    /// <summary>
    /// process mouse click
    /// </summary>
    void ProcessMouseClick()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            // onMouseClicked
            if (hitInfo.collider.gameObject.CompareTag("Portal"))
            {
                onMouseClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Item"))
            {
                onMouseClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("NPC"))
            {
                onMouseClicked?.Invoke(hitInfo.collider.gameObject);
            }


            // onEnemyClicked
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                onEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
            {
                onEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
        }
    }

    /// <summary>
    /// check if cursor is interacting wiht ui 
    /// </summary>
    /// <returns></returns>
    bool IsInteractWithUI()
    {
        // have eventSystem and cursor is on any type of ui gameObject
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}
