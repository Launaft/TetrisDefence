using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace TetrisDefence
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int _waveTime = 20, _surviveTime = 140, _waveCount = 1;
        bool[,] _gameGrid = new bool[21, 12];
        Random _random = new Random();
        List<Tetromino> _tetras = new List<Tetromino>();
        List<Block> _blocks = new List<Block>();
        List<Enemy> _enemies = new List<Enemy>();
        DispatcherTimer _checkTimer = new DispatcherTimer();
        DispatcherTimer _fallingTimer = new DispatcherTimer();
        DispatcherTimer _secondTimer = new DispatcherTimer();
        
        public MainWindow()
        {
            InitializeComponent();

            _checkTimer.Interval = new TimeSpan(1);
            _fallingTimer.Interval = new TimeSpan(0, 0, 0, 0, 800);
            _secondTimer.Interval = new TimeSpan(0, 0, 0, 1);

            _checkTimer.Tick += Check;
            _secondTimer.Tick += UpdateTime;
            

            for (int i = 0; i < _gameGrid.GetLength(0); i++) // боковые границы
            {
                _gameGrid[i, 0] = true;
                _gameGrid[i, _gameGrid.GetLength(1) - 1] = true;
            }

            for (int i = 0; i < _gameGrid.GetLength(1); i++) // нижняя граница
            {
                _gameGrid[_gameGrid.GetLength(0) - 1, i] = true;
            }
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            _checkTimer.Start();
            _fallingTimer.Start();
            _secondTimer.Start();

            waveLabel.Content = "Next Wave in " + _waveTime;
            surviveLabel.Content = "TimeLeft: " + _surviveTime;

            startBtn.IsEnabled = false;
            startBtn.Visibility = Visibility.Hidden;

            helpBtn.IsEnabled = false;
            helpBtn.Visibility = Visibility.Hidden;

            quitBtn.IsEnabled = false;
            quitBtn.Visibility = Visibility.Hidden;

            CreateNextTetromino();

            dungeon.Focus();
        }

        private void helpBtn_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
        }

        private void quitBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void dungeon_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A or Key.Left:
                    Shift(-1);
                    break;
                case Key.D or Key.Right:
                    Shift(1);
                    break;
                case Key.S or Key.Down:
                    Shift(2);
                    break;
                case Key.W or Key.Up:
                    Shift(-2);
                    break;
                case Key.Space:
                    Shift(0);
                    break;
                default:
                    break;
            }

            DrawBlocks();
        }

        private void Shift(int direction)
        {
            foreach (Tetromino tetromino in _tetras)
            {
                switch (direction)
                {
                    case -1 or 1:
                        tetromino.ShiftX(direction);
                        break;
                    case 0:
                        tetromino.CheckRest();
                        tetromino.ShiftY();
                        break;
                    case -2 or 2:
                        tetromino.Rotate(direction);
                        break;
                }
            }
        }

        private void CreateNextTetromino()
        {
            Tetromino tetromino;

            switch (_random.Next(4))
            {
                case 0:
                    tetromino = new OShapeTetromino(ref _gameGrid);
                    SetNewTetromino(tetromino);
                    break;
                case 1:
                    tetromino = new IShapeTetromino(ref _gameGrid);
                    SetNewTetromino(tetromino);
                    break;
                case 2:
                    tetromino = new JShapeTetromino(ref _gameGrid);
                    SetNewTetromino(tetromino);
                    break;
                case 3:
                    tetromino = new LShapeTetromino(ref _gameGrid);
                    SetNewTetromino(tetromino);
                    break;
                case 4:
                    tetromino = new TShapeTetromino(ref _gameGrid);
                    SetNewTetromino(tetromino);
                    break;
            }
        }

        private void SetNewTetromino(Tetromino tetromino)
        {
            _tetras.Add(tetromino);
            _fallingTimer.Tick += tetromino.ShiftY;
            tetromino.TetrominoDestroyed += DeleteTetromino;
        }

        private void DeleteTetromino(object sender, EventArgs e)
        {
            if (sender != null)
            {
                _tetras.Remove((Tetromino)sender);
                sender = null;
            }
        }

        private void DrawBlocks()
        {
            _blocks.Clear();
            dungeon.Children.Clear();

            foreach (Tetromino tetromino in _tetras)
            {
                foreach (Block block in tetromino.GetBlocks())
                {
                    _blocks.Add(block);
                    block.GetRect().Margin = new Thickness(block.GetX() * 40, block.GetY() * 40, 0, 0);
                    dungeon.Children.Add(block.GetRect());
                }
            }
        }

        private void SpawnEnemies()
        {
            for (int i = 0; i < _waveCount * 4; i++)
            {
                if (_enemies.Count < 19)
                {
                    Enemy enemy = new Enemy(ref _gameGrid, ref _blocks);

                    _fallingTimer.Tick += enemy.CheckState;

                    enemy.IsDead += DeleteEnemy;

                    _enemies.Add(enemy);
                    enemy.GetRect().Margin = new Thickness(enemy.GetX() * 40, enemy.GetY() * 40, 0, 0);
                    dungeon.Children.Add(enemy.GetRect());
                }
                else
                    break;
            }
        }

        private void DrawEnemies()
        {
            foreach (Enemy enemy in _enemies)
            {
                enemy.GetRect().Margin = new Thickness(enemy.GetX() * 40, enemy.GetY() * 40, 0, 0);
                dungeon.Children.Remove(enemy.GetRect());
                dungeon.Children.Add(enemy.GetRect());
            }
        }

        private void DeleteEnemy(object sender, EventArgs e)
        {
            if (sender != null)
            {
                dungeon.Children.Remove(((Enemy)sender).GetRect());
                _enemies.Remove((Enemy)sender);
                sender = null;
            }
        }

        private void Check(object sender, EventArgs e)
        {
            bool canCreateTetromino = false;
            bool canUpgradeLine = false;

            DrawBlocks();
            DrawEnemies();

            for (int i = 0; i < _tetras.Count; i++)
            {
                if (_tetras[i].OnRest())
                {
                    for (int j = 0; j < _tetras[i].GetBlocks().Count; j++)
                    {
                        if (_tetras[i].GetBlocks()[j].GetY() > 2)
                        {
                            canCreateTetromino = true;
                        }
                        else
                        {
                            _checkTimer.Stop();
                            Lose(0);
                            break;
                        }
                    }
                }
                else
                {
                    canCreateTetromino = false;
                    break;
                }
            }

            for (int i = 0; i < _gameGrid.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < _gameGrid.GetLength(1) - 1; j++)
                {
                    if (_gameGrid[i, j] && _blocks.Find(block => block.GetY() == i && block.GetX() == j) is CommonBlock)
                    {
                        canUpgradeLine = true;
                    }
                    else
                    {
                        canUpgradeLine = false;
                        break; 
                    }
                }

                if (canUpgradeLine && canCreateTetromino)
                {
                    foreach (Tetromino tetromino in _tetras)
                    {
                        for (int j = 1; j < _gameGrid.GetLength(1) - 1; j++)
                        {
                            tetromino.UpgradeBlock(j, i);
                        }
                    }
                }
            }

            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i].GetY() == 19)
                {
                    _checkTimer.Stop();
                    Lose(1);
                    break;
                }
            }

            if (canCreateTetromino)
                CreateNextTetromino();
        }

        private void UpdateTime(object sender, EventArgs e)
        {
            _waveTime--;
            _surviveTime--;

            waveLabel.Content = "Next Wave in " + _waveTime;
            surviveLabel.Content = "TimeLeft: " + _surviveTime;

            if (_waveTime == 0)
            {
                SpawnEnemies();
                _waveCount++;
                _waveTime = 400 / _blocks.Count * _waveCount;
            }

            if (_surviveTime == 0)
            {
                Win();
            }
        }

        public void Lose(int endNum)
        {
            _checkTimer.Stop();
            _fallingTimer.Stop();
            _secondTimer.Stop();

            switch (endNum)
            {
                case 0:
                    if (MessageBox.Show("You were careless in the construction and immured yourself in the dungeon", "You dead because of careless", MessageBoxButton.OK) == MessageBoxResult.OK)
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();

                        Close();
                    }
                    break;
                case 1:
                    if (MessageBox.Show("You lose, Dungeon Master. Now this dungeon MINE! *evil laugh*", "Dungeon Lost", MessageBoxButton.OK) == MessageBoxResult.OK)
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();

                        Close();
                    }
                    break;
            }

            
        }

        public void Win()
        {
            _checkTimer.Stop();
            _fallingTimer.Stop();
            _secondTimer.Stop();

            if (MessageBox.Show("You won, Dungeon Master. I won't attack your dungeon anymore", "Wizard is going away from your dungeon", MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                Close();
            }
        }
    }
}
