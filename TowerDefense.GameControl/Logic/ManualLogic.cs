// <copyright file="ManualLogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using TowerDefense.GameControl.Data;
    using TowerDefense.GameLogic;
    using TowerDefense.Repository;

    /// <summary>
    /// Logic for handling manual window querrys.
    /// </summary>
    public class ManualLogic : IManualLogic
    {
        private readonly IGameLogic gamelog;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualLogic"/> class.
        /// </summary>
        /// <param name="log">Game Logic.</param>
        public ManualLogic(IGameLogic log)
        {
            this.gamelog = log;
        }

        /// <inheritdoc/>
        public IList<EnemyData> GetAllEnemies()
        {
            ObservableCollection<EnemyData> enemydata = new ObservableCollection<EnemyData>();
            foreach (var enemy in this.gamelog.GetAvailableEnemies())
            {
                EnemyData tw = ConvertModelData.ConvertToTowerData(enemy.Value);
                enemydata.Add(tw);
            }

            return enemydata;
        }

        /// <inheritdoc/>
        public IList<TowerData> GetAllTowers()
        {
            ObservableCollection<TowerData> towersdata = new ObservableCollection<TowerData>();
            foreach (var tower in this.gamelog.GetAvailableTowers())
            {
                TowerData tw = ConvertModelData.ConvertToTowerData(tower.Value);
                towersdata.Add(tw);
            }

            return towersdata;
        }
    }
}
