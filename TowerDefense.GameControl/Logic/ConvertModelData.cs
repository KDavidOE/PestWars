// <copyright file="ConvertModelData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.Logic
{
    using TowerDefense.GameControl.Data;
    using TowerDefense.GameModel;

    /// <summary>
    /// Stataic class the helps converting between data.
    /// </summary>
    public static class ConvertModelData
    {
        /// <summary>
        /// Convert to enemy data.
        /// </summary>
        /// <param name="enemydata">The base model for the enemy.</param>
        /// <returns>Returns an upper layer enemy data.</returns>
        public static EnemyData ConvertToTowerData(IEnemy enemydata)
        {
            EnemyData enemy = new EnemyData();

            if (enemydata != null)
            {
                enemy.Name = enemydata.ZombieType.ToString();
                enemy.Damage = enemydata.Damage;
                enemy.Speed = enemydata.Speed;
                enemy.Health = enemydata.Health;
            }

            return enemy;
        }

        /// <summary>
        /// Convert to tower data.
        /// </summary>
        /// <param name="tower">The base model for tower.</param>
        /// <returns>Returns an upper layer tower data..</returns>
        public static TowerData ConvertToTowerData(ITower tower)
        {
            TowerData tw = new TowerData();

            if (tower != null)
            {
                tw.TowerName = tower.DefenseType.ToString();
                tw.TowerDamage = tower.Damage;
                tw.TowerRange = tower.Range;
                tw.TowerAttackSpeed = tower.AttackInterval;
                tw.TowerPrice = tower.Price;
            }

            return tw;
        }
    }
}
