// <copyright file="IStorageRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.Repository
{
    using System.Collections.Generic;
    using TowerDefense.GameModel;

    /// <summary>
    /// Interface for game storage which saves and loads the game.
    /// </summary>
    public interface IStorageRepository
    {
        /// <summary>
        /// Gets  the current game model.
        /// </summary>
        IGameModel CurrentGame { get; }

        /// <summary>
        /// Gets a collection of all time highscores.
        /// </summary>
        public IList<PlayerStats> HighScores { get; }

        /// <summary>
        /// Creates a new game.
        /// </summary>
        void CreateNewGame();

        /// <summary>
        /// Loads previously saved game.
        /// </summary>
        /// <param name="saveFilePath">The path to that save file.</param>
        /// <returns>Returns true if loading was successful.</returns>
        bool LoadGame(string saveFilePath);

        /// <summary>
        /// Saves the current game.
        /// </summary>
        /// <param name="saveFilePath">The path to the save file.</param>
        /// <returns>Returns true if saving was successful.</returns>
        bool SaveGame(string saveFilePath);

        /// <summary>
        /// Loads high scores.
        /// </summary>
        /// <param name="highscorePath">The path to the highscores file.</param>
        /// <returns>Returns true if loading was successful.</returns>
        bool LoadHighscores(string highscorePath);

        /// <summary>
        /// Saves the current score of the game.
        /// </summary>
        /// <param name="playerName">The path to the highscores file.</param>
        /// <returns>Returns true if saving was successful.</returns>
        bool SaveHighscores(string playerName);

        /// <summary>
        /// Gets the enemies that are currently used in the game files.
        /// </summary>
        /// <returns>Returns a collection of IEnemy.</returns>
        public Dictionary<EnemyType, IEnemy> LoadEnemies();

        /// <summary>
        /// Gets the towers that are currently in the game files.
        /// </summary>
        /// <returns>Return a collection of ITower.</returns>
        public Dictionary<TowerType, ITower> LoadTowers();
    }
}
