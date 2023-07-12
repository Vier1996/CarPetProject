using Codebase.Gameplay.Providers;
using Zenject;

namespace Codebase.Bootstrapper.Project
{
    public class ContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerDataProvider>().FromNew().AsSingle().Lazy();
            Container.Bind<GameplayResourceProvider>().FromNew().AsSingle().Lazy();
            Container.Bind<CarWorkshopProvider>().FromNew().AsSingle().Lazy();
        }
    }
}
