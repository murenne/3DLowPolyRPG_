using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    Text _levelText;
    Image _healthSlider;
    Image _expSlider;

    void Awake()
    {
        _healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        _expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        _levelText = transform.GetChild(2).GetComponent<Text>();
    }

    void Update()
    {
        _levelText.text = "LEVEL  " + GameManager.Instance.PlayerStatus.RuntimeStatusData.currentLevel.ToString("00");
        UpdateHealth();
        UpdateExp();
    }

    void UpdateHealth()
    {
        float sliderPercent = (float)GameManager.Instance.PlayerStatus.CurrentHealth / GameManager.Instance.PlayerStatus.MaxHealth;
        _healthSlider.fillAmount = sliderPercent;
    }

    void UpdateExp()
    {
        float sliderPercent = (float)GameManager.Instance.PlayerStatus.RuntimeStatusData.currentExp / GameManager.Instance.PlayerStatus.RuntimeStatusData.baseExp;
        _expSlider.fillAmount = sliderPercent;
    }
}
