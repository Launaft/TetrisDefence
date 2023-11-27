using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TetrisDefence
{
    internal class Enemy
    {
        public event EventHandler IsDead;

        private int _x, _y, _hitPoints = 40, _size = 40;
        private double _damage = 0.1;
        private bool _attacking = false, _dead = false;
        private bool[,] _gameGrid;
        private List<Block> _blocks;
        private Random _random = new Random();
        private List<ImageBrush> _images = new List<ImageBrush>()
        {
            new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/blue_slime.png"))),
            new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/brown_slime.png"))),
            new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/green_slime.png"))),
            new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/grey_slime.png"))),
            new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/pink_slime.png"))),
            new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/red_slime.png"))),
            new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/white_slime.png"))),
            new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/yellow_slime.png")))
        };
        private Rectangle _rectangle = new Rectangle();
        public Enemy(ref bool[,] gameGrid, ref List<Block> blocks)
        {
            _x = _random.Next(1, 11);
            _y = _random.Next(1);

            _rectangle.Width = _size;
            _rectangle.Height = _size;
            _rectangle.Fill = _images[_random.Next(7)];

            _blocks = blocks;
            _gameGrid = gameGrid;
        }

        public int GetX()
        {
            return _x;
        }

        public int GetY()
        {
            return _y;
        }

        public Rectangle GetRect()
        {
            return _rectangle;
        }

        public void CheckState(object sender, EventArgs e)
        {
            if ((_attacking && _gameGrid[_y, _x] && _gameGrid[_y + 1, _x]) || _hitPoints == 0)
            {
                _dead = true;
                _attacking = false;

                IsDead?.Invoke(this, null);
            }
            else if (!_dead && _gameGrid[_y + 1, _x] && _gameGrid[_y + 1, _x - 1] && _gameGrid[_y + 1, _x + 1])
            {
                _attacking = true;

                Attack();
            }
            else if (!_dead && (!_gameGrid[_y + 1, _x] || !_gameGrid[_y + 1, _x - 1] || !_gameGrid[_y + 1, _x + 1]))
            {
                _attacking = false;

                Move();
            }

            _hitPoints--;
        }

        private void Move()
        {
            switch (_random.Next(3))
            {
                case 0 when _gameGrid[_y + 1, _x + 1]:
                    switch (_random.Next(1))
                    {
                        case 0 when !_gameGrid[_y + 1, _x - 1]:
                            _x--;
                            _y++;
                            break;
                        case 0 or 1:
                            _y++;
                            break;
                    }
                    break;
                case 0:
                    _x++;
                    _y++;
                    break;
                case 1 when _gameGrid[_y + 1, _x - 1]:
                    switch (_random.Next(1))
                    {
                        case 0 when !_gameGrid[_y + 1, _x + 1]:
                            _x++;
                            _y++;
                            break;
                        case 0 or 1:
                            _y++;
                            break;
                    }
                    break;
                case 1:
                    _x--;
                    _y++;
                    break;
                case 2 when _gameGrid[_y + 1, _x]:
                    switch (_random.Next(1))
                    {
                        case 0 when _gameGrid[_y + 1, _x - 1]:
                            _x++;
                            _y++;
                            break;
                        case 0 or 1:
                            _x--;
                            _y++;
                            break;
                    }
                    break;
                case 2:
                    _y++;
                    break;
            }
            
        }

        public void Attack()
        {
            foreach (Block block in _blocks)
            {
                if (block.GetX() == _x && block.GetY() == _y + 1)
                {
                    block.Damaged(_damage);
                }
            }
        }
    }
}
