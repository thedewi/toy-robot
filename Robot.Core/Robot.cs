using System;
using System.Linq;

namespace Robot.Core
{
    public enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    public enum PlacementValidity
    {
        Valid,
        PosXOutOfRange,
        PosYOutOfRange,
        InvalidDirection
    }

    public class Robot
    {
        private Direction _direction;
        private bool _onTable;
        private int _posX;
        private int _posY;

        public Robot(int tableSideLength)
        {
            TableSideLength = tableSideLength;
        }

        public int TableSideLength { get; }

        private PlacementValidity Validate(int posX, int posY, Direction direction)
        {
            return posX < 0 || posX >= TableSideLength
                ? PlacementValidity.PosXOutOfRange
                : posY < 0 || posY >= TableSideLength
                    ? PlacementValidity.PosYOutOfRange
                    : !Enum.GetValues(typeof(Direction)).Cast<Direction>().Contains(direction)
                        ? PlacementValidity.InvalidDirection
                        : PlacementValidity.Valid;
        }

        public PlacementValidity Place(int posX, int posY, Direction direction)
        {
            var validity = Validate(posX, posY, direction);
            if (validity != PlacementValidity.Valid)
                return validity;

            _onTable = true;
            _posX = posX;
            _posY = posY;
            _direction = direction;
            return validity;
        }

        public bool Move()
        {
            if (!_onTable)
                throw new NotOnTableException();
            switch (_direction)
            {
                case Direction.North when _posY + 1 < TableSideLength:
                    _posY++;
                    return true;
                case Direction.East when _posX + 1 < TableSideLength:
                    _posX++;
                    return true;
                case Direction.South when _posY - 1 >= 0:
                    _posY--;
                    return true;
                case Direction.West when _posX - 1 >= 0:
                    _posX--;
                    return true;
                default:
                    return false;
            }
        }

        public void Left()
        {
            if (!_onTable)
                throw new NotOnTableException();
            _direction = _direction == Direction.North
                ? Direction.West
                : _direction - 1;
        }

        public void Right()
        {
            if (!_onTable)
                throw new NotOnTableException();
            _direction = _direction == Direction.West
                ? Direction.North
                : _direction + 1;
        }

        public (bool onTable, int posX, int posY, Direction direction) Report()
        {
            return (_onTable, _posX, _posY, _direction);
        }
    }
}