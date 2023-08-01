// <copyright file="EnemyData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.Data
{
    /// <summary>
    /// Class that represents enemy data.
    /// </summary>
    public class EnemyData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyData"/> class.
        /// </summary>
        public EnemyData()
        {
        }

        /// <summary>
        /// Gets or Sets the name of the enemy.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets speed of the enemy.
        /// </summary>
        public double Speed { get; set; }

        /// <summary>
        /// Gets or Sets the health of the enemy.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Gets or Sets the damage of the enemy.
        /// </summary>
        public int Damage { get; set; }
    }
}
