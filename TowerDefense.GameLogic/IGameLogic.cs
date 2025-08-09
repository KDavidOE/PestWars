// <copyright file="IGameLogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameLogic
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using TowerDefense.GameModel;
    using TowerDefense.Repository;

    /// <summary>
    /// Describes an interface that can be used as game logic.
    /// </summary>
    public interface IGameLogic
    {
        /// <summary>
        /// Updates the model on every tick.
        /// </summary>
        void Update();

        /// <summary>
        /// Returns if the game has already ended or not.
        /// </summary>
        /// <returns>Whether the game has already ended.</returns>
        bool GameEnded();

        /// <summary>
        /// Returns whether the game is lost or not.
        /// </summary>
        /// <returns>Whether the game is lost.</returns>
        bool GameLost();

        /// <summary>
        /// Moves all enemies closer to the base.
        /// </summary>
        void MoveEnemies();

        /// <summary>
        /// Spawns and moves projectiles.
        /// </summary>
        void AttackWithTowers();

        /// <summary>
        /// Spawns a new wave of enemies after the last ended.
        /// </summary>
        void SpawnNewWave();

        /// <summary>
        /// Checks if the previous wave has already ended.
        /// </summary>
        /// <returns>If the wave already ended.</returns>
        bool CheckWaveEnded();

        /// <summary>
        /// Allows the user to select a new tower.
        /// </summary>
        /// <param name="mousePos">The point at which the user has clicked.</param>
        void SelectTower(Point mousePos);

        /// <summary>
        /// Puts a new tower on the map.
        /// </summary>
        /// <param name="mousePos">The position of the mouse.</param>
        /// <returns>Whether the placement was successful or not.</returns>
        bool AddTower(Point mousePos);

        /// <summary>
        /// Pauses or resumes the game.
        /// </summary>
        void PauseOrResumeGame();

        /// <summary>
        /// Creates a new game.
        /// </summary>
        void CreateNewGame();

        /// <summary>
        /// Saves the game.
        /// </summary>
        /// <returns>If the save was successful or not.</returns>
        bool SaveGame();

        /// <summary>
        /// Loads the game.
        /// </summary>
        /// <returns>Whether the load was sucessful or not.</returns>
        bool LoadGame();

        /// <summary>
        /// Saves the high score.
        /// </summary>
        /// <returns>If the save was successful or not.</returns>
        /// <param name="name">The name of the player.</param>
        bool SaveHighscore(string name);

        /// <summary>
        /// Loads the high score.
        /// </summary>
        /// <returns>If the load was successful or not.</returns>
        bool LoadHighscore();

        /// <summary>
        /// Returns the player's score.
        /// </summary>
        /// <returns>The current score of the player.</returns>
        IList<PlayerStats> GetHighScores();

        /// <summary>
        /// Returns the list of possible enemies.
        /// </summary>
        /// <returns>A dictionary of possible enemies.</returns>
        IDictionary<EnemyType, IEnemy> GetAvailableEnemies();

        /// <summary>
        /// Returns the list of possible towers.
        /// </summary>
        /// <returns>A dictionary with the available towers.</returns>
        IDictionary<TowerType, ITower> GetAvailableTowers();

        /// <summary>
        /// Gets the currents score of the game.
        /// </summary>
        /// <returns>The current score of the game.</returns>
        int GetCurrentScore();

        /// <summary>
        /// Gets the current refresh rate of the game.
        /// </summary>
        /// <returns>Returns the integer value of the refresh rate.</returns>
        int GetRefreshRate();

        /// <summary>
        /// Returns whether the game is paused.
        /// </summary>
        /// <returns> Returns the integer value of the refresh rate.</returns>>
        public bool IsPaused();
    }
}
