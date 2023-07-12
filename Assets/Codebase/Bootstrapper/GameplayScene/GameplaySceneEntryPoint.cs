using Codebase.Gameplay.Entities;
using Codebase.Gameplay.Managers;
using UnityEngine;

namespace Codebase.Bootstrapper.GameplayScene
{
    public class GameplaySceneEntryPoint : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private GameplayManager _gameplayManager;
        [SerializeField] private GameplaySceneCompositor _sceneCompositor;
        
        private void Start() => PrepareScene();

        private void PrepareScene()
        {
            _gameplayManager.Init(_player);
            _player.Init();
            _sceneCompositor.Composite(_player.GetPlayerCar());
        }
    }
}
