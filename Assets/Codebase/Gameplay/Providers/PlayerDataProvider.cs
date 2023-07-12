using ACS.Data.DataService.Service;
using ACS.SignalBus.SignalBus;
using Codebase.Gameplay.Counters;
using Codebase.Gameplay.DataModels;
using Codebase.Gameplay.Enums;
using Codebase.Gameplay.Resources;

namespace Codebase.Gameplay.Providers
{
    public class PlayerDataProvider
    {
        private readonly ISignalBusService _signalBusService;
        private readonly IDataService _dataService;
        private PlayerModel _playerModel;

        public PlayerDataProvider(IDataService dataService, ISignalBusService signalBusService)
        {
            _dataService = dataService;
            _signalBusService = signalBusService;
        
            dataService.ModelsDataChanged += () => OnModelsDataChanged(dataService);
            OnModelsDataChanged(dataService);
        }
        
        public CarType GetCurrentType() => _playerModel.GetCurrentCarType();
        public CarType SetCurrentType(CarType targetType) => _playerModel.SetCurrentCarType(targetType);
        
        public int GetCurrentLevel() => _playerModel.GetCurrentLevel();
        public int SetCurrentLevel(int targetLevel) => _playerModel.SetCurrentLevel(targetLevel);
        
        public long GetCurrentExperience() => _playerModel.GetCurrentExperience();
        public long SetCurrentExperience(long targetExperience) => _playerModel.SetCurrentExperience(targetExperience);
        
        private void OnModelsDataChanged(IDataService dataService) => _playerModel = _dataService.Models.Resolve<PlayerModel>();
    }
}