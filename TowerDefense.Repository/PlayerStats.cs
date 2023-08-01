// <copyright file="PlayerStats.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.Repository
{
    /// <summary>
    /// Class to represent the player.
    /// </summary>
    public class PlayerStats
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerStats"/> class.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <param name="score">The score of the player.</param>
        public PlayerStats(string name, string score)
        {
            this.Name = name;
            this.Score = score;
        }

        /// <summary>
        /// Gets or Sets the name of the player.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the score of the player.
        /// </summary>
        public string Score { get; set; }
    }
}
