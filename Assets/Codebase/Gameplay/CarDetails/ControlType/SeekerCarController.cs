using Sirenix.OdinInspector;
using UnityEngine;

namespace Codebase.Gameplay.CarDetails.ControlType
{
    public class SeekerCarController : MonoBehaviour
    {
        [SerializeField] private Car _targetCar;
        [SerializeField] private Car _target;

        private SeekerAI _seekerAI;
        
        [Button]
        private void SetupSeeker()
        {
            _seekerAI = new SeekerAI(_targetCar, _target);
        }
        
    }
}
