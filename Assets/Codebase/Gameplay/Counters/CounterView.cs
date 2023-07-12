using ACS.SignalBus.SignalBus;
using ACS.SignalBus.SignalBus.Parent;
using Codebase.Gameplay.Providers;
using Codebase.Gameplay.Resources;
using Codebase.Utils;
using UnityEngine;
using Zenject;

namespace Codebase.Gameplay.Counters
{
    [RequireComponent(typeof(CounterText))]
    public class CounterView : MonoBehaviour
    {
        [SerializeField] protected GameplayResourceType _resourceType;
        [SerializeField] private bool _longFormat;

        private CounterText _counter;
        private GameplayResourceProvider _resourceProvider;
        private ISignalBusService _signalBusService;

        [Inject] private void Init(GameplayResourceProvider resourceProvider, ISignalBusService signalBusService)
        {
            _resourceProvider = resourceProvider;
            _signalBusService = signalBusService;
            
            _counter = GetComponent<CounterText>();
            _counter.Init();
            
            Setup();
        }

        private void Setup()
        {
            UpdateCounter(_resourceProvider.GetResourceAmount(_resourceType), true);
            
            _signalBusService.Subscribe<ResourceChangeSignal>(OnCounterChanged);
            
            _counter.SetCustomFormatter(v => _longFormat ? TextUtils.FormatDouble(v) : TextUtils.FormatShort(v));
        }
        
        private void OnCounterChanged(ResourceChangeSignal signal)
        {
            if(signal.ResourceType == _resourceType)
                UpdateCounter(signal.ResourceChangeAmount);
        }

        private void UpdateCounter(double changeValue, bool initialization = false)
        {
            _counter.SetDuration(initialization ? 0f : 1f);
            _counter.ChangeTargetValue(changeValue, true);
        }
        
        protected void OnDisable() => _signalBusService.Unsubscribe<ResourceChangeSignal>(OnCounterChanged);
    }
    
    [Signal]
    public class ResourceChangeSignal
    {
        public GameplayResourceType ResourceType;
        public double ResourceChangeAmount;
    }
}