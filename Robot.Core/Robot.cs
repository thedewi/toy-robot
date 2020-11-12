namespace Robot.Core
{
    public enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
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

        public void Place(int posX, int posY, Direction direction)
        {
            _onTable = true;
            _posX = posX;
            _posY = posY;
            _direction = direction;
        }

        public (bool onTable, int posX, int posY, Direction direction) Report()
        {
            return (_onTable, _posX, _posY, _direction);
        }
    }
}