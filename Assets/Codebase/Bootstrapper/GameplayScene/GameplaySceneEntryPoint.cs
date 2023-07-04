using Codebase.Gameplay.CarDetails;
using UnityEngine;

namespace Codebase.Bootstrapper.GameplayScene
{
    public class GameplaySceneEntryPoint : MonoBehaviour
    {
        [SerializeField] private Car _targetCar;
        [SerializeField] private GameplaySceneCompositor _sceneCompositor;
        
        private void Start() => PrepareScene();

        private void PrepareScene()
        {
            _targetCar.Init();
            
            _sceneCompositor.Composite(_targetCar);
        }
    }
}
