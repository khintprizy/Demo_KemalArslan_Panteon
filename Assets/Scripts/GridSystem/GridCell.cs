using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    private Grid grid;
    private Vector2 cellPosition;
    private EntityController occupant;
    private int cellXIndex;
    private int cellYIndex;

    private int gCost;
    private int hCost;

    private GridCell parentCell;

    List<GridCell> neighbors = new List<GridCell>();

    public int FCost { get { return GCost + HCost; } }

    public int GCost { get => gCost; set => gCost = value; }
    public int HCost { get => hCost; set => hCost = value; }
    public GridCell ParentCell { get => parentCell; set => parentCell = value; }

    public GridCell(Vector2 cellPosition, int cellXIndex, int cellYIndex, Grid grid)
    {
        // constructor of the grid cell

        this.cellPosition = cellPosition;
        this.cellXIndex = cellXIndex;
        this.cellYIndex = cellYIndex;
        this.grid = grid;
    }

    public Vector2 GetCellPosition()
    {
        return cellPosition;
    }

    public Vector2 GetMiddlePointOfTheCell()
    {
        return GetCellPosition() + new Vector2(.16f, .16f);
    }

    public EntityController GetOccupierEntity()
    {
        return occupant;
    }

    public void SetNeighbours(int degree)
    {
        for (int i = -degree; i <= degree; i++)
        {
            for (int j = -degree; j <= degree; j++)
            {
                if (i == 0 && j == 0) continue;

                int checkX = GetCellXIndex() + i;
                int checkY = GetCellYIndex() + j;

                if (checkX >= 0 && checkX < grid.GetGridWidth() && checkY >= 0 && checkY < grid.GetGridHeight())
                    neighbors.Add(grid.GetGridCell(checkX, checkY));
            }
        }
    }

    public List<GridCell> GetNeighbours()
    {
        return neighbors;
    }

    public List<GridCell> GetNeighborsDynamic(int degree)
    {
        List<GridCell> nbrs = new List<GridCell>();

        for (int i = -degree; i <= degree; i++)
        {
            for (int j = -degree; j <= degree; j++)
            {
                if (i == 0 && j == 0) continue;

                int checkX = GetCellXIndex() + i;
                int checkY = GetCellYIndex() + j;

                if (checkX >= 0 && checkX < grid.GetGridWidth() && checkY >= 0 && checkY < grid.GetGridHeight())
                    nbrs.Add(grid.GetGridCell(checkX, checkY));
            }
        }

        return nbrs;
    }

    public bool IsCellOccupied() { return occupant != null; }
    public void SetCellOccupation(EntityController occupant)
    {
        if (occupant == null)
            grid.ChangeOccupiedCellCount(-1);
        else
            grid.ChangeOccupiedCellCount(1);
        this.occupant = occupant;
    }

    public int GetCellXIndex() { return cellXIndex; }
    public int GetCellYIndex() { return cellYIndex; }
}
