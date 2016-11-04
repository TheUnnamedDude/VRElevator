using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DependencyInstaller : MonoInstaller<DependencyInstaller>
{
    [SerializeField]
    private Settings _settings;

    public override void InstallBindings()
    {
        Container.Bind<GameObject>().WithId("Elevator").FromInstance(_settings.Elevator);
        Container.Bind<GameObject>().WithId("Scene").FromInstance(_settings.Scene);
        Container.Bind<GameController>().FromNew().AsSingle();
        Container.BindAllInterfaces<GameController>().To<GameController>().AsSingle();
        Container.Bind<ScoreManager>().FromNew().AsSingle();
        Container.Bind<ITickable>().To<ScoreManager>().AsSingle();
        Container.Bind<LevelGenerator>().FromNew().AsSingle();
        Container.BindAllInterfaces<LevelGenerator>().To<LevelGenerator>().AsSingle();
    }

    [Serializable]
    public class Settings
    {
        public GameObject Elevator;
        public GameObject Scene;
    }

}