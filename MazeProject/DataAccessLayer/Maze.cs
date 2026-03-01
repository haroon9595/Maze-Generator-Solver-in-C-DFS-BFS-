using System;

namespace MazeProject.DataAccessLayer
{
    public class Maze
    {
        public Cell[,] Grid { get; private set; }
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public Cell StartCell { get; set; }
        public Cell EndCell { get; set; }

        public Maze(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Grid = new Cell[rows, cols];

            InitializeGrid();
            SetStartAndEnd();
        }

        private void InitializeGrid()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    Grid[row, col] = new Cell(row, col);
                }
            }
        }

        private void SetStartAndEnd()
        {
            StartCell = Grid[0, 0];
            EndCell = Grid[Rows - 1, Cols - 1];
        }

        public Cell GetNeighbor(Cell cell, int direction)
        {
            int row = cell.Row;
            int col = cell.Col;

            if (direction == 0 && row > 0)
                return Grid[row - 1, col];
            if (direction == 1 && col < Cols - 1)
                return Grid[row, col + 1];
            if (direction == 2 && row < Rows - 1)
                return Grid[row + 1, col];
            if (direction == 3 && col > 0)
                return Grid[row, col - 1];

            return null;
        }

        public void ResetMaze()
        {
            foreach (var cell in Grid)
            {
                cell.Reset();
            }
            SetStartAndEnd();
        }
    }
}