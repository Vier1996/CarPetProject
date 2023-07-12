using Codebase.Gameplay.CarDetails;
using Codebase.Gameplay.CarDetails.ControlType;
using Codebase.Gameplay.Triggers;
using UnityEngine;

namespace Codebase.Gameplay.Entities
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Trigger _pursuitTrigger;
        
        [SerializeField] private Car _targetCar;
        [SerializeField] private ManualCarController _manualCarController;
        
        public void Init()
        {
            _targetCar.Init();
            _targetCar.SwitchOnCar();
            
            _manualCarController.Init(_targetCar);
        }

        public Trigger GetPursuitTrigger() => _pursuitTrigger;
        public Car GetPlayerCar() => _targetCar;
    }
}
