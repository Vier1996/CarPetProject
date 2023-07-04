using Codebase.Gameplay.CarDetails;
using Codebase.UI;
using UnityEngine;

namespace Codebase.Bootstrapper.GameplayScene
{
    public class GameplaySceneCompositor : MonoBehaviour
    {
        [SerializeField] private InGameCarInterface _inGameCarInterface;

        public void Composite(Car targetCar)
        {
            _inGameCarInterface.Init(targetCar);
        }
    }
}
