using Codebase.Gameplay.CarDetails;
using UnityEngine;

namespace Codebase.UI
{
    public class InGameCarInterface : MonoBehaviour
    {
        [SerializeField] private FuelIndicatorView _fuelIndicatorView;

        public void Init(Car targetCar)
        {
            _fuelIndicatorView.Init(targetCar);
        }
    }
}
