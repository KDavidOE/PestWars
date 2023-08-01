// <copyright file="StorageRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using TowerDefense.GameModel;

    /// <summary>
    /// Class which implements loading and saving.
    /// </summary>
    public class StorageRepository : IStorageRepository
    {
        private static Random rng = new Random();
        private IGameModel currentModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageRepository"/> class.
        /// </summary>
        /// <param name="model">The base model of the game.</param>
        public StorageRepository(IGameModel model)
        {
            this.currentModel = model;
        }

        /// <inheritdoc/>
        public IGameModel CurrentGame
        {
            get { return this.currentModel; }
            private set { this.currentModel = value; }
        }

        /// <inheritdoc/>
        public IList<PlayerStats> HighScores { get; private set; }

        /// <inheritdoc/>
        public void CreateNewGame()
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                var resourcenames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                Assembly a = Assembly.GetExecutingAssembly();
                Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream($"TowerDefense.Repository.DataXML.map{rng.Next(1, resourcenames.Length - 2 + 1)}.xml");
                doc.Load(s);
                this.LoadEnemies();
                this.LoadTowers();
                var gamewidth = doc.DocumentElement.SelectSingleNode("width").InnerText;
                var gameheight = doc.DocumentElement.SelectSingleNode("height").InnerText;
                string[] startposition = doc.DocumentElement.SelectSingleNode("start").InnerText.Split(",");
                Point startpoint = new Point(int.Parse(startposition[1], CultureInfo.InvariantCulture), int.Parse(startposition[0], CultureInfo.InvariantCulture));
                string[] endposition = doc.DocumentElement.SelectSingleNode("end").InnerText.Split(",");
                Point endpoint = new Point(int.Parse(endposition[1], CultureInfo.InvariantCulture), int.Parse(endposition[0], CultureInfo.InvariantCulture));
                this.currentModel.StartPoint = startpoint;
                this.currentModel.EndPoint = endpoint;
                this.currentModel.Map = new Map[int.Parse(gameheight, CultureInfo.InvariantCulture), int.Parse(gamewidth, CultureInfo.InvariantCulture)];
                var rows = doc.DocumentElement.SelectSingleNode("map").ChildNodes;
                for (int y = 0; y < int.Parse(gameheight, CultureInfo.InvariantCulture); y++)
                {
                    var currentRow = rows[y].InnerText.Split(",");
                    for (int x = 0; x < int.Parse(gamewidth, CultureInfo.InvariantCulture); x++)
                    {
                        this.currentModel.Map[y, x] = (Map)Enum.Parse(typeof(Map), currentRow[x]);

                        if (rng.Next(1, 101) < 5 && this.currentModel.Map[y, x] == Map.BUILDABLE)
                        {
                            this.currentModel.Map[y, x] = Map.OBSTACLE;
                        }
                    }
                }

                foreach (var tower in this.currentModel.AvailableTowers)
                {
                    tower.Value.Range = tower.Value.Range * (this.currentModel.GameWidth / 1920.00);
                }
            }
            catch (FileNotFoundException)
            {
            }
        }

        /// <inheritdoc/>
        public Dictionary<EnemyType, IEnemy> LoadEnemies()
        {
            XmlDocument doc = new XmlDocument();
            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream($"TowerDefense.Repository.DataXML.zombies.xml");
            try
            {
                doc.Load(s);
                var enemies = doc.DocumentElement.SelectNodes("zombie");

                for (int i = 0; i < enemies.Count; i++)
                {
                    if (!this.currentModel.AvailableEnemies.ContainsKey((EnemyType)Enum.Parse(typeof(EnemyType), enemies.Item(i).SelectSingleNode("name").InnerText)))
                    {
                        Enemy enemy = new Enemy();
                        enemy.ZombieType = (EnemyType)Enum.Parse(typeof(EnemyType), enemies.Item(i).SelectSingleNode("name").InnerText);
                        enemy.Damage = int.Parse(enemies.Item(i).SelectSingleNode("damage").InnerText, CultureInfo.InvariantCulture);
                        enemy.MaxHealth = int.Parse(enemies.Item(i).SelectSingleNode("health").InnerText, CultureInfo.InvariantCulture);
                        enemy.Speed = int.Parse(enemies.Item(i).SelectSingleNode("speed").InnerText, CultureInfo.InvariantCulture);
                        enemy.Health = enemy.MaxHealth;
                        this.currentModel.AvailableEnemies.Add(enemy.ZombieType, enemy);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                return this.currentModel.AvailableEnemies;
            }

            return this.currentModel.AvailableEnemies;
        }

        /// <inheritdoc/>
        public bool LoadGame(string saveFilePath)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(saveFilePath);
                this.LoadEnemies();
                this.LoadTowers();
                var gamewidth = doc.DocumentElement.SelectSingleNode("width").InnerText;
                var gameheight = doc.DocumentElement.SelectSingleNode("height").InnerText;
                string[] startposition = doc.DocumentElement.SelectSingleNode("start").InnerText.Split(",");
                Point startpoint = new Point(int.Parse(startposition[1], CultureInfo.InvariantCulture), int.Parse(startposition[0], CultureInfo.InvariantCulture));
                string[] endposition = doc.DocumentElement.SelectSingleNode("end").InnerText.Split(",");
                Point endpoint = new Point(int.Parse(endposition[1], CultureInfo.InvariantCulture), int.Parse(endposition[0], CultureInfo.InvariantCulture));
                this.currentModel.StartPoint = startpoint;
                this.currentModel.EndPoint = endpoint;
                this.currentModel.Wave = int.Parse(doc.DocumentElement.SelectSingleNode("wave").InnerText, CultureInfo.InvariantCulture);
                this.currentModel.Money = int.Parse(doc.DocumentElement.SelectSingleNode("gold").InnerText, CultureInfo.InvariantCulture);
                this.currentModel.Score = int.Parse(doc.DocumentElement.SelectSingleNode("score").InnerText, CultureInfo.InvariantCulture);
                this.currentModel.BaseHealth = int.Parse(doc.DocumentElement.SelectSingleNode("health").InnerText, CultureInfo.InvariantCulture);
                this.currentModel.Map = new Map[int.Parse(gameheight, CultureInfo.InvariantCulture), int.Parse(gamewidth, CultureInfo.InvariantCulture)];
                var rows = doc.DocumentElement.SelectSingleNode("map").ChildNodes;
                for (int x = 0; x < int.Parse(gameheight, CultureInfo.InvariantCulture); x++)
                {
                    var currentRow = rows[x].InnerText.Split(",");
                    for (int i = 0; i < int.Parse(gamewidth, CultureInfo.InvariantCulture); i++)
                    {
                        this.currentModel.Map[x, i] = (Map)Enum.Parse(typeof(Map), currentRow[i]);
                    }
                }

                var towers = doc.DocumentElement.SelectSingleNode("towerunit").SelectNodes("tower");
                var enemies = doc.DocumentElement.SelectSingleNode("enemyunit").SelectNodes("enemy");

                for (int i = 0; i < towers.Count; i++)
                {
                    var tower = towers.Item(i);

                    if (tower != null)
                    {
                        Tower newunit = new Tower();
                        newunit.CopyFrom(this.currentModel.AvailableTowers.GetValueOrDefault((TowerType)Enum.Parse(typeof(TowerType), tower.SelectSingleNode("name").InnerText)));
                        string[] tpos = tower.SelectSingleNode("position").InnerText.Split(";");
                        newunit.TileX = int.Parse(tpos[0], CultureInfo.InvariantCulture);
                        newunit.TileY = int.Parse(tpos[1], CultureInfo.InvariantCulture);
                        this.currentModel.Towers.Add(newunit);
                    }
                }

                for (int i = 0; i < enemies.Count; i++)
                {
                    var enemy = enemies.Item(i);

                    if (enemy != null)
                    {
                        Enemy newEnemy = new Enemy();
                        newEnemy.CopyFrom(this.currentModel.AvailableEnemies.GetValueOrDefault((EnemyType)Enum.Parse(typeof(EnemyType), enemy.SelectSingleNode("name").InnerText)));
                        string[] tpos = enemies.Item(i).SelectSingleNode("position").InnerText.Split(";");
                        newEnemy.Xpos = double.Parse(tpos[0], CultureInfo.InvariantCulture) * this.currentModel.TileSize;
                        newEnemy.Ypos = double.Parse(tpos[1], CultureInfo.InvariantCulture) * this.currentModel.TileSize;
                        this.currentModel.Enemies.Add(newEnemy);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public bool LoadHighscores(string highscorePath)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(highscorePath);
                var playerscores = doc.DocumentElement.SelectNodes("player");
                this.HighScores = new List<PlayerStats>();
                for (int i = 0; i < playerscores.Count; i++)
                {
                    string record = playerscores.Item(i).SelectSingleNode("name").InnerText + " " +
                        playerscores.Item(i).SelectSingleNode("score").InnerText;

                    this.HighScores.Add(new PlayerStats(
                        playerscores.Item(i).SelectSingleNode("name").InnerText,
                        playerscores.Item(i).SelectSingleNode("score").InnerText));
                }
            }
            catch (FileNotFoundException)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public Dictionary<TowerType, ITower> LoadTowers()
        {
            XmlDocument doc = new XmlDocument();
            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream($"TowerDefense.Repository.DataXML.towers.xml");
            try
            {
                doc.Load(s);
                var towers = doc.DocumentElement.SelectNodes("tower");

                for (int i = 0; i < towers.Count; i++)
                {
                    if (!this.currentModel.AvailableTowers.ContainsKey((TowerType)Enum.Parse(typeof(TowerType), towers.Item(i).SelectSingleNode("name").InnerText)))
                    {
                        Tower tw = new Tower();
                        tw.DefenseType = (TowerType)Enum.Parse(typeof(TowerType), towers.Item(i).SelectSingleNode("name").InnerText);
                        tw.Damage = int.Parse(towers.Item(i).SelectSingleNode("damage").InnerText, CultureInfo.InvariantCulture);
                        tw.Price = int.Parse(towers.Item(i).SelectSingleNode("price").InnerText, CultureInfo.InvariantCulture);
                        tw.Range = int.Parse(towers.Item(i).SelectSingleNode("range").InnerText, CultureInfo.InvariantCulture);
                        tw.AttackInterval = int.Parse(towers.Item(i).SelectSingleNode("attackspeed").InnerText, CultureInfo.InvariantCulture);
                        this.currentModel.AvailableTowers.Add(tw.DefenseType, tw);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                return this.currentModel.AvailableTowers;
            }

            return this.currentModel.AvailableTowers;
        }

        /// <inheritdoc/>
        public bool SaveGame(string saveFilePath)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                XmlElement element1 = doc.CreateElement("gamedata");
                doc.AppendChild(element1);

                XmlElement mapwidth = doc.CreateElement("width");
                mapwidth.InnerText = this.currentModel.Map.GetLength(1).ToString(CultureInfo.InvariantCulture);
                element1.AppendChild(mapwidth);

                XmlElement mapheight = doc.CreateElement("height");
                mapheight.InnerText = this.currentModel.Map.GetLength(0).ToString(CultureInfo.InvariantCulture);
                element1.AppendChild(mapheight);

                XmlElement start = doc.CreateElement("start");
                start.InnerText = this.currentModel.StartPoint.Y + "," + this.currentModel.StartPoint.X;
                element1.AppendChild(start);

                XmlElement end = doc.CreateElement("end");
                end.InnerText = this.currentModel.EndPoint.Y + "," + this.currentModel.EndPoint.X;
                element1.AppendChild(end);

                XmlElement wavenumber = doc.CreateElement("wave");
                wavenumber.InnerText = this.currentModel.Wave.ToString(CultureInfo.InvariantCulture);
                element1.AppendChild(wavenumber);

                XmlElement basehealth = doc.CreateElement("health");
                basehealth.InnerText = this.currentModel.BaseHealth.ToString(CultureInfo.InvariantCulture);
                element1.AppendChild(basehealth);

                XmlElement gamegold = doc.CreateElement("gold");
                gamegold.InnerText = this.currentModel.Money.ToString(CultureInfo.InvariantCulture);
                element1.AppendChild(gamegold);

                XmlElement gamescore = doc.CreateElement("score");
                gamescore.InnerText = this.currentModel.Score.ToString(CultureInfo.InvariantCulture);
                element1.AppendChild(gamescore);

                XmlElement gameMap = doc.CreateElement("map");
                for (int i = 0; i < this.currentModel.Map.GetLength(0); i++)
                {
                    string row = string.Empty;
                    for (int j = 0; j < this.currentModel.Map.GetLength(1); j++)
                    {
                        if (j == 0)
                        {
                            row += this.currentModel.Map[i, j].ToString();
                        }
                        else
                        {
                            row = row + "," + this.currentModel.Map[i, j].ToString();
                        }
                    }

                    XmlElement mapRow = doc.CreateElement("row");
                    mapRow.InnerText = row;
                    gameMap.AppendChild(mapRow);
                }

                element1.AppendChild(gameMap);
                XmlElement towerUnit = doc.CreateElement("towerunit");

                for (int i = 0; i < this.currentModel.Towers.Count; i++)
                {
                    XmlElement tower = doc.CreateElement("tower");

                    XmlElement name = doc.CreateElement("name");
                    name.InnerText = this.currentModel.Towers[i].DefenseType.ToString();

                    XmlElement range = doc.CreateElement("range");
                    range.InnerText = this.currentModel.Towers[i].Range.ToString(CultureInfo.InvariantCulture);
                    XmlElement damage = doc.CreateElement("damage");
                    damage.InnerText = this.currentModel.Towers[i].Damage.ToString(CultureInfo.InvariantCulture);
                    XmlElement position = doc.CreateElement("position");
                    position.InnerText = this.currentModel.Towers[i].TileX + ";" + this.currentModel.Towers[i].TileY;

                    tower.AppendChild(name);
                    tower.AppendChild(range);
                    tower.AppendChild(damage);
                    tower.AppendChild(position);
                    towerUnit.AppendChild(tower);
                }

                element1.AppendChild(towerUnit);
                XmlElement enemyUnit = doc.CreateElement("enemyunit");

                for (int i = 0; i < this.currentModel.Enemies.Count; i++)
                {
                    XmlElement enemy = doc.CreateElement("enemy");

                    XmlElement name = doc.CreateElement("name");
                    name.InnerText = this.currentModel.Enemies[i].ZombieType.ToString();

                    XmlElement movespeed = doc.CreateElement("movespeed");
                    movespeed.InnerText = this.currentModel.Enemies[i].Speed.ToString(CultureInfo.InvariantCulture);
                    XmlElement health = doc.CreateElement("health");
                    health.InnerText = this.currentModel.Enemies[i].Health.ToString(CultureInfo.InvariantCulture);
                    XmlElement position = doc.CreateElement("position");
                    position.InnerText = Math.Round(this.currentModel.Enemies[i].Xpos / this.currentModel.TileSize, 4).ToString(CultureInfo.InvariantCulture) + ";" + Math.Round(this.currentModel.Enemies[i].Ypos / this.currentModel.TileSize, 4).ToString(CultureInfo.InvariantCulture);

                    enemy.AppendChild(name);
                    enemy.AppendChild(movespeed);
                    enemy.AppendChild(health);
                    enemy.AppendChild(position);
                    enemyUnit.AppendChild(enemy);
                }

                element1.AppendChild(enemyUnit);

                doc.Save("lastGame.xml");
            }
            catch (XmlException)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public bool SaveHighscores(string playerName)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                if (!File.Exists("highScores.xml"))
                {
                    XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);

                    XmlElement root = doc.DocumentElement;
                    doc.InsertBefore(dec, root);

                    XmlElement body = doc.CreateElement("highscore");
                    doc.AppendChild(body);

                    XmlElement player = doc.CreateElement("player");
                    body.AppendChild(player);

                    XmlElement name = doc.CreateElement("name");
                    name.InnerText = playerName;

                    XmlElement score = doc.CreateElement("score");
                    score.InnerText = this.currentModel.Score.ToString(CultureInfo.InvariantCulture);

                    player.AppendChild(name);
                    player.AppendChild(score);

                    doc.Save("highScores.xml");
                }
                else
                {
                    doc.Load("highScores.xml");

                    var start = doc.GetElementsByTagName("highscore");

                    XmlElement playerf = doc.CreateElement("player");
                    start.Item(0).AppendChild(playerf);

                    XmlElement namef = doc.CreateElement("name");
                    namef.InnerText = playerName;

                    XmlElement scoref = doc.CreateElement("score");
                    scoref.InnerText = this.currentModel.Score.ToString(CultureInfo.InvariantCulture);

                    start.Item(0).AppendChild(namef);
                    playerf.AppendChild(namef);
                    playerf.AppendChild(scoref);

                    doc.Save("highScores.xml");
                }
            }
            catch (FileNotFoundException)
            {
                return false;
            }

            return true;
        }
    }
}
