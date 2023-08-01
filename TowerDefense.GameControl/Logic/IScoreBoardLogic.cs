// <copyright file="IScoreBoardLogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.Logic
{
    using System.Collections.Generic;
    using TowerDefense.GameControl.Data;
    using TowerDefense.GameLogic;
    using TowerDefense.Repository;

    /// <summary>
    /// Logic interface for getting scoreboard.
    /// </summary>
    public interface IScoreBoardLogic
    {
        /// <summary>
        /// Gets or Sets the logic of the game.
        /// </summary>
        public IGameLogic GLogic { get; set; }

        /// <summary>
        /// Method for getting the scores.
        /// </summary>
        /// <returns>Returns a collection of PlayerScores.</returns>
        public IList<PlayerScore> GetScores();

        /// <summary>
        /// Method that decides if can be loaded.
        /// </summary>
        /// <returns>Returns true if saved game can be loaded, or false if it can not be loaded.</returns>
        public bool SaveGameCanBeLoaded();
    }
}
