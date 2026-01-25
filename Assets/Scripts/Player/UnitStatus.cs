using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    [Header("data")]
    [SerializeField] private CharacterData_SO _defalutStatusData;
    public CharacterData_SO RuntimeStatusData { get; private set; }
    [SerializeField] private AttackDataBase_SO _defalutAttackData;
    public AttackDataBase_SO RuntimeAttackData { get; private set; }
    private RuntimeAnimatorController _defalutAttackAnimator;
    private PlayerController _playerController;

    // event
    public event Action<int, int> onGetDamageUpdataHealthBar;

    // current status
    public int MaxHealth => RuntimeStatusData != null ? RuntimeStatusData.maxHealth : 0;
    public int CurrentHealth { get { return RuntimeStatusData != null ? RuntimeStatusData.currentHealth : 0; } private set { RuntimeStatusData.currentHealth = value; } }
    public int Basedefence => RuntimeStatusData != null ? RuntimeStatusData.baseDefence : 0;
    public int CurrentDefence => RuntimeStatusData != null ? RuntimeStatusData.currentDefence : 0;

    public HurtState HurtState { get; set; }
    public GameObject WeaponObjec { get; private set; }

    public bool IsCritical { get; set; }
    public bool IsTalking { get; set; }
    public bool IsTransing { get; set; }

    void Awake()
    {
        TryGetComponent<PlayerController>(out _playerController);

        // copy template data
        if (_defalutStatusData != null)
        {
            RuntimeStatusData = Instantiate(_defalutStatusData);
        }
        if (_defalutAttackData != null)
        {
            RuntimeAttackData = Instantiate(_defalutAttackData);
        }

        _defalutAttackAnimator = GetComponent<Animator>().runtimeAnimatorController;

        HurtState = HurtState.Nothing;
    }

    /// <summary>
    /// calculate enemy will get damage
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    public void GetUnitDamage(UnitStatus attacker, UnitStatus defender)
    {
        Debug.Log($"defend is {defender.gameObject.name}");
        var attackPoint = attacker.CalculateCurrentAttackPoint();
        int damage = Mathf.Max(attackPoint - defender.CurrentDefence, 1);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (defender.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(CurrentHealth);
        }

        defender.HurtState = attacker.IsCritical ? HurtState.CriticalHurt : HurtState.Nothing;

        //UI,LEVEL UP
        onGetDamageUpdataHealthBar?.Invoke(CurrentHealth, MaxHealth);
        if (CurrentHealth <= 0)
        {
            attacker.RuntimeStatusData.UpdateExp(RuntimeStatusData.killPoint);
        }
    }

    /// <summary>
    /// calculate rock will get damage
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="defender"></param>
    public void GetRockDamage(int damage, UnitStatus defender)
    {
        int currentDamage = Mathf.Max(damage - defender.CurrentDefence, 1);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);
        onGetDamageUpdataHealthBar?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            GameManager.Instance.PlayerStatus.RuntimeStatusData.UpdateExp(RuntimeStatusData.killPoint);
        }
    }

    /// <summary>
    /// calculate player's attack damage
    /// </summary>
    /// <returns></returns>
    private int CalculateCurrentAttackPoint()
    {
        float coreAttackPoint = UnityEngine.Random.Range(RuntimeAttackData.maxAttackPoint, RuntimeAttackData.minAttackPoint);
        if (IsCritical)
        {
            coreAttackPoint *= RuntimeAttackData.criticalMultiplier;
        }
        return (int)coreAttackPoint;
    }

    /// <summary>
    /// update health
    /// </summary>
    /// <param name="amount"></param>
    public void ApplyHealth(int amount)
    {
        CurrentHealth = CurrentHealth + amount <= MaxHealth ? CurrentHealth += amount : MaxHealth;
    }

    /// <summary>
    /// change weapon
    /// </summary>
    /// <param name="weapon"></param>
    public void ChangeWeapon(ItemData_SO weapon)
    {
        UnEquipWeapon();
        EquipWeapon(weapon);
    }

    /// <summary>
    /// has any weapon
    /// </summary>
    /// <param name="weapon"></param>
    public void EquipWeapon(ItemData_SO weaponData)
    {
        if (weaponData.weaponPrefab != null && _playerController)
        {
            WeaponObjec = Instantiate(weaponData.weaponPrefab, _playerController.WeaponSlot);
            var weaponAttack = WeaponObjec.transform.GetComponent<Attack>();
            weaponAttack.OwnerUnitStatus = this;
        }

        // switch to this weapon's attack data and animation
        RuntimeAttackData.ApplyWeaponData(weaponData.weaponAttackData);
        GetComponent<Animator>().runtimeAnimatorController = weaponData.weaponAnimator;
    }

    /// <summary>
    /// has not any weapon 
    /// </summary>
    public void UnEquipWeapon()
    {
        if (_playerController && _playerController.WeaponSlot.transform.childCount != 0)
        {
            for (int i = 0; i < _playerController.WeaponSlot.transform.childCount; i++)
            {
                Destroy(_playerController.WeaponSlot.transform.GetChild(i).gameObject);
            }
        }

        // switch to base attack data and animation
        RuntimeAttackData.ApplyWeaponData(_defalutAttackData);
        GetComponent<Animator>().runtimeAnimatorController = _defalutAttackAnimator;
    }

    /// <summary>
    /// get loaded data
    /// </summary>
    public void ApplyLoadedData()
    {
        onGetDamageUpdataHealthBar?.Invoke(CurrentHealth, MaxHealth);
    }
}





