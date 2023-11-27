using System;
using System.Collections.Generic;

namespace TetrisDefence
{
    internal abstract class Tetromino
    {
        public abstract event EventHandler TetrominoDestroyed;
        public abstract List<Block> GetBlocks();
        public abstract bool OnRest();
        public abstract void ShiftX(int offset);
        public abstract void ShiftY();
        public abstract void ShiftY(object sender, EventArgs e);
        public abstract void ClearBool();
        public abstract void FillBool(int dir);
        public abstract void Rotate(int direction);
        public abstract void CheckRest();
        public abstract void UpgradeBlock(int x, int y);
        public abstract void DeleteBlock(object sender, EventArgs e);
        public abstract void Destroy();
    }
}
