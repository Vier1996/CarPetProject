using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Codebase.Gameplay.CarDetails.Config
{
    [CreateAssetMenu(fileName = "CarConfigs", menuName = "App/Car/CarConfigs")]
    public class CarConfigs : ScriptableObject
    {
        [SerializeField] private List<CarConfig> _configs = new List<CarConfig>();

        public CarConfig GetConfig(CarType type)
        {
            CarConfig config = _configs.FirstOrDefault(cfg => cfg.CarType == type);

            if (config == default)
                throw new Exception($"Can not get setup for type:[{type}]");

            return config;
        }
    }
}
