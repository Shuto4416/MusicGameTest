using UnityEngine;
using Zenject;
using InputSystem;
using Interface.FileLoader;
using Classes.FileLoader;

namespace ZenjectSample
{
    public class InputInstaller : MonoInstaller<InputInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IInputProvider>()
                .To<InputProvider>()
                .AsCached();
            Container
                .Bind<IFileLoader>()
                .To<FileLoader>()
                .AsCached();
            Container
                .Bind<ISelectInputProvider>()
                .To<SelectInputProvider>()
                .AsCached();
        }
    }
}
