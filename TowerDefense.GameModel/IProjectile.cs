// <copyright file="IProjectile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameModel
{
    using System.Windows;

    /// <summary>
    /// Interface for projectiles.
    /// </summary>
    public interface IProjectile
    {
        /// <summary>
        /// Gets or Sets the targeted unit.
        /// </summary>
        public IEnemy Target { get; set; }

        /// <summary>
        /// Gets or Sets the shooter of the projectile.
        /// </summary>
        public ITower Shooter { get; set; }

        /// <summary>
        /// Gets or Sets the position of the projectile.
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Gets or Sets the angle of the projectile relative to shooter-target position.
        /// </summary>
        public double Angle { get; set; }
    }
}