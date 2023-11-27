﻿using System;
using System.Collections.Generic;

namespace TetrisDefence
{
    internal class TShapeTetromino : Tetromino
    {
        public override event EventHandler TetrominoDestroyed;

        private bool[,] _gameGrid;
        private bool _isOnRest = false, _canRotate = true;
        private int _rotationState;
        private List<Block> _blocks;

        public TShapeTetromino(ref bool[,] gameGrid)
        {
            _blocks = new List<Block>()
            {
                new CommonBlock(4, 1),
                new CommonBlock(5, 1),
                new CommonBlock(5, 0),
                new CommonBlock(6, 1)
            };

            for (int i = 0; i < _blocks.Count; i++)
            {
                _blocks[i].IsDestroyed += DeleteBlock;
            }

            _gameGrid = gameGrid;
        }

        public override List<Block> GetBlocks()
        {
            return _blocks;
        }

        public override bool OnRest()
        {
            return _isOnRest;
        }

        public override void ShiftX(int offset)
        {
            if (!_isOnRest)
            {
                bool canShift = true;

                ClearBool();

                for (int i = 0; i < _blocks.Count; i++)
                {
                    if (_gameGrid[_blocks[i].GetY(), _blocks[i].GetX() + offset])
                    {
                        canShift = false;
                        break;
                    }
                }

                if (canShift)
                {
                    FillBool(offset);
                }
            }

            CheckRest();
        }

        public override void ShiftY()
        {
            if (!_isOnRest)
            {
                ClearBool();
                FillBool(0);
            }

            CheckRest();
        }

        public override void ShiftY(object sender, EventArgs e)
        {
            ShiftY();
        }

        public override void ClearBool()
        {
            foreach (Block block in _blocks)
            {
                _gameGrid[block.GetY(), block.GetX()] = false;
            }
        }

        public override void FillBool(int dir)
        {
            foreach (Block block in _blocks)
            {
                switch (dir)
                {
                    case 0:
                        block.ShiftY();
                        break;
                    case -1 or 1:
                        block.ShiftX(dir);
                        break;
                }
            }
        }

        public override void Rotate(int direction)
        {
            ClearBool();

            if (direction == 2 && !_isOnRest && _canRotate)
            {
                switch (_rotationState)
                {
                    case 0:
                        if (!_gameGrid[_blocks[3].GetY() + 1, _blocks[3].GetX() - 1])
                        {
                            _blocks[0].SetNewCoordinates(_blocks[0].GetX() + 1, _blocks[0].GetY() - 1);
                            _blocks[2].SetNewCoordinates(_blocks[2].GetX() + 1, _blocks[2].GetY() + 1);
                            _blocks[3].SetNewCoordinates(_blocks[3].GetX() - 1, _blocks[3].GetY() + 1);

                            _rotationState++;
                        }
                        break;
                    case 1:
                        if (!_gameGrid[_blocks[3].GetY() - 1, _blocks[3].GetX() - 1])
                        {
                            _blocks[0].SetNewCoordinates(_blocks[0].GetX() + 1, _blocks[0].GetY() + 1);
                            _blocks[2].SetNewCoordinates(_blocks[2].GetX() - 1, _blocks[2].GetY() + 1);
                            _blocks[3].SetNewCoordinates(_blocks[3].GetX() - 1, _blocks[3].GetY() - 1);

                            _rotationState++;
                        }
                        break;
                    case 2:
                        if (!_gameGrid[_blocks[3].GetY() - 1, _blocks[3].GetX() + 1])
                        {
                            _blocks[0].SetNewCoordinates(_blocks[0].GetX() - 1, _blocks[0].GetY() + 1);
                            _blocks[2].SetNewCoordinates(_blocks[2].GetX() - 1, _blocks[2].GetY() - 1);
                            _blocks[3].SetNewCoordinates(_blocks[3].GetX() + 1, _blocks[3].GetY() - 1);

                            _rotationState++;
                        }
                        break;
                    case 3:
                        if (!_gameGrid[_blocks[3].GetY() + 1, _blocks[3].GetX() + 1])
                        {
                            _blocks[0].SetNewCoordinates(_blocks[0].GetX() - 1, _blocks[0].GetY() - 1);
                            _blocks[2].SetNewCoordinates(_blocks[2].GetX() + 1, _blocks[2].GetY() - 1);
                            _blocks[3].SetNewCoordinates(_blocks[3].GetX() + 1, _blocks[3].GetY() + 1);

                            _rotationState = 0;
                        }
                        break;
                }
            }
            else if (direction == -2 && !_isOnRest && _canRotate)
            {
                switch (_rotationState)
                {
                    case 0:
                        if (!_gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX() + 1])
                        {
                            _blocks[0].SetNewCoordinates(_blocks[0].GetX() + 1, _blocks[0].GetY() + 1);
                            _blocks[2].SetNewCoordinates(_blocks[2].GetX() - 1, _blocks[2].GetY() + 1);
                            _blocks[3].SetNewCoordinates(_blocks[3].GetX() - 1, _blocks[3].GetY() - 1);

                            _rotationState = 3;
                        }
                        break;
                    case 3:
                        if (!_gameGrid[_blocks[0].GetY() - 1, _blocks[0].GetX() + 1])
                        {
                            _blocks[0].SetNewCoordinates(_blocks[0].GetX() + 1, _blocks[0].GetY() - 1);
                            _blocks[2].SetNewCoordinates(_blocks[2].GetX() + 1, _blocks[2].GetY() + 1);
                            _blocks[3].SetNewCoordinates(_blocks[3].GetX() - 1, _blocks[3].GetY() + 1);

                            _rotationState--;
                        }
                        break;
                    case 2:
                        if (!_gameGrid[_blocks[0].GetY() - 1, _blocks[0].GetX() - 1])
                        {
                            _blocks[0].SetNewCoordinates(_blocks[0].GetX() - 1, _blocks[0].GetY() - 1);
                            _blocks[2].SetNewCoordinates(_blocks[2].GetX() + 1, _blocks[2].GetY() - 1);
                            _blocks[3].SetNewCoordinates(_blocks[3].GetX() + 1, _blocks[3].GetY() + 1);

                            _rotationState--;
                        }
                        break;
                    case 1:
                        if (!_gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX() - 1])
                        {
                            _blocks[0].SetNewCoordinates(_blocks[0].GetX() - 1, _blocks[0].GetY() + 1);
                            _blocks[2].SetNewCoordinates(_blocks[2].GetX() - 1, _blocks[2].GetY() - 1);
                            _blocks[3].SetNewCoordinates(_blocks[3].GetX() + 1, _blocks[3].GetY() - 1);

                            _rotationState--;
                        }
                        break;
                }
            }

