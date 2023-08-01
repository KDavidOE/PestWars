// <copyright file="GControl.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Threading;
    using TowerDefense.GameControl.View;
    using TowerDefense.GameLogic;
    using TowerDefense.GameModel;
    using TowerDefense.GameRenderer;
    using TowerDefense.Repository;

    /// <summary>
    /// Control to manage game parts and interactions with it.
    /// </summary>
    public class GControl : FrameworkElement
    {
        private IGameModel model;
        private IStorageRepository repo;
        private IGameLogic logic;
        private Renderer renderer;
        private Stopwatch stw;
        private DispatcherTimer timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GControl"/> class.
        /// </summary>
        public GControl()
        {
            this.Loaded += this.GameControl_Loaded;
        }

        /// <summary>
        /// Event for indicating if an error occured.
        /// </summary>
        public event EventHandler<ErrorOccuredEventArgs> GameCreationErrorEvent;

        /// <summary>
        /// Gets or sets a value indicating whether this game is a new game or it will be loaded.
        /// </summary>
        public bool IsNewGame { get; set; } = true;

        /// <inheritdoc/>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (this.renderer != null)
            {
                if (drawingContext != null)
                {
                    drawingContext.DrawDrawing(this.renderer.BuildDrawing());
                }

                this.renderer.BuildTextDisplay(drawingContext);
            }
        }

        private void GameControl_Loaded(object sender, RoutedEventArgs e)
        {
            double width;
            double height;
            width = 1920;
            height = 1080;
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                width = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive).ActualWidth;
                height = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive).ActualHeight;
            }

            this.model = new GameBaseModel(width, height);
            this.repo = new StorageRepository(this.model);
            this.logic = new MainGameLogic(this.repo);
            this.stw = new Stopwatch();
            if (this.IsNewGame)
            {
                this.repo.CreateNewGame();
            }
            else
            {
                if (!this.logic.LoadGame())
                {
                    ErrorOccuredEventArgs arg = new ErrorOccuredEventArgs("Game load failed, file not found or corrupted!");
                    EventHandler<ErrorOccuredEventArgs> handler = this.GameCreationErrorEvent;
                    if (handler != null)
                    {
                        handler(this, arg);
                    }

                    Window.GetWindow(this).Close();
                    return;
                }
            }

            this.timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000 / this.model.RefreshRate),
            };
            this.timer.Tick += this.Timer_Tick;
            this.timer.Start();

            this.renderer = new Renderer(this.model);
            Window win = Window.GetWindow(this);
            if (win != null)
            {
                win.KeyDown += this.Win_KeyDown;
                win.Closed += this.Win_Closed;
                win.MouseDown += this.Win_MouseDown;
            }

            this.InvalidateVisual();
            this.stw.Start();
        }

        private void Win_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var position = e.GetPosition(this);
            this.logic.AddTower(position);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!this.model.IsPaused && !this.logic.GameLost())
            {
                this.logic.Update();
                this.logic.SpawnNewWave();
                this.InvalidateVisual();
            }

            if (this.logic.GameEnded())
            {
                if (!this.logic.GameLost())
                {
                    Window win = new SaveHighScoreWindow(this.logic);
                    win.ShowDialog();
                    Window.GetWindow(this).DialogResult = true;
                }

                this.stw.Stop();
            }
        }

        private void Win_Closed(object sender, System.EventArgs e)
        {
            this.timer.Tick -= this.Timer_Tick;
        }

        private void Win_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                this.logic.PauseOrResumeGame();
                Window random = new InGameMenuWindow(this.logic);
                random.Activate();
                if (random.ShowDialog().Value == false)
                {
                    Window.GetWindow(this).DialogResult = true;
                }

                this.logic.PauseOrResumeGame();
            }
            else
            {
                if (this.logic.GameLost())
                {
                    Window.GetWindow(this).DialogResult = true;
                }
            }
        }
    }
}
