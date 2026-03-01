using System;
using System.Collections.Generic;
using MazeProject.DataAccessLayer;

namespace MazeProject.BusinessLogicLayer
{
    public class MazeSolver
    {
        public List<Cell> SolveMazeBFS(Maze maze)
        {
            if (maze == null || maze.StartCell == null || maze.EndCell == null)
                return new List<Cell>();

            // Reset only solving states
            foreach (var cell in maze.Grid)
            {
                cell.IsExplored = false;
                cell.IsInPath = false;
                cell.Parent = null;
            }

            // BFS Algorithm
            Queue<Cell> queue = new Queue<Cell>();
            maze.StartCell.IsExplored = true;
            queue.Enqueue(maze.StartCell);

            while (queue.Count > 0)
            {
                Cell current = queue.Dequeue();

                // Check if reached end
                if (current == maze.EndCell)
                {
                    return ReconstructPath(current);
                }

                // Check all four directions
                for (int dir = 0; dir < 4; dir++)
                {
                    // If there's no wall in this direction
                    if (!current.Walls[dir])
                    {
                        Cell neighbor = maze.GetNeighbor(current, dir);

                        if (neighbor != null && !neighbor.IsExplored)
                        {
                            neighbor.IsExplored = true;
                            neighbor.Parent = current;
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }

            return new List<Cell>(); // No path found
        }

        private List<Cell> ReconstructPath(Cell endCell)
        {
            List<Cell> path = new List<Cell>();
            Cell current = endCell;

            while (current != null)
            {
                path.Add(current);
                current = current.Parent;
            }

            path.Reverse(); // Reverse to get start→end order
            return path;
        }

        public int CalculatePathLength(List<Cell> path)
        {
            return path.Count;
        }
    }
}