using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] private GameObject _enemyHealthBarTemplate;
    private GameObject _enemyHealthBar;
    [SerializeField] private Transform _enemyHealthBarTransform;
    [SerializeField] private bool _isAlwaysVisible;
    [SerializeField] private float _visibleTime;
    private float _visibleTimer;
    private Image _healthBarSlider;
    private Transform _cameraTransform;
    private UnitStatus _unitStatus;

    void Awake()
    {
        _unitStatus = GetComponent<UnitStatus>();
    }

    void OnEnable()
    {
        _cameraTransform = Camera.main.transform;

        _unitStatus.onGetDamageUpdataHealthBar += HandleUpdateHealthBar;

        // find enemy health bar canvas
        Canvas canvas = GameObject.Find("EnemyHealthBarCanvas").GetComponent<Canvas>();
        _enemyHealthBar = Instantiate(_enemyHealthBarTemplate, canvas.transform);
        _healthBarSlider = _enemyHealthBar.transform.GetChild(0).GetComponent<Image>();
        _enemyHealthBar.gameObject.SetActive(_isAlwaysVisible);
    }

    void OnDisable()
    {
        _unitStatus.onGetDamageUpdataHealthBar -= HandleUpdateHealthBar;
    }

    /// <summary>
    /// handle update enemey health bar
    /// </summary>
    /// <param name="currentHealth"></param>
    /// <param name="maxHealth"></param>
    private void HandleUpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
        {
            Destroy(_enemyHealthBar.gameObject);
        }

        _enemyHealthBar.gameObject.SetActive(true);
        _visibleTimer = _visibleTime;

        float sliderPercent = (float)currentHealth / maxHealth;
        _healthBarSlider.fillAmount = sliderPercent;
    }

    /// <summary>
    /// update ui visible
    /// </summary>
    void LateUpdate()
    {
        if (_enemyHealthBar != null)
        {
            _enemyHealthBar.transform.position = _enemyHealthBarTransform.position;
            _enemyHealthBar.transform.forward = -_cameraTransform.forward;

            if (_visibleTimer <= 0 && !_isAlwaysVisible)
            {
                _enemyHealthBar.gameObject.SetActive(false);
            }
            else
            {
                _visibleTimer -= Time.deltaTime;
            }
        }
    }
}
