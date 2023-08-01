// <copyright file="TowerData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.Data
{
    /// <summary>
    /// Class that represents data for towers.
    /// </summary>
    public class TowerData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TowerData"/> class.
        /// </summary>
        public TowerData()
        {
        }

        /// <summary>
        /// Gets or Sets the name of the tower.
        /// </summary>
        public string TowerName { get; set; }

        /// <summary>
        /// Gets or Sets the damage of the tower.
        /// </summary>
        public double TowerDamage { get; set; }

        /// <summary>
        /// Gets or Sets the range of the tower.
        /// </summary>
        public double TowerRange { get; set; }

        /// <summary>
        /// Gets or Sets the attack speed of the tower.
        /// </summary>
        public double TowerAttackSpeed { get; set; }

        /// <summary>
        /// Gets or Sets the price of the tower.
        /// </summary>
        public double TowerPrice { get; set; }
    }
}
