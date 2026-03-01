using System;
using System.Collections.Generic;
using MazeProject.DataAccessLayer;

namespace MazeProject.BusinessLogicLayer
{
    public class MazeGenerator
    {
        private Random rand = new Random();

        public void GenerateMazeDFS(Maze maze)
        {
            if (maze == null) return;

            // Reset maze
            maze.ResetMaze();

            // Start from random cell
            int startRow = rand.Next(maze.Rows);
            int startCol = rand.Next(maze.Cols);
            Cell startCell = maze.Grid[startRow, startCol];

            Stack<Cell> stack = new Stack<Cell>();
            List<Cell> visitedCells = new List<Cell>();

            startCell.Visited = true;
            visitedCells.Add(startCell);
            stack.Push(startCell);

            // DFS Algorithm for maze generation
            while (visitedCells.Count < maze.Rows * maze.Cols)
            {
                if (stack.Count == 0)
                    break;

                Cell current = stack.Peek();
                List<Cell> neighbors = GetUnvisitedNeighbors(current, maze);

                if (neighbors.Count > 0)
                {
                    // Choose random neighbor
                    Cell neighbor = neighbors[rand.Next(neighbors.Count)];

                    // Remove wall between current and neighbor
                    RemoveWall(current, neighbor);

                    // Mark neighbor as visited
                    neighbor.Visited = true;
                    visitedCells.Add(neighbor);

                    // Push neighbor to stack
                    stack.Push(neighbor);
                }
                else
                {
                    // Backtrack
                    stack.Pop();
                }
            }
        }

        private List<Cell> GetUnvisitedNeighbors(Cell cell, Maze maze)
        {
            List<Cell> neighbors = new List<Cell>();

            for (int dir = 0; dir < 4; dir++)
            {
                Cell neighbor = maze.GetNeighbor(cell, dir);
                if (neighbor != null && !neighbor.Visited)
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        private void RemoveWall(Cell current, Cell neighbor)
        {
            int rowDiff = neighbor.Row - current.Row;
            int colDiff = neighbor.Col - current.Col;

            if (rowDiff == -1) // Neighbor is above
            {
                current.Walls[0] = false;
                neighbor.Walls[2] = false;
            }
            else if (rowDiff == 1) // Neighbor is below
            {
                current.Walls[2] = false;
                neighbor.Walls[0] = false;
            }
            else if (colDiff == -1) // Neighbor is left
            {
                current.Walls[3] = false;
                neighbor.Walls[1] = false;
            }
            else if (colDiff == 1) // Neighbor is right
            {
                current.Walls[1] = false;
                neighbor.Walls[3] = false;
            }
        }
    }
}