            FillBool(direction);
            CheckRest();
        }

        public override void CheckRest()
        {
            switch (_blocks.Count)
            {
                case 4:
                    switch (_rotationState)
                    {
                        case 0:
                            if (_gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX()] || _gameGrid[_blocks[1].GetY() + 1, _blocks[1].GetX()] || _gameGrid[_blocks[3].GetY() + 1, _blocks[3].GetX()])
                            {
                                _isOnRest = true;
                                _canRotate = false;
                            }
                            else
                                _isOnRest = false;
                            break;
                        case 1:
                            if (_gameGrid[_blocks[2].GetY() + 1, _blocks[2].GetX()] || _gameGrid[_blocks[3].GetY() + 1, _blocks[3].GetX()])
                            {
                                _isOnRest = true;
                                _canRotate = false;
                            }
                            else
                                _isOnRest = false;
                            break;
                        case 2:
                            if (_gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX()] || _gameGrid[_blocks[2].GetY() + 1, _blocks[2].GetX()] || _gameGrid[_blocks[3].GetY() + 1, _blocks[3].GetX()])
                            {
                                _isOnRest = true;
                                _canRotate = false;
                            }
                            else
                                _isOnRest = false;
                            break;
                        case 3:
                            if (_gameGrid[_blocks[2].GetY() + 1, _blocks[2].GetX()] || _gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX()])
                            {
                                _isOnRest = true;
                                _canRotate = false;
                            }
                            else
                                _isOnRest = false;
                            break;
                    }
                    break; 
                case 3:
                    switch (_rotationState)
                    {
                        case 0 when _blocks[0].GetY() > _blocks[1].GetY(): // уничтожен левый блок
                            if (_gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX()] || _gameGrid[_blocks[2].GetY() + 1, _blocks[2].GetX()])
                                _isOnRest = true;
                            else
                                _isOnRest = false;
                            break;
                        case 0 when _blocks[1].GetY() > _blocks[2].GetY(): // уничтожен правый блок
                            if (_gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX()] || _gameGrid[_blocks[1].GetY() + 1, _blocks[1].GetX()])
                                _isOnRest = true;
                            else
                                _isOnRest = false;
                            break;
                        case 0:
                            if (_gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX()] || _gameGrid[_blocks[1].GetY() + 1, _blocks[1].GetX()] || _gameGrid[_blocks[2].GetY() + 1, _blocks[2].GetX()])
                                _isOnRest = true;
                            else
                                _isOnRest = false;
                            break;
                        case 1 when _blocks[0].GetY() == _blocks[1].GetY():
                            if (_gameGrid[_blocks[2].GetY() + 1, _blocks[2].GetX()] || _gameGrid[_blocks[1].GetY() + 1, _blocks[1].GetX()])
                                _isOnRest = true;
                            else
                                _isOnRest = false;
                            break;
                        case 1:
                            if (_gameGrid[_blocks[2].GetY() + 1, _blocks[2].GetX()])
                                _isOnRest = true;
                            else
                                _isOnRest = false;
                            break;
                        case 2 when _blocks[2].GetY() > _blocks[0].GetY(): // уничтожен левый блок
                            if (_gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX()] || _gameGrid[_blocks[2].GetY() + 1, _blocks[2].GetX()])
                                _isOnRest = true;
                            else
                                _isOnRest = false;
                            break;
                        case 2 when _blocks[1].GetY() > _blocks[2].GetY(): // уничтожен правый блок
                            if (_gameGrid[_blocks[1].GetY() + 1, _blocks[1].GetX()] || _gameGrid[_blocks[2].GetY() + 1, _blocks[2].GetX()])
                                _isOnRest = true;
                            else
                                _isOnRest = false;
                            break;
                        case 2:
                            if (_gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX()] || _gameGrid[_blocks[1].GetY() + 1, _blocks[1].GetX()] || _gameGrid[_blocks[2].GetY() + 1, _blocks[2].GetX()])
                                _isOnRest = true;
                            else
                                _isOnRest = false;
                            break;
                        case 3 when _blocks[2].GetY() == _blocks[1].GetY():
                            if (_gameGrid[_blocks[2].GetY() + 1, _blocks[2].GetX()] || _gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX()])
                                _isOnRest = true;
                            else
                                _isOnRest = false;
                            break;
                        case 3:
                            if (_gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX()])
                                _isOnRest = true;
                            else
                                _isOnRest = false;
                            break;
                    }
                    break;
                case 2:
                    switch (_rotationState)
                    {
                        case 0 when _blocks[0].GetY() > _blocks[1].GetY():
                            if (_gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX()])
                                _isOnRest = true;
                            else
                                _isOnRest = false;
                            break;
                        case 1 or 3 when _blocks[0].GetX() == _blocks[1].GetX():
                            if (_gameGrid[_blocks[1].GetY() + 1, _blocks[1].GetX()])
                                _isOnRest = true;
                            else
                                _isOnRest = false;
                            break;
                        case 1 or 3:
                            if (_gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX()] || _gameGrid[_blocks[1].GetY() + 1, _blocks[1].GetX()])
                                _isOnRest = true;
                            else
                                _isOnRest = false;
                            break;
                        case 2 when _blocks[0].GetY() < _blocks[1].GetY():
                            if (_gameGrid[_blocks[1].GetY() + 1, _blocks[1].GetX()])
                                _isOnRest = true;
                            else
                                _isOnRest = false;
                            break;
                        case 0 or 2:
                            if (_gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX()] || _gameGrid[_blocks[1].GetY() + 1, _blocks[1].GetX()])
                                _isOnRest = true;
                            else
                                _isOnRest = false;
                            break;
                    }
                    break;
                case 1:
                    if (_gameGrid[_blocks[0].GetY() + 1, _blocks[0].GetX()])
                        _isOnRest = true;
                    else
                        _isOnRest = false;
                    break;
            }            
        }

        public override void UpgradeBlock(int x, int y)
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                if (_blocks[i].GetX() == x && _blocks[i].GetY() == y)
                {
                    _blocks[i] = new UpgradedBlock(x, y);
                    _blocks[i].IsDestroyed += DeleteBlock;
                }
            }
        }

        public override void DeleteBlock(object sender, EventArgs e)
        {
            if (_blocks.Count > 0)
            {
                _gameGrid[((Block)sender).GetY(), ((Block)sender).GetX()] = false;
                _blocks.Remove((Block)sender);
                sender = null;
            }
            else
            {
                TetrominoDestroyed?.Invoke(this, null);
            }
        }

        public override void Destroy()
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                _blocks[i] = null;
            }

            _blocks.Clear();
            ClearBool();

            TetrominoDestroyed?.Invoke(this, null);
        }
    }
}