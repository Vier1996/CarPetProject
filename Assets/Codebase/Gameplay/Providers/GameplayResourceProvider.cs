using ACS.Data.DataService.Service;
using ACS.SignalBus.SignalBus;
using Codebase.Gameplay.Counters;
using Codebase.Gameplay.DataModels;
using Codebase.Gameplay.Resources;

namespace Codebase.Gameplay.Providers
{
    public class GameplayResourceProvider
    {
        private readonly ISignalBusService _signalBusService;

        private readonly IDataService _dataService;
        private ResourceModel _resourceModel;

        public GameplayResourceProvider(IDataService dataService, ISignalBusService signalBusService)
        {
            _dataService = dataService;
            _signalBusService = signalBusService;
        
            dataService.ModelsDataChanged += () => OnModelsDataChanged(dataService);
            OnModelsDataChanged(dataService);
        }
        
        public long GetResourceAmount(GameplayResourceType resourceType) => _resourceModel.Get(resourceType);

        public void SetResourceAmount(GameplayResourceType resourceType, long amount)
        {
            _resourceModel.Set(resourceType, amount);

            _signalBusService.Fire(new ResourceChangeSignal()
            {
                ResourceType = resourceType,
                ResourceChangeAmount = amount
            });
        }

        public void AppendResourceAmount(GameplayResourceType resourceType, long amount)
        {
            long appendedAmount = _resourceModel.Get(resourceType) + amount;
            SetResourceAmount(resourceType, appendedAmount);
        }
        
        private void OnModelsDataChanged(IDataService dataService) => _resourceModel = _dataService.Models.Resolve<ResourceModel>();
    }
}