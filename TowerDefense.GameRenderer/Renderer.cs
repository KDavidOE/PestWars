// <copyright file="Renderer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameRenderer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using TowerDefense.GameModel;

    /// <summary>
    /// Class that renders all game objects.
    /// </summary>
    public class Renderer
    {
        private readonly IGameModel model;
        private readonly Typeface font = new Typeface("Arial");
        private readonly Dictionary<string, Brush> myBrushes = new Dictionary<string, Brush>();
        private Drawing oldBackground;
        private Drawing oldshopBackGround;
        private Drawing userFrame;
        private Drawing life;
        private Drawing coin;
        private Drawing baseIcon;
        private Drawing village;
        private Drawing castle;
        private Drawing selectedItemFrame;
        private Drawing towerRange;
        private Drawing obstacles;
        private FormattedText goldText;
        private FormattedText healthText;
        private FormattedText scoreText;
        private FormattedText waveText;
        private FormattedText lostText;
        private int oldwave = -1;
        private int oldMoney = -1;
        private int oldHealth = -1;
        private int oldScore = -1;
        private List<Drawing> buildArea;
        private List<Drawing> oldRoads;
        private List<Drawing> oldShopItems;
        private List<Drawing> oldEnemies;
        private List<Drawing> projectiles;
        private List<Point> oldEnemypositions;
        private List<Drawing> oldTowers;
        private List<Drawing> healtbarBases;
        private List<Drawing> healtbars;
        private Point moneyPos;
        private Point healthPos;
        private Point scorePos;
        private Point wavePos;

        /// <summary>
        /// Initializes a new instance of the <see cref="Renderer"/> class.
        /// </summary>
        /// <param name="model">Current game mode.</param>
        public Renderer(IGameModel model)
        {
            this.model = model;

            this.moneyPos = new Point(
                (this.model.GameWidth * this.model.MapShopRatio) + ((1.0 - this.model.MapShopRatio) * 0.6 * this.model.GameWidth),
                this.model.GameHeight - (this.model.ShopTileHeight * 0.4));

            this.healthPos = new Point(
                (this.model.GameWidth * this.model.MapShopRatio) + ((1.0 - this.model.MapShopRatio) / 5 * this.model.GameWidth),
                this.model.GameHeight - (this.model.ShopTileHeight * 0.4));

            this.wavePos = new Point(
                (this.model.GameWidth * this.model.MapShopRatio) + ((1.0 - this.model.MapShopRatio) * 0.7 * this.model.GameWidth),
                this.model.GameHeight - (this.model.ShopTileHeight * 1.9));

            this.scorePos = new Point(
                (this.model.GameWidth * this.model.MapShopRatio) + ((1.0 - this.model.MapShopRatio) * 0.7 * this.model.GameWidth),
                this.model.GameHeight - this.model.ShopTileHeight);
        }

        private Brush RoadBrush
        {
            get { return this.GetBrush("egyenesut.png", true, this.model.TileSize); }
        }

        private Brush ObstacleBrush
        {
            get { return this.GetBrush("Tree.png", true, this.model.TileSize); }
        }

        private Brush RoadHorizontalBrush
        {
            get { return this.GetBrush("vizszintesut.png", true, this.model.TileSize); }
        }

        private Brush RoadCurveUpBrush
        {
            get { return this.GetBrush("curveupleft.png", true, this.model.TileSize); }
        }

        private Brush RoadCurveDownLeft
        {
            get { return this.GetBrush("curvedownleft.png", true, this.model.TileSize); }
        }

        private Brush RoadCurveDownRight
        {
            get { return this.GetBrush("curvedownright.png", true, this.model.TileSize); }
        }

        private Brush RoadCurveUpRight
        {
            get { return this.GetBrush("curveupright.png", true, this.model.TileSize); }
        }

        private Brush LifeBrush
        {
            get { return this.GetBrush("life.png", false); }
        }

        private Brush CoinBrush
        {
            get { return this.GetBrush("coin.png", false); }
        }

        /// <summary>
        /// Resets drawing variables.
        /// </summary>
        public void Reset()
        {
            this.oldBackground = null;
            this.myBrushes.Clear();
        }

        /// <summary>
        /// Bulder for drawing object which consist game elements.
        /// </summary>
        /// <returns>Returns drawing.</returns>
        public Drawing BuildDrawing()
        {
            DrawingGroup dg = new DrawingGroup();
            try
            {
                dg.Children.Add(this.GetBackground());
                dg.Children.Add(this.GetShopBackground());
                foreach (var item in this.GetRoads())
                {
                    dg.Children.Add(item);
                }

                dg.Children.Add(this.GetVillage());
                dg.Children.Add(this.GetCastle());
                this.oldShopItems = this.GetShopItems();
                foreach (var item in this.oldShopItems)
                {
                    dg.Children.Add(item);
                }

                if (this.GetObstacles() != null)
                {
                    dg.Children.Add(this.GetObstacles());
                }

                dg.Children.Add(this.GetLife());
                dg.Children.Add(this.GetShopBaseIcon());
                dg.Children.Add(this.GetCoin());
                this.GetTowers();
                foreach (var item in this.oldTowers)
                {
                    dg.Children.Add(item);
                }

                this.GetEnemies();
                foreach (var item in this.oldEnemies)
                {
                    dg.Children.Add(item);
                }

                this.GetHealthBar();

                for (int i = 0; i < this.healtbarBases.Count; i++)
                {
                    dg.Children.Add(this.healtbarBases[i]);
                    dg.Children.Add(this.healtbars[i]);
                }

                this.GetProjectiles();

                foreach (var item in this.projectiles)
                {
                    dg.Children.Add(item);
                }

                if (this.GetSelectedShopItemFrame() != null)
                {
                    dg.Children.Add(this.GetSelectedShopItemFrame());
                }

                foreach (var item in this.GetBuildArea())
                {
                    dg.Children.Add(item);
                }

                if (this.GetTowerArea() != null)
                {
                    dg.Children.Add(this.towerRange);
                }

                dg.Children.Add(this.GetUserFrame());
            }
            catch (NullReferenceException)
            {
            }

            return dg;
        }

        /// <summary>
        /// Creates several Formatted text to display game informations.
        /// </summary>
        /// <param name="drawingContext">Context of all drawings.</param>
        public void BuildTextDisplay(DrawingContext drawingContext)
        {
            if (drawingContext != null)
            {
                this.GetMoney(drawingContext);
                this.GetHealth(drawingContext);
                this.GetWave(drawingContext);
                this.GetScore(drawingContext);
                this.GetLostText(drawingContext);
            }
        }

        private static Brush TransformBrush(Brush br)
        {
            BitmapImage newbitmap = (BitmapImage)(br as ImageBrush).ImageSource;
            var tb = new TransformedBitmap();
            tb.BeginInit();
            tb.Source = newbitmap;
            ScaleTransform tr = new ScaleTransform(-1, 1, 0, 0);
            tb.Transform = tr;
            tb.EndInit();
            ImageBrush newbr = new ImageBrush(tb);
            return newbr;
        }

        private static Brush TransformBrushToPoint(Brush br, Projectile p, double size)
        {
            RotateTransform rt = new RotateTransform();
            rt.CenterX = p.Position.X + (size / 2);
            rt.CenterY = p.Position.Y + (size / 2);
            rt.Angle = p.Angle * 180.0 / Math.PI;
            br.Transform = rt;
            return br;
        }

        private Brush GetBrush(string fname, bool isTiled, double tileSize = 50)
        {
            if (!this.myBrushes.ContainsKey(fname))
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("TowerDefense.GameRenderer.Images." + fname);
                bmp.EndInit();
                ImageBrush ib = new ImageBrush(bmp);
                if (isTiled)
                {
                    ib.TileMode = TileMode.Tile;
                    ib.Viewport = new Rect(0, 0, tileSize, tileSize);
                    ib.ViewportUnits = BrushMappingMode.Absolute;
                }

                this.myBrushes[fname] = ib;
            }

            return this.myBrushes[fname];
        }

        private Drawing GetTowerArea()
        {
            if (this.model.EditedTower != null)
            {
                EllipseGeometry towerRange = new EllipseGeometry(
                    new Point(
                    (this.model.EditedTower.TileX * this.model.TileSize) + (this.model.TileSize / 2),
                    (this.model.EditedTower.TileY * this.model.TileSize) + (this.model.TileSize / 2)),
                    this.model.EditedTower.Range,
                    this.model.EditedTower.Range);
                this.towerRange = new GeometryDrawing(null, new Pen(Brushes.White, 2), towerRange);
                return this.towerRange;
            }

            return null;
        }

        private Drawing GetBackground()
        {
            if (this.oldBackground == null)
            {
                Geometry backgroundGeometry = new RectangleGeometry(new Rect(0, 0, this.model.GameWidth * 0.8, this.model.GameHeight));
                this.oldBackground = new GeometryDrawing(Brushes.DarkOliveGreen, null, backgroundGeometry);
            }

            return this.oldBackground;
        }

        private Drawing GetShopBackground()
        {
            if (this.oldshopBackGround == null)
            {
                Geometry backgroundGeometry = new RectangleGeometry(new Rect(this.model.GameWidth * 0.8, 0, this.model.GameWidth * 0.2, this.model.GameHeight));
                this.oldshopBackGround = new GeometryDrawing(Brushes.LightBlue, null, backgroundGeometry);
            }

            return this.oldshopBackGround;
        }

        private bool IsRoad(int x, int y)
        {
            return this.model.Map[y, x] == Map.ROAD_DOWN ||
                   this.model.Map[y, x] == Map.ROAD_UP ||
                   this.model.Map[y, x] == Map.ROAD_LEFT ||
                   this.model.Map[y, x] == Map.ROAD_RIGHT;
        }

        private List<Drawing> GetBuildArea()
        {
            GeometryGroup buildable = new GeometryGroup();
            GeometryGroup notbuildable = new GeometryGroup();
            this.buildArea = new List<Drawing>();
            if (this.model.Map != null && this.model.SelectedTower != null && this.buildArea.Count == 0)
            {
                if (!this.model.SelectedTower.DefenseType.ToString().Equals("Water", StringComparison.Ordinal))
                {
                    for (int y = 0; y < this.model.Map.GetLength(0); y++)
                    {
                        for (int x = 0; x < this.model.Map.GetLength(1); x++)
                        {
                            if (this.model.Map[y, x] != Map.BUILDABLE)
                            {
                                Geometry tileBox = new RectangleGeometry(new Rect(x * this.model.TileSize, y * this.model.TileSize, this.model.TileSize, this.model.TileSize));
                                notbuildable.Children.Add(tileBox);
                            }
                            else if (this.model.Map[y, x] == Map.BUILDABLE)
                            {
                                Geometry tileBox = new RectangleGeometry(new Rect(x * this.model.TileSize, y * this.model.TileSize, this.model.TileSize, this.model.TileSize));
                                buildable.Children.Add(tileBox);
                            }
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < this.model.Map.GetLength(0); y++)
                    {
                        for (int x = 0; x < this.model.Map.GetLength(1); x++)
                        {
                            if (this.IsRoad(x, y) && !(this.model.StartPoint.X == x && this.model.StartPoint.Y == y))
                            {
                                Geometry tileBox = new RectangleGeometry(new Rect(x * this.model.TileSize, y * this.model.TileSize, this.model.TileSize, this.model.TileSize));
                                buildable.Children.Add(tileBox);
                            }
                            else
                            {
                                Geometry tileBox = new RectangleGeometry(new Rect(x * this.model.TileSize, y * this.model.TileSize, this.model.TileSize, this.model.TileSize));
                                notbuildable.Children.Add(tileBox);
                            }
                        }
                    }
                }

                SolidColorBrush redRectBrush = new SolidColorBrush(Colors.Red);
                redRectBrush.Opacity = 0.5; // or whatever

                SolidColorBrush greenRectBrush = new SolidColorBrush(Colors.Green);
                greenRectBrush.Opacity = 0.5; // or whatever
                this.buildArea.Add(new GeometryDrawing(redRectBrush, new Pen(new SolidColorBrush(Colors.Black), 2), notbuildable));
                this.buildArea.Add(new GeometryDrawing(greenRectBrush, new Pen(new SolidColorBrush(Colors.Black), 2), buildable));
            }

            return this.buildArea;
        }

        private List<Drawing> GetRoads()
        {
            if (this.model.Map != null && this.oldRoads == null)
            {
                this.oldRoads = new List<Drawing>();
                GeometryGroup roadvertical = new GeometryGroup();
                GeometryGroup roadhorizontal = new GeometryGroup();
                GeometryGroup roadcurveup = new GeometryGroup();
                GeometryGroup roadcurvedown = new GeometryGroup();
                GeometryGroup rightroadcurveup = new GeometryGroup();
                GeometryGroup rightroadcurvedown = new GeometryGroup();
                for (int y = 0; y < this.model.Map.GetLength(0); y++)
                {
                    for (int x = 0; x < this.model.Map.GetLength(1); x++)
                    {
                        if (this.IsRoad(x, y))
                        {
                            Geometry roadbox = new RectangleGeometry(new Rect(x * this.model.TileSize, y * this.model.TileSize, this.model.TileSize, this.model.TileSize));

                            bool[] directions = new bool[4];

                            if (y - 1 >= 0 && this.IsRoad(x, y - 1))
                            {
                                directions[1] = true;
                            }

                            if (y + 1 < this.model.Map.GetLength(0) - 1 && this.IsRoad(x, y + 1))
                            {
                                directions[3] = true;
                            }

                            if (x - 1 >= 0 && this.IsRoad(x - 1, y))
                            {
                                directions[0] = true;
                            }

                            if (x + 1 < this.model.Map.GetLength(1) - 1 && this.IsRoad(x + 1, y))
                            {
                                directions[2] = true;
                            }

                            if (directions[0] && directions[2])
                            {
                                roadhorizontal.Children.Add(roadbox);
                            }
                            else if (directions[1] && directions[3])
                            {
                                roadvertical.Children.Add(roadbox);
                            }
                            else if (directions[0] && directions[1])
                            {
                                roadcurveup.Children.Add(roadbox);
                            }
                            else if (directions[0] && directions[3])
                            {
                                roadcurvedown.Children.Add(roadbox);
                            }
                            else if (directions[3] && directions[2])
                            {
                                rightroadcurveup.Children.Add(roadbox);
                            }
                            else if (directions[1] && directions[2])
                            {
                                rightroadcurvedown.Children.Add(roadbox);
                            }
                            else if (directions[2] || directions[0])
                            {
                                roadhorizontal.Children.Add(roadbox);
                            }
                            else if (directions[3] || directions[1])
                            {
                                roadvertical.Children.Add(roadbox);
                            }
                        }
                    }
                }

                this.oldRoads.Add(new GeometryDrawing(this.RoadBrush, null, roadvertical));
                this.oldRoads.Add(new GeometryDrawing(this.RoadHorizontalBrush, null, roadhorizontal));
                this.oldRoads.Add(new GeometryDrawing(this.RoadCurveUpBrush, null, roadcurveup));
                this.oldRoads.Add(new GeometryDrawing(this.RoadCurveDownLeft, null, roadcurvedown));
                this.oldRoads.Add(new GeometryDrawing(this.RoadCurveUpRight, null, rightroadcurveup));
                this.oldRoads.Add(new GeometryDrawing(this.RoadCurveDownRight, null, rightroadcurvedown));
            }

            return this.oldRoads;
        }

        private Drawing GetObstacles()
        {
            if (this.model.Map != null && this.obstacles == null)
            {
                GeometryGroup mapobstacles = new GeometryGroup();

                for (int y = 0; y < this.model.Map.GetLength(0); y++)
                {
                    for (int x = 0; x < this.model.Map.GetLength(1); x++)
                    {
                        if (this.model.Map[y, x] == Map.OBSTACLE)
                        {
                            Geometry obs = new RectangleGeometry(new Rect(x * this.model.TileSize, y * this.model.TileSize, this.model.TileSize, this.model.TileSize));
                            mapobstacles.Children.Add(obs);
                        }
                    }
                }

                this.obstacles = new GeometryDrawing(this.ObstacleBrush, null, mapobstacles);
            }

            return this.obstacles;
        }

        private List<Drawing> GetShopItems()
        {
            if (this.oldShopItems == null && this.model.AvailableTowers != null)
            {
                List<Drawing> items = new List<Drawing>();

                int prevlevel = 0;
                int level = 0;
                int i = 0;

                foreach (var item in this.model.AvailableTowers)
                {
                    int x;
                    if (i == 0 || i % 2 == 0)
                    {
                        x = 0;
                    }
                    else
                    {
                        x = 1;
                    }

                    prevlevel++;
                    GeometryGroup g = new GeometryGroup();
                    Geometry box = new RectangleGeometry(new Rect(
                        (this.model.GameWidth * this.model.MapShopRatio) + (x * (this.model.GameWidth * 0.1)),
                        level * this.model.ShopTileHeight,
                        this.model.GameWidth * 0.1,
                        this.model.ShopTileHeight));
                    g.Children.Add(box);

                    Drawing shopitem = new GeometryDrawing(this.GetBrush(item.Value.DefenseType.ToString() + ".png", false), null, g);
                    items.Add(shopitem);
                    if (prevlevel == 2)
                    {
                        level++;
                        prevlevel = 0;
                    }

                    i++;
                }

                return items;
            }

            return this.oldShopItems;
        }

        private Drawing GetSelectedShopItemFrame()
        {
            if (this.model.SelectedTower != null)
            {
                int prevlevel = 0;
                int level = 0;
                for (int i = 0; i < this.model.AvailableTowers.Count; i++)
                {
                    int x;
                    if (i == 0 || i % 2 == 0)
                    {
                        x = 0;
                    }
                    else
                    {
                        x = 1;
                    }

                    prevlevel++;

                    if (this.model.AvailableTowers.Values.ElementAt(i).DefenseType == this.model.SelectedTower.DefenseType)
                    {
                        Geometry box = new RectangleGeometry(new Rect(
                        (this.model.GameWidth * this.model.MapShopRatio) + (x * (this.model.GameWidth * 0.1)),
                        level * this.model.ShopTileHeight,
                        this.model.GameWidth * 0.1,
                        this.model.ShopTileHeight));

                        if (this.model.Money >= this.model.SelectedTower.Price)
                        {
                            this.selectedItemFrame = new GeometryDrawing(this.GetBrush("ShopItemFrame" + ".png", false), null, box);
                        }
                        else
                        {
                            this.selectedItemFrame = new GeometryDrawing(this.GetBrush("ShopItemFrameDenied" + ".png", false), null, box);
                        }

                        return this.selectedItemFrame;
                    }

                    if (prevlevel == 2)
                    {
                        level++;
                        prevlevel = 0;
                    }
                }
            }

            return null;
        }

        private Drawing GetUserFrame()
        {
            Point p1 = new Point(this.model.GameWidth * this.model.MapShopRatio, 0);
            Point p2 = new Point(this.model.GameWidth * this.model.MapShopRatio, this.model.GameHeight);
            Point p3 = new Point((this.model.GameWidth * this.model.MapShopRatio) + (this.model.GameWidth * ((1.0 - this.model.MapShopRatio) / 2)), Math.Ceiling((double)this.model.AvailableTowers.Count / 2) * this.model.ShopTileHeight);
            Point p4 = new Point((this.model.GameWidth * this.model.MapShopRatio) + (this.model.GameWidth * ((1.0 - this.model.MapShopRatio) / 2)), this.model.GameHeight);
            Point p5 = new Point(this.model.GameWidth * this.model.MapShopRatio, Math.Ceiling((double)this.model.AvailableTowers.Count / 2) * this.model.ShopTileHeight);
            Point p6 = new Point(this.model.GameWidth, Math.Ceiling((double)this.model.AvailableTowers.Count / 2) * this.model.ShopTileHeight);
            Point p7 = new Point((this.model.GameWidth * this.model.MapShopRatio) + (this.model.GameWidth * ((1.0 - this.model.MapShopRatio) / 2)), 0);
            Point p8 = new Point((this.model.GameWidth * this.model.MapShopRatio) + (this.model.GameWidth * ((1.0 - this.model.MapShopRatio) / 2)), Math.Ceiling((double)this.model.AvailableTowers.Count / 2) * this.model.ShopTileHeight);
            Point p9 = new Point(this.model.GameWidth * this.model.MapShopRatio, this.model.ShopTileHeight);
            Point p10 = new Point(this.model.GameWidth, this.model.ShopTileHeight);
            Point p11 = new Point(this.model.GameWidth * this.model.MapShopRatio, this.model.ShopTileHeight * 2);
            Point p12 = new Point(this.model.GameWidth, this.model.ShopTileHeight * 2);
            Point p13 = new Point(this.model.GameWidth * this.model.MapShopRatio, this.model.ShopTileHeight * 3);
            Point p14 = new Point(this.model.GameWidth, this.model.ShopTileHeight * 3);

            GeometryGroup g = new GeometryGroup();
            g.Children.Add(new LineGeometry(p1, p2));
            g.Children.Add(new LineGeometry(p3, p4));
            g.Children.Add(new LineGeometry(p5, p6));
            g.Children.Add(new LineGeometry(p7, p8));
            g.Children.Add(new LineGeometry(p9, p10));
            g.Children.Add(new LineGeometry(p11, p12));
            g.Children.Add(new LineGeometry(p13, p14));
            g.GetWidenedPathGeometry(new Pen(Brushes.Black, 10));

            Geometry proba = g;
            this.userFrame = new GeometryDrawing(Brushes.White, new Pen(Brushes.Black, 10), proba.GetFlattenedPathGeometry());
            return this.userFrame;
        }

        private Drawing GetLife()
        {
            if (this.life == null)
            {
                Geometry lifeGeometry = new RectangleGeometry(new Rect(
                    (this.model.GameWidth * this.model.MapShopRatio) + ((1.0 - this.model.MapShopRatio) * 0.3 * this.model.GameWidth),
                    this.model.GameHeight - (this.model.ShopTileHeight / 2),
                    this.model.ShopTileHeight * 0.25,
                    this.model.ShopTileHeight * 0.25));

                this.life = new GeometryDrawing(this.LifeBrush, null, lifeGeometry);
            }

            return this.life;
        }

        private Drawing GetShopBaseIcon()
        {
            if (this.baseIcon == null)
            {
                Geometry baseGeometry = new RectangleGeometry(new Rect(
                    (this.model.GameWidth * this.model.MapShopRatio) + ((1.0 - this.model.MapShopRatio) / 10 * this.model.GameWidth),
                    this.model.GameHeight - (this.model.ShopTileHeight * 1.75),
                    this.model.ShopTileHeight * 0.75,
                    this.model.ShopTileHeight * 0.75));

                this.baseIcon = new GeometryDrawing(this.GetBrush("castle.png", false), null, baseGeometry);
            }

            return this.baseIcon;
        }

        private Drawing GetCoin()
        {
            if (this.coin == null)
            {
                Geometry coinGeometry = new RectangleGeometry(new Rect(
                    (this.model.GameWidth * this.model.MapShopRatio) + ((1.0 - this.model.MapShopRatio) * 0.8 * this.model.GameWidth),
                    this.model.GameHeight - (this.model.ShopTileHeight / 2),
                    this.model.ShopTileHeight * 0.25,
                    this.model.ShopTileHeight * 0.25));

                this.coin = new GeometryDrawing(this.CoinBrush, null, coinGeometry);
            }

            return this.coin;
        }

        private void GetMoney(DrawingContext drawingContext)
        {
            if (this.oldMoney != this.model.Money)
            {
                this.goldText = new FormattedText(this.model.Money.ToString(CultureInfo.CurrentCulture), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, this.font, 21, Brushes.Black, 1.25);
                this.oldMoney = this.model.Money;
            }

            drawingContext.DrawText(this.goldText, this.moneyPos);
        }

        private void GetHealth(DrawingContext drawingContext)
        {
            if (this.oldHealth != this.model.BaseHealth)
            {
                this.healthText = new FormattedText(this.model.BaseHealth.ToString(CultureInfo.CurrentCulture), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, this.font, 21, Brushes.Black, 1.25);
                this.oldHealth = this.model.BaseHealth;
            }

            drawingContext.DrawText(this.healthText, this.healthPos);
        }

        private void GetLostText(DrawingContext drawingContext)
        {
            if (this.model.BaseHealth <= 0)
            {
                string text = "GAME OVER";
                this.lostText = new FormattedText(text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, this.font, this.model.TileSize, Brushes.Black, 1.25);
                drawingContext.DrawText(this.lostText, new Point(this.model.GameWidth / 3, (this.model.GameHeight / 2) - (this.model.GameHeight / 10)));
            }
        }

        private void GetWave(DrawingContext drawingContext)
        {
            if (this.oldwave != this.model.Wave)
            {
                this.waveText = new FormattedText("Wave\n" + this.model.Wave.ToString(CultureInfo.CurrentCulture) + " / 10", System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, this.font, 25, Brushes.Black, 1.25);
                this.oldwave = this.model.Wave;
            }

            drawingContext.DrawText(this.waveText, this.wavePos);
        }

        private void GetScore(DrawingContext drawingContext)
        {
            if (this.oldScore != this.model.Score)
            {
                this.scoreText = new FormattedText("Score\n" + this.model.Score.ToString(CultureInfo.CurrentCulture), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, this.font, 25, Brushes.Black, 1.25);
                this.oldScore = this.model.Score;
            }

            drawingContext.DrawText(this.scoreText, this.scorePos);
        }

        private void GetEnemies()
        {
            bool reinitialize = false;

            if (this.oldEnemies == null && this.model.Enemies != null)
            {
                this.oldEnemies = new List<Drawing>();
                reinitialize = true;
            }
            else if (this.oldEnemies.Count != this.model.Enemies.Count)
            {
                this.oldEnemies.Clear();
                reinitialize = true;
            }

            if (reinitialize)
            {
                if (this.oldEnemypositions == null)
                {
                    this.oldEnemypositions = new List<Point>();
                }
                else
                {
                    this.oldEnemypositions.Clear();
                }

                foreach (var item in this.model.Enemies)
                {
                    this.oldEnemypositions.Add(new Point(item.Xpos, item.Ypos));
                    Geometry enemy = new RectangleGeometry(new Rect(item.Xpos, item.Ypos - (this.model.TileSize / 2), this.model.TileSize, this.model.TileSize));
                    this.oldEnemies.Add(new GeometryDrawing(this.GetBrush(item.ZombieType.ToString() + ".png", false).Clone(), null, enemy));
                }
            }
            else
            {
                for (int i = 0; i < this.model.Enemies.Count; i++)
                {
                    if (this.model.Enemies[i].Xpos != this.oldEnemypositions[i].X || this.model.Enemies[i].Ypos != this.oldEnemypositions[i].Y)
                    {
                        Geometry enemy = new RectangleGeometry(new Rect(this.model.Enemies[i].Xpos, this.model.Enemies[i].Ypos - (this.model.TileSize / 2), this.model.TileSize, this.model.TileSize));
                        if (this.model.Enemies[i].Xpos < this.oldEnemypositions[i].X)
                        {
                            this.oldEnemies[i] = new GeometryDrawing(
                                TransformBrush(this.GetBrush(this.model.Enemies[i].ZombieType.ToString() + ".png", false, 50)).Clone(), null, enemy);
                        }
                        else
                        {
                            this.oldEnemies[i] = new GeometryDrawing(this.GetBrush(this.model.Enemies[i].ZombieType.ToString() + ".png", false).Clone(), null, enemy);
                        }

                        this.oldEnemypositions[i] = new Point(this.model.Enemies[i].Xpos, this.model.Enemies[i].Ypos);
                    }
                }
            }
        }

        private void GetTowers()
        {
            if (this.oldTowers == null && this.model.Towers != null)
            {
                this.oldTowers = new List<Drawing>();
            }
            else if (this.oldTowers.Count != this.model.Towers.Count)
            {
                this.oldTowers.Clear();
            }

            foreach (var tower in this.model.Towers)
            {
                if (!tower.DefenseType.ToString().Equals("Water", StringComparison.Ordinal))
                {
                    Geometry enemy = new RectangleGeometry(new Rect(tower.TileX * this.model.TileSize, tower.TileY * this.model.TileSize, this.model.TileSize, this.model.TileSize));
                    this.oldTowers.Add(new GeometryDrawing(this.GetBrush(tower.DefenseType.ToString() + ".png", false), null, enemy));
                }
                else
                {
                    Geometry enemy = new RectangleGeometry(new Rect(tower.TileX * this.model.TileSize, tower.TileY * this.model.TileSize, this.model.TileSize, this.model.TileSize));
                    this.oldTowers.Add(new GeometryDrawing(this.GetBrush("WaterTile.png", false), null, enemy));
                }
            }
        }

        private Drawing GetVillage()
        {
            if (this.village == null)
            {
                Geometry box = new RectangleGeometry(new Rect(
                    this.model.StartPoint.X * this.model.TileSize,
                    this.model.StartPoint.Y * this.model.TileSize,
                    this.model.TileSize,
                    this.model.TileSize));
                this.village = new GeometryDrawing(this.GetBrush("village.png", false), null, box);
            }

            return this.village;
        }

        private Drawing GetCastle()
        {
            if (this.castle == null)
            {
                Geometry box = new RectangleGeometry(new Rect(
                    this.model.EndPoint.X * this.model.TileSize,
                    this.model.EndPoint.Y * this.model.TileSize,
                    this.model.TileSize,
                    this.model.TileSize));
                this.castle = new GeometryDrawing(this.GetBrush("castle.png", false), null, box);
            }

            return this.castle;
        }

        private void GetHealthBar()
        {
            if (this.healtbars == null && this.healtbarBases == null)
            {
                this.healtbarBases = new List<Drawing>();
                this.healtbars = new List<Drawing>();
            }

            this.healtbars.Clear();
            this.healtbarBases.Clear();

            foreach (var item in this.model.Enemies)
            {
                if (item.Health > 0)
                {
                    Geometry healthbarbases = new RectangleGeometry(new Rect(item.Xpos + (this.model.TileSize / 4), item.Ypos + (this.model.TileSize / 2), this.model.TileSize / 2, this.model.TileSize / 10));
                    this.healtbarBases.Add(new GeometryDrawing(Brushes.Black, null, healthbarbases));

                    Geometry healthbars = new RectangleGeometry(new Rect(item.Xpos + (this.model.TileSize / 4), item.Ypos + (this.model.TileSize / 2), this.model.TileSize / 2 * ((double)item.Health / item.MaxHealth), this.model.TileSize / 10));
                    this.healtbars.Add(new GeometryDrawing(Brushes.Green, null, healthbars));
                }
            }
        }

        private void GetProjectiles()
        {
            if (this.projectiles == null)
            {
                this.projectiles = new List<Drawing>();
            }
            else
            {
                this.projectiles.Clear();
            }

            foreach (var tower in this.model.Towers)
            {
                string shooterName = tower.DefenseType.ToString();
                foreach (var proj in tower.Projectiles)
                {
                    if (proj.Shooter.DefenseType.ToString() != "Water")
                    {
                        Geometry projectile = new RectangleGeometry(new Rect(proj.Position.X, proj.Position.Y, this.model.TileSize, this.model.TileSize));
                        try
                        {
                            this.projectiles.Add(new GeometryDrawing(TransformBrushToPoint(this.GetBrush(shooterName + "Proj.png", false), (Projectile)proj, this.model.TileSize).Clone(), null, projectile.Clone()));
                        }
                        catch (NullReferenceException)
                        {
                        }
                    }
                }
            }
        }
    }
}
