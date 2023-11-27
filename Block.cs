using System;
using System.Windows.Shapes;

namespace TetrisDefence
{
    internal abstract class Block
    {
        public abstract event EventHandler IsDestroyed;
        public abstract void SetNewCoordinates(int x, int y);
        public abstract int GetX();
        public abstract int GetY();
        public abstract Rectangle GetRect();
        public abstract void ShiftX(int offset);
        public abstract void ShiftY();
        public abstract void Damaged(double damage);
    }
}
