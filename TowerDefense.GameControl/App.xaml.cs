// <copyright file="App.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl
{
    using System.Windows;
    using CommonServiceLocator;
    using TowerDefense.GameControl.Logic;
    using TowerDefense.GameLogic;
    using TowerDefense.GameModel;
    using TowerDefense.Repository;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            ServiceLocator.SetLocatorProvider(() => GameIOC.Instance);
            GameIOC.Instance.Register<IStorageRepository, StorageRepository>();
            GameIOC.Instance.Register<IGameLogic, MainGameLogic>();
            GameIOC.Instance.Register<IManualLogic, ManualLogic>();
            GameIOC.Instance.Register<IGameModel>(() => new GameBaseModel(0, 0));
            GameIOC.Instance.Register<IScoreBoardLogic, ScoreBoardLogic>();
        }
    }
}
