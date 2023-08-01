// <copyright file="ManualWindowViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.VM
{
    using System.Collections.ObjectModel;
    using CommonServiceLocator;
    using GalaSoft.MvvmLight;
    using TowerDefense.GameControl.Data;
    using TowerDefense.GameControl.Logic;

    /// <summary>
    /// Manual window view model for handling data.
    /// </summary>
    public class ManualWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManualWindowViewModel"/> class.
        /// </summary>
        /// <param name="log">Manual logic.</param>
        public ManualWindowViewModel(IManualLogic log)
        {
            this.Towers = new ObservableCollection<TowerData>();
            this.Enemies = new ObservableCollection<EnemyData>();
            if (log != null)
            {
                this.Towers = (ObservableCollection<TowerData>)log.GetAllTowers();
                this.Enemies = (ObservableCollection<EnemyData>)log.GetAllEnemies();
            }

            if (this.IsInDesignMode)
            {
                this.Towers.Add(new TowerData() { TowerName = "NoData Tower", TowerAttackSpeed = 10, TowerDamage = 100, TowerPrice = 12345, TowerRange = 250, });
                this.Enemies.Add(new EnemyData() { Name = "NoDataZombie", Damage = 11, Health = 150, Speed = 2, });
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualWindowViewModel"/> class.
        /// </summary>
        public ManualWindowViewModel()
            : this(IsInDesignModeStatic ? null : ServiceLocator.Current.GetInstance<IManualLogic>())
        {
        }

        /// <summary>
        /// Gets the collection of all buildable towers.
        /// </summary>
        public ObservableCollection<TowerData> Towers { get; private set; }

        /// <summary>
        /// Gets the collection of all possible enemies.
        /// </summary>
        public ObservableCollection<EnemyData> Enemies { get; private set; }
    }
}
