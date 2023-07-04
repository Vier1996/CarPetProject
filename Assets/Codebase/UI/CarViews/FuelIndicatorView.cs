using System;
using Codebase.Gameplay.CarDetails;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Codebase.UI
{
    public class FuelIndicatorView : MonoBehaviour
    {
        [SerializeField] private Slider _fuelSlider;

        private IDisposable _updateDisposable;
        private Car _targetCar;

        private float _defaultFuelValue;
        
        public void Init(Car targetCar)
        {
            _targetCar = targetCar;
            _defaultFuelValue = _targetCar.FuelCount;
            
            _updateDisposable = Observable.EveryUpdate().Subscribe(OnTick);
        }

        private void OnTick(long tick) => _fuelSlider.value = _targetCar.FuelCount / _defaultFuelValue;

        public void OnDestroy()
        {
            _updateDisposable?.Dispose();
        }
    }
}