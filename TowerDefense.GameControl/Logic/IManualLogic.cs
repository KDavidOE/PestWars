// <copyright file="IManualLogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.Logic
{
    using System.Collections.Generic;
    using TowerDefense.GameControl.Data;

    /// <summary>
    /// Interface for manual logic.
    /// </summary>
    public interface IManualLogic
    {
        /// <summary>
        /// Gets all buildable towers.
        /// </summary>
        /// <returns>Returns a collection of Tower Data.</returns>
        public IList<TowerData> GetAllTowers();

        /// <summary>
        /// Gets all possible enemies.
        /// </summary>
        /// <returns>Returns a collection of Enemy Data.</returns>
        public IList<EnemyData> GetAllEnemies();
    }
}
