using System;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TetrisDefence
{
    internal class UpgradedBlock : Block
    {
        public override event EventHandler IsDestroyed;

        private int _x, _y, _size = 40;
        private double _hitPoints = 1.0;
        private ImageBrush _image = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/upgraded_block.png")));
        private Rectangle _rectangle = new Rectangle();

        public UpgradedBlock(int x, int y)
        {
            _x = x;
            _y = y;

            _rectangle.Width = _size;
            _rectangle.Height = _size;
            _rectangle.Fill = _image;
            _rectangle.Opacity = _hitPoints;
            _rectangle.StrokeThickness = 0.7;
            _rectangle.Stroke = Brushes.Black;
        }

        public override void SetNewCoordinates(int x, int y)
        {
            _x = x;
            _y = y;
        }
        public override int GetX()
        {
            return _x;
        }
        public override int GetY()
        {
            return _y;
        }
        public override Rectangle GetRect()
        {
            return _rectangle;
        }
        public override void ShiftX(int offset)
        {
            _x += offset;
        }
        public override void ShiftY()
        {
            _y++;
        }
        public override void Damaged(double damage)
        {
            if (_hitPoints > 0.2)
            {
                _hitPoints = _hitPoints - damage / 2;
                _rectangle.Opacity = _hitPoints;
            }
            else
            {
                IsDestroyed?.Invoke(this, null);
            }
        }
    }
}
