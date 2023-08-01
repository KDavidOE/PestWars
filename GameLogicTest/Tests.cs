// <copyright file="Tests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

[assembly: CLSCompliant(false)]

namespace GameLogicTest
{
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using TowerDefense.GameLogic;
    using TowerDefense.GameModel;
    using TowerDefense.Repository;

    /// <summary>
    /// Provides unit tests for this.logic.
    /// </summary>
    public class Tests
    {
        private MainGameLogic logic;
        private Mock<IStorageRepository> repo;
        private Mock<IGameModel> model;

        /// <summary>
        /// Sets up the tests.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.repo = new Mock<IStorageRepository>();
            this.model = new Mock<IGameModel>();

            this.repo.Setup(r => r.CurrentGame).Returns(() => this.model.Object);
            this.logic = new MainGameLogic(this.repo.Object);
        }

        /// <summary>
        /// Tests if loading is properly passed on to the this.repo.
        /// </summary>
        [Test]
        public void CanPlaceTest()
        {
            Map[,] map = new Map[,]
            {
                { Map.BUILDABLE, Map.OBSTACLE },
                { Map.OCCUPIED, Map.ROAD_DOWN },
            };
            this.model.Setup(m => m.Map).Returns(() => map);
            this.model.Setup(m => m.Towers).Returns(() => new List<ITower>());
            this.model.Setup(m => m.TileSize).Returns(32);
            this.model.Setup(m => m.Money).Returns(() => 10000);

            this.model.Setup(m => m.SelectedTower).Returns(() => new Tower { DefenseType = TowerType.Archer });

            Assert.IsTrue(this.logic.AddTower(new System.Windows.Point(0, 0)));

            // Can't place on the same position twice.
            Assert.IsFalse(this.logic.AddTower(new System.Windows.Point(0, 0)));

            // Can't place on an obstacle.
            Assert.IsFalse(this.logic.AddTower(new System.Windows.Point(38, 10)));

            // Can't place on road...
            Assert.IsFalse(this.logic.AddTower(new System.Windows.Point(38, 40)));

            this.model.Setup(m => m.SelectedTower).Returns(() => new Tower { DefenseType = TowerType.Water });

            // Unless it's Water.
            Assert.IsTrue(this.logic.AddTower(new System.Windows.Point(38, 40)));
        }

        /// <summary>
        /// Tests if we correctly detect wins.
        /// </summary>
        [Test]
        public void WinTest()
        {
            var hp = 100;
            var wave = 11;
            this.model.Setup(m => m.BaseHealth).Returns(() => hp);
            this.model.Setup(m => m.Wave).Returns(() => wave);

            Assert.IsTrue(this.logic.GameEnded());
            Assert.IsFalse(this.logic.GameLost());

            hp = 0;
            Assert.IsTrue(this.logic.GameEnded());
            Assert.IsTrue(this.logic.GameLost());

            hp = 100;
            wave = 5;
            Assert.IsFalse(this.logic.GameEnded());
            Assert.IsFalse(this.logic.GameLost());
        }

        /// <summary>
        /// Tests if we correctly detect the game being lost.
        /// </summary>
        [Test]
        public void LossTest()
        {
            var hp = 100;
            this.model.Setup(m => m.BaseHealth).Returns(() => hp);
            Assert.IsFalse(this.logic.GameLost());

            hp = 0;
            Assert.IsTrue(this.logic.GameLost());
        }

        /// <summary>
        /// Tests if the game correctly recognizes that the wave hasn't ended yet.
        /// </summary>
        [Test]
        public void WaveEndedTest()
        {
            var enemyList = new List<IEnemy>()
            {
                new Enemy(),
            };

            this.model.Setup(m => m.Enemies).Returns(enemyList);

            Assert.IsFalse(this.logic.CheckWaveEnded());
        }

        /// <summary>
        /// Tests the shop selector mechanics.
        /// </summary>
        [Test]
        public void ShopSelectTest()
        {
            var towers = new Dictionary<TowerType, ITower>
            {
                { TowerType.Archer, new Tower() { DefenseType = TowerType.Archer } },
                { TowerType.Cannon, new Tower() { DefenseType = TowerType.Cannon } },
                { TowerType.Water, new Tower() { DefenseType = TowerType.Water } },
            };

            this.model.Setup(m => m.AvailableTowers).Returns(towers);
            this.model.Setup(m => m.GameWidth).Returns(800);
            this.model.Setup(m => m.GameHeight).Returns(800);
            this.model.Setup(m => m.MapShopRatio).Returns(0.5);
            this.model.Setup(m => m.ShopTileWidth).Returns(50);
            this.model.Setup(m => m.ShopTileHeight).Returns(50);

            this.model.SetupSet(m => m.SelectedTower = towers[TowerType.Archer]).Verifiable();
            this.logic.SelectTower(new System.Windows.Point(420, 0));

            this.model.SetupSet(m => m.SelectedTower = towers[TowerType.Cannon]).Verifiable();
            this.logic.SelectTower(new System.Windows.Point(500, 0));

            this.model.SetupSet(m => m.SelectedTower = towers[TowerType.Water]).Verifiable();
            this.logic.SelectTower(new System.Windows.Point(600, 0));

            this.model.Verify();
        }
    }
}