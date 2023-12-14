using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Scripts.UI.HUD
{
    public class Condition : MonoBehaviour
    {
        #region Field
        private float _currentValue;
        private float _maxValue;
        private float _startValue;
        private float _regenerate;
        private float _decayRate;
        private float _percentageValue;
        #endregion
    
        #region Properties
    
        public float PercentageValue => Mathf.Clamp(CurrentValue, 0.0f, MaxValue) / MaxValue;

        public float CurrentValue
        {
            get => _currentValue;
            set  => _currentValue = value;
        }
    
        public float MaxValue
        {
            get => _maxValue;
            set => _maxValue = value;
        }
    
        public float StartValue
        {
            get => _startValue;
            set
            {
                _startValue = value;
                _currentValue = _startValue;
            }
        }
    
        public float Regenerate
        {
            get => _regenerate;
            set => _regenerate = value;
        }
    
        public float DecayRate
        {
            get => _decayRate;
            set => _decayRate = value;
        }
        #endregion

        #region Initialized

        public void Initialized(float startValue, float maxValue, float regenerate, float decayRate)
        {
            StartValue = startValue;
            MaxValue = maxValue;
            Regenerate = regenerate;
            DecayRate = decayRate;
        }
        #endregion
    }
}