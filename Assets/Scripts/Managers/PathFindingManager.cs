using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    //This system implements A* pathfinding algorithm
    private Grid grid;

    public Pathfinder(Grid grid)
    {
        this.grid = grid;
    }

    public List<GridCell> FindPath(GridCell startCell, GridCell targetCell)
    {
        List<GridCell> openSet = new List<GridCell>();
        HashSet<GridCell> closedSet = new HashSet<GridCell>();
        openSet.Add(startCell);

        while (openSet.Count > 0)
        {
            GridCell currentCell = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentCell.FCost || openSet[i].FCost == currentCell.FCost && openSet[i].HCost < currentCell.HCost)
                {
                    currentCell = openSet[i];
                }
            }

            openSet.Remove(currentCell);
            closedSet.Add(currentCell);

            if (currentCell == targetCell)
            {
                return RetracePath(startCell, targetCell);
            }

            List<GridCell> neighbors = currentCell.GetNeighbours();

            for (int i = 0; i < neighbors.Count; i++)
            {
                GridCell neighborCell = neighbors[i];
                if (neighborCell.IsCellOccupied() || closedSet.Contains(neighborCell))
                    continue;

                int movementCost = currentCell.GCost + GetDistance(currentCell, neighborCell);
                if (movementCost < neighborCell.GCost || !openSet.Contains(neighborCell))
                {
                    neighborCell.GCost = movementCost;
                    neighborCell.HCost = GetDistance(neighborCell, targetCell);
                    neighborCell.ParentCell = currentCell;

                    if (!openSet.Contains(neighborCell))
                        openSet.Add(neighborCell);
                }

            }
        }

        return null;
    }

    private List<GridCell> RetracePath(GridCell startingCell, GridCell endingCell)
    {
        List<GridCell> pathCells = new List<GridCell>();
        GridCell currentCell = endingCell;

        while (currentCell != startingCell)
        {
            pathCells.Add(currentCell);
            currentCell = currentCell.ParentCell;
        }

        pathCells.Reverse();

        return pathCells;
    }

    private int GetDistance(GridCell cellA, GridCell cellB)
    {
        int distanceX = Mathf.Abs(cellA.GetCellXIndex() - cellB.GetCellXIndex());
        int distanceY = Mathf.Abs(cellA.GetCellYIndex() - cellB.GetCellYIndex());

        if (distanceX > distanceY)
            return (14 * distanceY) + (10 * (distanceX - distanceY));
        else
            return (14 * distanceX) + (10 * (distanceY - distanceX));
    }

    public void SetGrid(Grid grid)
    {
        this.grid = grid;
    }
}
