using System;

namespace MazeProject.DataAccessLayer
{
    public class Cell
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public bool Visited { get; set; }
        public bool[] Walls = { true, true, true, true };
        public Cell Parent { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsInPath { get; set; }
        public bool IsExplored { get; set; }

        public Cell(int row, int col)
        {
            Row = row;
            Col = col;
            Visited = false;
            Parent = null;
            IsCurrent = false;
            IsInPath = false;
            IsExplored = false;
        }

        public void Reset()
        {
            Visited = false;
            Walls = new bool[] { true, true, true, true };
            Parent = null;
            IsCurrent = false;
            IsInPath = false;
            IsExplored = false;
        }
    }
}