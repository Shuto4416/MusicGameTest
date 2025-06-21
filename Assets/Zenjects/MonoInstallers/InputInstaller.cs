using UnityEngine;
using Zenject;
using InputSystem;

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
        }
    }
}
