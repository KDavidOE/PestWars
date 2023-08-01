// <copyright file="MainGameLogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

[assembly:CLSCompliant(false)]

namespace TowerDefense.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using TowerDefense.GameModel;
    using TowerDefense.Repository;

    /// <inheritdoc/>
    public class MainGameLogic : IGameLogic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainGameLogic"/> class.
        /// </summary>
        /// <param name="repo">The repostiory used for the game.</param>
        public MainGameLogic(IStorageRepository repo)
        {
            this.Repo = repo;
            this.Model = this.Repo.CurrentGame;
            this.Rnd = new Random();
        }

        private List<int> SpawnTimers { get; set; } = new List<int>();

        private IStorageRepository Repo { get; set; }

        private IGameModel Model { get; set; }

        private Random Rnd { get; set; }

        /// <inheritdoc/>
        public void SelectTower(Point mousePos)
        {
            int towerX = (int)(mousePos.X / this.Model.TileSize);
            int towerY = (int)(mousePos.Y / this.Model.TileSize);

            if (towerY < this.Model.Map.GetLength(0) && towerY >= 0 && towerX < this.Model.Map.GetLength(1) && towerX >= 0)
            {
                if (this.Model.Map[towerY, towerX] == Map.OCCUPIED)
                {
                    var tower = this.Model.Towers.ToList().Find(t => t.TileX == towerX && t.TileY == towerY);
                    this.Model.EditedTower = tower;
                }
                else
                {
                    this.Model.EditedTower = null;
                }
            }

            int mouseX = (int)(mousePos.X - (this.Model.GameWidth * this.Model.MapShopRatio));
            int mX = (int)(mouseX / (this.Model.GameWidth * 0.1));
            int mY = (int)(mousePos.Y / this.Model.ShopTileHeight);

            int index = (mY * 2) + mX;

            if (mouseX < 0 || mX < 0 || mY < 0)
            {
                return;
            }

            if (index < 0 || index >= this.Model.AvailableTowers.Count)
            {
                return;
            }

            if (this.Model.SelectedTower != null && this.Model.SelectedTower.DefenseType == this.Model.AvailableTowers.ElementAt(index).Value.DefenseType)
            {
                this.Model.SelectedTower = null;
            }
            else
            {
                this.Model.SelectedTower = this.Model.AvailableTowers.ElementAt(index).Value;
            }
        }

        /// <inheritdoc/>
        public bool AddTower(Point mousePos)
        {
            this.SelectTower(mousePos);

            if (this.Model.SelectedTower == null)
            {
                return false;
            }

            if (this.Model.Money < this.Model.SelectedTower.Price)
            {
                return false;
            }

            int candidateX = (int)(mousePos.X / this.Model.TileSize);
            int candidateY = (int)(mousePos.Y / this.Model.TileSize);

            if (candidateX < 0 || candidateX >= this.Model.Map.GetLength(1) || candidateY < 0 || candidateY >= this.Model.Map.GetLength(0))
            {
                return false;
            }

            if (!this.CanPlaceOnThisTile(candidateX, candidateY))
            {
                return false;
            }

            var orig = this.Model.SelectedTower;
            var tower = new Tower();

            tower.CopyFrom(orig);
            tower.TileX = candidateX;
            tower.TileY = candidateY;

            if (!(tower.DefenseType == TowerType.Water))
            {
                this.Model.Map[candidateY, candidateX] = Map.OCCUPIED;
            }

            this.Model.Towers.Add(tower);
            this.Model.Money -= tower.Price;
            this.Model.SelectedTower = null;

            return true;
        }

        /// <inheritdoc/>
        public void AttackWithTowers()
        {
            foreach (ITower tower in this.Model.Towers)
            {
                if (tower.AttackTimer == 0)
                {
                    this.SpawnProjectile(tower);
                }

                if (tower.DefenseType != TowerType.Hunter && tower.DefenseType != TowerType.Wizard)
                {
                    (tower.Projectiles as List<IProjectile>).RemoveAll(p => p.Target == null || p.Target.Health <= 0);
                }

                for (int i = 0; i < tower.Projectiles.Count; i++)
                {
                    Projectile proj = (Projectile)tower.Projectiles[i];

                    switch (tower.DefenseType)
                    {
                        /* Normal bullet movement. */
                        case TowerType.Archer:
                        case TowerType.Cannon:
                        case TowerType.Doctor:
                        case TowerType.Catapult:
                        case TowerType.Knight:
                            if (proj.Target != null)
                            {
                                double angle = Math.Atan2(proj.Target.Ypos - proj.Position.Y, proj.Target.Xpos - proj.Position.X);
                                double xDir = Math.Cos(angle) * 20;
                                double yDir = Math.Sin(angle) * 20;

                                proj.Position = new Point(proj.Position.X + xDir, proj.Position.Y + yDir);
                                proj.Angle = angle;
                            }

                            break;
                        /* Do nothing. */
                        case TowerType.Hunter:
                        case TowerType.Water:
                        case TowerType.Wizard:
                        default:
                            break;
                    }

                    if (tower.DefenseType != TowerType.Hunter && tower.DefenseType != TowerType.Wizard)
                    {
                        IEnemy enemy = proj.Target;
                        if (Math.Abs(proj.Position.X - enemy.Xpos) <= 20 && Math.Abs(proj.Position.Y - enemy.Ypos) <= 20)
                        {
                            enemy.Health -= proj.Shooter.Damage;

                            if (enemy.SlowTimer == -1 && tower.DefenseType == TowerType.Catapult)
                            {
                                enemy.SlowTimer = 160;
                            }

                            if (enemy.Health <= 0)
                            {
                                this.Model.Money += 200;
                                this.Model.Score += this.Model.BaseHealth * 10;
                            }

                            tower.Projectiles.Remove(proj);
                        }
                    }
                    else
                    {
                        foreach (IEnemy enemy in this.Model.Enemies)
                        {
                            int mX = (int)(proj.Position.X / this.Model.TileSize);
                            int mY = (int)(proj.Position.Y / this.Model.TileSize);
                            int eX = (int)(enemy.Xpos / this.Model.TileSize);
                            int eY = (int)(enemy.Ypos / this.Model.TileSize);

                            if ((mX == eX && mY == eY) || (Math.Abs(proj.Position.X - enemy.Xpos) <= 40 && Math.Abs(proj.Position.Y - enemy.Ypos) <= 40))
                            {
                                enemy.Health -= proj.Shooter.Damage;
                                if (enemy.Health <= 0)
                                {
                                    this.Model.Money += 200;
                                    this.Model.Score += this.Model.BaseHealth * 10;
                                }

                                tower.Projectiles.Remove(proj);
                            }
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public bool CheckWaveEnded()
        {
            return this.Model.Enemies.Count == 0 && this.SpawnTimers.Count == 0;
        }

        /// <inheritdoc/>
        public bool LoadGame()
        {
            return this.Repo.LoadGame("lastGame.xml");
        }

        /// <inheritdoc/>
        public bool LoadHighscore()
        {
            return this.Repo.LoadHighscores("highScores.xml");
        }

        /// <inheritdoc/>
        public int GetCurrentScore()
        {
            return this.Model.Score;
        }

        /// <inheritdoc/>
        public void MoveEnemies()
        {
            (this.Model.Enemies as List<IEnemy>).RemoveAll(enemy => enemy.Health <= 0);

            for (int i = 0; i < this.Model.Enemies.Count; i++)
            {
                IEnemy enemy = this.Model.Enemies[i];

                double translatedX = enemy.Xpos + (this.Model.TileSize / 2);
                double translatedY = enemy.Ypos + (this.Model.TileSize / 2);

                int xMapPos = (int)(translatedX / this.Model.TileSize);
                int yMapPos = (int)(translatedY / this.Model.TileSize);

                int dX = 0;
                int dY = 0;

                double origX = enemy.Xpos;
                double origY = enemy.Ypos;

                if (enemy.SlowTimer == 0)
                {
                    enemy.SlowTimer = -1;
                }

                bool isStandingOnWater = this.Model.Towers.Count > 0 && this.Model.Towers.Where(t => t.DefenseType == TowerType.Water).Where(t => t.TileX == xMapPos && t.TileY == yMapPos).FirstOrDefault() != null;
                if (isStandingOnWater && enemy.SlowTimer == -1)
                {
                    enemy.SlowTimer = 180;
                }

                switch (this.Model.Map[yMapPos, xMapPos])
                {
                    case Map.ROAD_DOWN:
                        enemy.Ypos += enemy.Speed * ((enemy.SlowTimer > 0) ? 0.25 : 1);
                        dY = 1;
                        break;

                    case Map.ROAD_UP:
                        enemy.Ypos -= enemy.Speed * ((enemy.SlowTimer > 0) ? 0.25 : 1);
                        dY = -1;
                        break;

                    case Map.ROAD_LEFT:
                        enemy.Xpos -= enemy.Speed * ((enemy.SlowTimer > 0) ? 0.25 : 1);
                        dX = -1;
                        break;

                    case Map.ROAD_RIGHT:
                        enemy.Xpos += enemy.Speed * ((enemy.SlowTimer > 0) ? 0.25 : 1);
                        dX = 1;
                        break;

                    default:
                        break;
                }

                translatedX = enemy.Xpos + (this.Model.TileSize / 2);
                translatedY = enemy.Ypos + (this.Model.TileSize / 2);
                xMapPos = (int)(translatedX / this.Model.TileSize);
                yMapPos = (int)(translatedY / this.Model.TileSize);
                int inTileX = (int)(translatedX % this.Model.TileSize);
                int inTileY = (int)(translatedY % this.Model.TileSize);

                try
                {
                    if (this.Model.Map[yMapPos, xMapPos] != this.Model.Map[yMapPos - dY, xMapPos - dX])
                    {
                        switch (this.Model.Map[yMapPos - dY, xMapPos - dX])
                        {
                            case Map.ROAD_DOWN:
                                enemy.Ypos += this.Model.TileSize / 2;
                                break;

                            case Map.ROAD_UP:
                                enemy.Ypos -= this.Model.TileSize / 2;
                                break;

                            case Map.ROAD_LEFT:
                                enemy.Xpos -= this.Model.TileSize / 2;
                                break;

                            case Map.ROAD_RIGHT:
                                enemy.Xpos += this.Model.TileSize / 2;
                                break;

                            default:
                                break;
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                }

                if (xMapPos == this.Model.EndPoint.X && yMapPos == this.Model.EndPoint.Y)
                {
                    this.Model.BaseHealth -= enemy.Damage;
                    enemy.Health = 0;
                }
            }
        }

        /// <inheritdoc/>
        public void PauseOrResumeGame()
        {
            this.Model.IsPaused = !this.Model.IsPaused;
        }

        /// <inheritdoc/>
        public bool SaveGame()
        {
            return this.Repo.SaveGame("proba2.xml");
        }

        /// <inheritdoc/>
        public bool SaveHighscore(string name)
        {
            return this.Repo.SaveHighscores(name);
        }

        /// <inheritdoc/>
        public void SpawnNewWave()
        {
            if (this.CheckWaveEnded())
            {
                this.Model.Wave++;
                int numEnemies = this.Rnd.Next(this.Model.Wave, this.Model.Wave * 3);

                for (int i = 0; i < numEnemies; i++)
                {
                    this.SpawnTimers.Add((i * 100) + this.Rnd.Next(50, 150));
                }

                if (this.Model.Wave == 10)
                {
                    this.SpawnEnemy(true);
                }
            }

            for (int i = 0; i < this.SpawnTimers.Count; i++)
            {
                if (--this.SpawnTimers[i] <= 0)
                {
                    this.SpawnEnemy(false);
                }
            }

            this.SpawnTimers.RemoveAll(i => i <= 0);
        }

        /// <inheritdoc/>
        public void Update()
        {
            this.SpawnNewWave();
            this.TickTimers();
            this.AttackWithTowers();
            this.MoveEnemies();
        }

        /// <inheritdoc/>
        public IList<PlayerStats> GetHighScores()
        {
            return this.Repo.HighScores;
        }

        /// <inheritdoc/>
        public IDictionary<EnemyType, IEnemy> GetAvailableEnemies()
        {
            return this.Repo.LoadEnemies();
        }

        /// <inheritdoc/>
        public IDictionary<TowerType, ITower> GetAvailableTowers()
        {
            return this.Repo.LoadTowers();
        }

        /// <inheritdoc/>
        public void CreateNewGame()
        {
            this.Repo.CreateNewGame();
        }

        /// <inheritdoc/>
        public bool GameEnded()
        {
            return (this.Model.Wave > 10) || (this.Model.BaseHealth <= 0);
        }

        /// <inheritdoc/>
        public bool GameLost()
        {
            return this.Model.BaseHealth <= 0;
        }

        private void SpawnEnemy(bool isNecro)
        {
            int type = this.Rnd.Next(100);
            double xPos = this.Model.StartPoint.X * this.Model.TileSize;
            double yPos = this.Model.StartPoint.Y * this.Model.TileSize;

            Enemy enemy = new Enemy();

            if (isNecro)
            {
                enemy.CopyFrom(this.Model.AvailableEnemies[EnemyType.Necromancer]);
                enemy.Xpos = xPos;
                enemy.Ypos = yPos;

                this.Model.Enemies.Add(enemy);
                return;
            }

            EnemyType zType;

            if (type < 60 || this.Model.Wave <= 2)
            {
                zType = EnemyType.NormalZombie;
            }
            else if (type < 80)
            {
                zType = EnemyType.FastZombie;
            }
            else
            {
                zType = EnemyType.BigZombie;
            }

            enemy.CopyFrom(this.Model.AvailableEnemies[zType]);
            enemy.Xpos = xPos;
            enemy.Ypos = yPos;
            this.Model.Enemies.Add(enemy);
        }

        private bool IsRoad(int x, int y)
        {
            return !(x == this.Model.StartPoint.X && y == this.Model.StartPoint.Y) && (this.Model.Map[y, x] == Map.ROAD_DOWN || this.Model.Map[y, x] == Map.ROAD_UP || this.Model.Map[y, x] == Map.ROAD_LEFT || this.Model.Map[y, x] == Map.ROAD_RIGHT);
        }

        private bool CanPlaceOnThisTile(int x, int y)
        {
            if (this.Model.SelectedTower.DefenseType == TowerType.Water)
            {
                return this.IsRoad(x, y);
            }

            return this.Model.Map[y, x] == Map.BUILDABLE;
        }

        private IEnemy GetClosestEnemy(int x, int y, double range)
        {
            IEnemy enemy = null;
            double distance = double.MaxValue;

            foreach (IEnemy candidate in this.Model.Enemies)
            {
                double candidateDistance = Math.Pow(x - candidate.Xpos, 2) + Math.Pow(y - candidate.Ypos, 2);

                if (candidateDistance < distance && candidateDistance <= range * range)
                {
                    distance = candidateDistance;
                    enemy = candidate;
                }
            }

            return enemy;
        }

        private void TickTimers()
        {
            foreach (ITower tower in this.Model.Towers)
            {
                if (tower.AttackTimer == 0)
                {
                    tower.AttackTimer = tower.AttackInterval;
                }

                tower.AttackTimer--;
            }

            foreach (IEnemy enemy in this.Model.Enemies)
            {
                if (enemy.SlowTimer > 0)
                {
                    enemy.SlowTimer--;
                }
            }
        }

        private void SpawnProjectile(ITower tower)
        {
            int spawnX = (int)((tower.TileX + 0.5) * this.Model.TileSize);
            int spawnY = (int)((tower.TileY + 0.5) * this.Model.TileSize);

            IEnemy closest = this.GetClosestEnemy(spawnX, spawnY, tower.Range);

            if (tower.DefenseType != TowerType.Hunter && this.Model.Enemies.Count == 0)
            {
                tower.Projectiles.Clear();
            }

            switch (tower.DefenseType)
            {
                /* Basic shooting. */
                case TowerType.Archer:
                case TowerType.Cannon:
                case TowerType.Catapult:
                case TowerType.Knight:
                    if (closest != null)
                    {
                        tower.Projectiles.Add(new Projectile(tower, closest, new Point(spawnX, spawnY)));
                    }

                    break;
                /* Attack everyone. */
                case TowerType.Doctor:
                    foreach (IEnemy enemy in this.Model.Enemies)
                    {
                        double dX = enemy.Xpos - spawnX;
                        double dY = enemy.Ypos - spawnY;
                        double distance = Math.Pow(dX, 2) + Math.Pow(dY, 2);

                        if (distance < Math.Pow(tower.Range, 2))
                        {
                            tower.Projectiles.Add(new Projectile(tower, enemy, new Point(spawnX, spawnY)));
                        }
                    }

                    break;
                /* Place traps. */
                case TowerType.Hunter:
                    {
                        Func<Point, bool> isInsideMap = (Point p) => p.X >= 0 && p.X < this.Model.Map.GetLength(1) && p.Y >= 0 && p.Y < this.Model.Map.GetLength(0);
                        Func<Point, bool> isOnRoad = (Point p) => this.IsRoad((int)p.X, (int)p.Y);
                        Func<Point, bool> isNotOccupied = (Point p) =>
                        {
                            foreach (Projectile proj in tower.Projectiles)
                            {
                                if (proj.Position.X == p.X && proj.Position.Y == p.Y)
                                {
                                    return false;
                                }
                            }

                            return true;
                        };

                        List<Point> candidates = Enumerable.Range(-2, 4)
                                            .Select(y => Enumerable.Range(-2, 4)
                                                            .Select(x => new Point(tower.TileX + x, tower.TileY + y))
                                                            .Where(isInsideMap)
                                                            .Where(isOnRoad)
                                                            .Where(isNotOccupied)
                                                            .Select(p => new Point(p.X * this.Model.TileSize, p.Y * this.Model.TileSize)))
                                            .SelectMany(i => i)
                                            .ToList();

                        if (candidates.Count > 0)
                        {
                            Point place = candidates[this.Rnd.Next(candidates.Count)];
                            tower.Projectiles.Add(new Projectile(tower, closest, place));
                        }
                    }

                    break;
                /* Pillar of fire. */
                case TowerType.Wizard:
                    {
                        if (closest == null)
                        {
                            return;
                        }

                        double dX = closest.Xpos - spawnX;
                        double dY = closest.Ypos - spawnY;
                        double distance = Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2)) + 150;
                        double angle = Math.Atan2(dY, dX);

                        int fires = 6;

                        tower.Projectiles.Clear();
                        for (int i = 0; i < fires; i++)
                        {
                            double pX = spawnX - (this.Model.TileSize / 2) + (Math.Cos(angle) * ((distance / fires * i) + this.Model.TileSize));
                            double pY = spawnY - (this.Model.TileSize / 2) + (Math.Sin(angle) * ((distance / fires * i) + this.Model.TileSize));
                            Projectile proj = new Projectile(tower, closest, new Point(pX, pY));
                            proj.Angle = 0;

                            tower.Projectiles.Add(proj);
                        }
                    }

                    break;
                /* Do nothing. */
                case TowerType.Water:
                default:
                    break;
            }
        }
    }
}
