using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.UI.HUD;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerCondition : MonoBehaviour, IDamage
{
    #region Field

    [SerializeField] private Image healthBar;
    [SerializeField] private Image saturationBar;
    [SerializeField] private Image staminaBar;
    private Condition _health;
    private Condition _saturation;
    private Condition _stamina;
    private float _noSaturationHealthDecay;
    private UnityEvent _onAttackDamage; 

    #endregion

    #region Properties

    public Condition Health
    {
        get => _health;
        set => _health = value;
    }

    public Condition Saturation
    {
        get => _saturation;
        set => _saturation = value;
    }

    public Condition Stamina
    {
        get => _stamina;
        set => _stamina = value;
    }

    public float NoSaturationHealthDecay
    {
        get => _noSaturationHealthDecay;
        set => _noSaturationHealthDecay = value;
    }

    public UnityEvent OnAttackDamage
    {
        get => _onAttackDamage;
        set => _onAttackDamage = value;
    }

    #endregion

    #region Initialized
    private void Start()
    {
        Health = gameObject.AddComponent<Condition>();
        Health.Initialized(100,100,1,5);
        Stamina = gameObject.AddComponent<Condition>();
        Stamina.Initialized(100,100,5,0);
        Saturation = gameObject.AddComponent<Condition>();
        Saturation.Initialized(400,500,0,2);
    }
    #endregion

    #region Player Status Controller
    
    public void Heal(float amount)
    {
        Health.CurrentValue += amount;
    }

    public void Eat(float amount)
    {
        Saturation.CurrentValue += amount;
    }

    public bool UseStamina(float amount)
    {
        if (Stamina.CurrentValue - amount < 0.0f) return false;
        Stamina.CurrentValue -= amount;
        return true;
    }

    private void DiedPlayer()
    {
        Debug.Log("플레이어 사망");
    }
    #endregion

    #region PlayGame

    private void FixedUpdate()
    {
        Saturation.CurrentValue -= Saturation.DecayRate * Time.deltaTime;
        Stamina.CurrentValue += Stamina.Regenerate * Time.deltaTime;
        
        if (Saturation.CurrentValue <= 0.0f)
        {
            Health.CurrentValue -= Health.DecayRate * Time.deltaTime;
        }

        if (Health.CurrentValue <= 0.0f)
        {
            DiedPlayer();
        }
        healthBar.fillAmount = Health.PercentageValue;
        saturationBar.fillAmount = Saturation.PercentageValue;
        staminaBar.fillAmount = Stamina.PercentageValue;
    }

    #endregion

    public void TakePhysicalDamage(int damageAmount)
    {
        Health.CurrentValue -= damageAmount;
        OnAttackDamage?.Invoke();
    }
}
