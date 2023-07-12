using Codebase.Gameplay.CarDetails.Config;
using UnityEngine;
using Zenject;

namespace Codebase.Bootstrapper.Project
{
    [CreateAssetMenu(fileName = "ScriptablesInstaller", menuName = "App/Zenject/ScriptablesInstaller")]
    public class ScriptablesInstaller : ScriptableObjectInstaller<ScriptablesInstaller>
    {
        [SerializeField] private CarConfigs statsConfigs;
        
        public override void InstallBindings()
        {
            Container.Bind<CarConfigs>().FromInstance(statsConfigs).AsSingle().Lazy();
        }
    }
}
