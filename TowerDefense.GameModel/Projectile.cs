// <copyright file="Projectile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameModel
{
    using System.Windows;

    /// <summary>
    /// Class that represents the projectiles that towers shoot.
    /// </summary>
    public class Projectile : IProjectile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Projectile"/> class.
        /// </summary>
        /// <param name="shooter">The tower which creates the projectile.</param>
        /// <param name="target">The target which will be damaged.</param>
        /// <param name="point">The position of the projectile.</param>
        public Projectile(ITower shooter, IEnemy target, Point point)
        {
            this.Target = target;
            this.Shooter = shooter;
            this.Position = point;
        }

        /// <inheritdoc/>
        public IEnemy Target { get; set; }

        /// <inheritdoc/>
        public ITower Shooter { get; set; }

        /// <inheritdoc/>
        public Point Position { get; set; }

        /// <inheritdoc/>
        public double Angle { get; set; }
    }
}
