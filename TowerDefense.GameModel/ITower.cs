// <copyright file="ITower.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for towers to declare necessery variables.
    /// </summary>
    public interface ITower
    {
        /// <summary>
        /// Gets or Sets the damage of a tower.
        /// </summary>
        int Damage { get; set; }

        /// <summary>
        /// Gets or Sets the range of a tower.
        /// </summary>
        double Range { get; set; }

        /// <summary>
        /// Gets or Sets the position of a tower object. This referes to a specific map tile horizontally.
        /// </summary>
        int TileX { get; set; }

        /// <summary>
        /// Gets or Sets the position of a tower object. This referes to a specific map tile vertically.
        /// </summary>
        int TileY { get; set; }

        /// <summary>
        /// Gets or Sets the price of a tower.
        /// </summary>
        int Price { get; set; }

        /// <summary>
        /// Gets the collection of all the projectiles that has been shot and did not arrived at destination.
        /// </summary>
        IList<IProjectile> Projectiles { get; }

        /// <summary>
        /// Gets or Sets the interval between each attack of the tower.
        /// </summary>
        double AttackInterval { get; set; }

        /// <summary>
        /// Gets or sets the number of frames until the tower attacks again.
        /// </summary>
        double AttackTimer { get; set; }

        /// <summary>
        /// Gets or Sets the defense tower's type.
        /// </summary>
        public TowerType DefenseType { get; set; }

        /// <summary>
        /// Copies the values of another tower into the new one.
        /// </summary>
        /// <param name="other">Other tower.</param>
        public void CopyFrom(ITower other);
    }
}
