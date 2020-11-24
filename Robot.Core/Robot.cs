using System;
using System.Collections.Generic;
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
        InvalidDirection,
        Blocked
    }

    public class Robot
    {
        private Direction _direction;
        private bool _onTable;
        private int _posX;
        private int _posY;
        private readonly List<(int posX, int posY)> _blockedPositions = new List<(int posX, int posY)>();

        public Robot(int tableSideLength)
        {
            TableSideLength = tableSideLength;
        }

        public int TableSideLength { get; }

        private PlacementValidity ValidatePositionOnTable(int posX, int posY)
        {
            return posX < 0 || posX >= TableSideLength
                ? PlacementValidity.PosXOutOfRange
                : posY < 0 || posY >= TableSideLength
                    ? PlacementValidity.PosYOutOfRange
                    : PlacementValidity.Valid;
        }

        private PlacementValidity Validate(int posX, int posY, Direction direction)
        {
            var positionValidity = ValidatePositionOnTable(posX, posY);
            return positionValidity == PlacementValidity.Valid
                ? !Enum.GetValues(typeof(Direction)).Cast<Direction>().Contains(direction)
                    ? PlacementValidity.InvalidDirection
                    : _blockedPositions.Contains((posX, posY))
                        ? PlacementValidity.Blocked
                        : PlacementValidity.Valid
                : positionValidity;
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

        public PlacementValidity Move()
        {
            if (!_onTable)
                throw new NotOnTableException();
            switch (_direction)
            {
                case Direction.North:
                    var northValidity = Validate(_posX, _posY + 1, _direction);
                    if (northValidity == PlacementValidity.Valid)
                        _posY++;
                    return northValidity;
                case Direction.East:
                    var eastValidity = Validate(_posX + 1, _posY, _direction);
                    if (eastValidity == PlacementValidity.Valid)
                        _posX++;
                    return eastValidity;
                case Direction.South:
                    var southValidity = Validate(_posX, _posY - 1, _direction);
                    if (southValidity == PlacementValidity.Valid)
                        _posY--;
                    return southValidity;
                case Direction.West:
                    var westValidity = Validate(_posX - 1, _posY, _direction);
                    if (westValidity == PlacementValidity.Valid)
                        _posX--;
                    return westValidity;
                default:
                    return PlacementValidity.InvalidDirection;
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

        public PlacementValidity Block(int posX, int posY)
        {
            var positionValidity = ValidatePositionOnTable(posX, posY);
            if (positionValidity != PlacementValidity.Valid)
                return positionValidity;
            _blockedPositions.Add((posX, posY));
            return PlacementValidity.Valid;
        }

        public (bool onTable, int posX, int posY, Direction direction) Report()
        {
            return (_onTable, _posX, _posY, _direction);
        }
    }
}