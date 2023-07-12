using ACS.Data.DataService.Model;
using Codebase.Gameplay.Enums;
using Newtonsoft.Json;

namespace Codebase.Gameplay.DataModels
{
    [UnityEngine.Scripting.Preserve]
    public class PlayerModel : ProgressModel
    {
        [JsonProperty] private CarType _currentCar = CarType.DEFAULT;
        [JsonProperty] private int _currentLevel = 1;
        [JsonProperty] private long _currentExperience = 0;

        public CarType GetCurrentCarType() => _currentCar;
        public CarType SetCurrentCarType(CarType targetType) => _currentCar = targetType;
        
        public int GetCurrentLevel() => _currentLevel;
        public int SetCurrentLevel(int targetLevel) => _currentLevel = targetLevel;
        
        public long GetCurrentExperience() => _currentExperience;
        public long SetCurrentExperience(long targetExperience) => _currentExperience = targetExperience;
    }
}
