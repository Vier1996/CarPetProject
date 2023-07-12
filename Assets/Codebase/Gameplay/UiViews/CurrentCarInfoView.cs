using System;
using System.Globalization;
using Codebase.Gameplay.Enums;
using Codebase.Gameplay.Providers;
using TMPro;
using UnityEngine;
using Zenject;

namespace Codebase.Gameplay.UiViews
{
    public class CurrentCarInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _maxSpeedText;
        [SerializeField] private TextMeshProUGUI _accelerationText;
        [SerializeField] private TextMeshProUGUI _suspensionText;
        [SerializeField] private TextMeshProUGUI _brakingText;

        private CarWorkshopProvider _carWorkshopProvider;
        private CarWorkshopData _actualCarWorkshopData;
        
        private CarType _carType = CarType.PROTOTYPE;
        
        [Inject] private void Init(CarWorkshopProvider workshopProvider)
        {
            _carWorkshopProvider = workshopProvider;
        }

        private void Start()
        {
            _actualCarWorkshopData = _carWorkshopProvider.GetCarWorkshopData(_carType);

            UpdateTextInfo();
        }

        private void UpdateTextInfo()
        {
            _maxSpeedText.text = _actualCarWorkshopData.ActualMaxSpeed.ToString(CultureInfo.InvariantCulture);
            _accelerationText.text = _actualCarWorkshopData.ActualAcceleration.ToString(CultureInfo.InvariantCulture);
            _suspensionText.text = _actualCarWorkshopData.ActualSuspension.ToString(CultureInfo.InvariantCulture);
            _brakingText.text = _actualCarWorkshopData.ActualBraking.ToString(CultureInfo.InvariantCulture);
        }
    }
}
