using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    #region Fields
    private int _damage;
    private float _damageRate;
    private List<IDamage> _thingsToDamage = new();
    #endregion

    #region Properties

    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }

    public float DamageRate
    {
        get => _damageRate;
        set => _damageRate = value;
    }
    #endregion


    private void Start()
    {
        Damage = 10;
        DamageRate = 0.5f;
        InvokeRepeating("DealDamage", 0, DamageRate);
    }

    private void DealDamage()
    {
        foreach (var d in _thingsToDamage)
        {
            d.TakePhysicalDamage(Damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamage damage))
        {
            _thingsToDamage.Add(damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamage damage))
        {
            _thingsToDamage.Remove(damage);
        }
    }
}
