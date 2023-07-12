using System;
using ACS.Data.DataService.Service;
using ACS.SignalBus.SignalBus;
using Codebase.Gameplay.CarDetails.Config;
using Codebase.Gameplay.DataModels;
using Codebase.Gameplay.Enums;

namespace Codebase.Gameplay.Providers
{
    public class CarWorkshopProvider
    {
        private readonly ISignalBusService _signalBusService;
        private readonly IDataService _dataService;

        private CarConfigs _carConfigs;
        private CarUpgradesModel _carUpgradesModel;

        public CarWorkshopProvider(IDataService dataService, ISignalBusService signalBusService, CarConfigs carConfigs)
        {
            _dataService = dataService;
            _signalBusService = signalBusService;
            _carConfigs = carConfigs;
            
            dataService.ModelsDataChanged += () => OnModelsDataChanged(dataService);
            OnModelsDataChanged(dataService);
        }

        public CarWorkshopData GetCarWorkshopData(CarType carType)
        {
            CarConfig config = _carConfigs.GetConfig(carType);
            CarUpgradeData upgradeData = _carUpgradesModel.GetCarUpgradeData(carType);
            
            return new CarWorkshopData()
            {
                ActualMaxSpeed = CropFloat((float)(config.MaxForwardSpeed * Math.Pow(1.05f, upgradeData.EngineLevel))),
                ActualAcceleration = CropFloat((float)(config.AccelerationMultiplier * Math.Pow(1.05f, upgradeData.TransmissionLevel))),
                ActualSuspension = CropFloat((float)(config.MaxSteeringAngle * Math.Pow(1.05f, upgradeData.SuspensionLevel))),
                ActualBraking = CropFloat((float)(config.BrakeForce * Math.Pow(1.05f, upgradeData.AdhesionLevel))),
            };
        }

        private float CropFloat(float rawFloat) => 
            (float)Math.Round((decimal)rawFloat, 1);

        private void OnModelsDataChanged(IDataService dataService) => _carUpgradesModel = _dataService.Models.Resolve<CarUpgradesModel>();
    }

    public class CarWorkshopData
    {
        public float ActualMaxSpeed;
        public float ActualAcceleration;
        public float ActualSuspension;
        public float ActualBraking;
    }
}
