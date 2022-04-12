using UnityEngine;
using Zenject;

public class GardenerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Container.Bind<GameManager>().AsSingle().NonLazy();
    }
}