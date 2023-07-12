using System;
using System.Collections.Generic;
using ACS.Data.DataService.Model;
using Codebase.Gameplay.Enums;
using Newtonsoft.Json;

namespace Codebase.Gameplay.DataModels
{
    [UnityEngine.Scripting.Preserve]
    public class CarUpgradesModel : ProgressModel
    {
        [JsonProperty] private Dictionary<CarType, CarUpgradeData> _carUpgradeDatas = new Dictionary<CarType, CarUpgradeData>();

        public CarUpgradeData GetCarUpgradeData(CarType carType)
        {
            TryAddNewData(carType);
            
            return _carUpgradeDatas[carType];
        }

        public void AppendUpgrade(CarType carType, CarUpgradeType upgradeType)
        {
            TryAddNewData(carType);
            
            switch (upgradeType)
            {
                case CarUpgradeType.ENGINE:
                    _carUpgradeDatas[carType].EngineLevel++;
                    break;
                
                case CarUpgradeType.TRANSMISSION: 
                    _carUpgradeDatas[carType].TransmissionLevel++;
                    break;
                
                case CarUpgradeType.SUSPENSION:
                    _carUpgradeDatas[carType].SuspensionLevel++;
                    break;
                
                case CarUpgradeType.ADHESION:
                    _carUpgradeDatas[carType].AdhesionLevel++;
                    break;
            }
        }

        private void TryAddNewData(CarType carType)
        {
            if(!_carUpgradeDatas.ContainsKey(carType))
                _carUpgradeDatas.Add(carType, new CarUpgradeData());
        }
    }

    [Serializable]
    public class CarUpgradeData
    {
        public int EngineLevel = 1;
        public int TransmissionLevel = 1;
        public int SuspensionLevel = 1;
        public int AdhesionLevel = 1;
    }
}
