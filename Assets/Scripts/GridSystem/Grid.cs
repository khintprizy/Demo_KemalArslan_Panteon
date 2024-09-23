using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int gridHeight;
    private int gridWidth;
    private float cellSize;

    private GridCell[,] cells;

    int occupiedCellCount = 0;

    public Grid(int cellHeight, int cellWidth, float cellSize)
    {
        // constructor of the grid

        this.gridHeight = cellHeight;
        this.gridWidth = cellWidth;
        this.cellSize = cellSize;

        cells = new GridCell[cellWidth, cellHeight];

        for (int i = 0; i < cellWidth; i++)
        {
            for (int j = 0; j < cellHeight; j++)
            {
                GridCell newCell = new GridCell(new Vector2(i, j) * cellSize, i, j, this);
                cells[i, j] = newCell;
            }
        }

        for (int i = 0; i < cellWidth; i++)
        {
            for (int j = 0; j < cellHeight; j++)
            {
                cells[i, j].SetNeighbours(1);
            }
        }
    }

    public GridCell GetGridCell(Vector2 pos, int actorWidth, int actorHeight)
    {
        // herhangi bir loop a gerek kalmadan pozisyonu 2d arrayin indexlerine donusturup cell i cekebiliyoruz

        int widthIndex = Mathf.FloorToInt(pos.x / cellSize);
        int heightIndex = Mathf.FloorToInt(pos.y / cellSize);

        if (widthIndex < 0) widthIndex = 0;
        if (heightIndex < 0) heightIndex = 0;

        if (widthIndex > (gridWidth - actorWidth)) widthIndex = gridWidth - actorWidth;
        if (heightIndex > (gridHeight - actorHeight)) heightIndex = gridHeight - actorHeight;

        return cells[widthIndex, heightIndex];
    }

    public GridCell GetGridCell(int xIndex, int yIndex)
    {
        if ((xIndex < 0) || xIndex > gridWidth) return null;
        if ((yIndex < 0) || yIndex > gridHeight) return null;
        return cells[xIndex, yIndex];
    }

    public Vector2 GetGridCellPosition(Vector2 pos, int actorWidth, int actorHeight)
    {
        return GetGridCell(pos, actorWidth, actorHeight).GetCellPosition();
    }

    public bool IsOutOfBounds(Vector2 pos)
    {
        float maxX = gridWidth * cellSize;
        float maxY = gridHeight * cellSize;

        return (pos.x > maxX || pos.y > maxY || pos.x < 0 || pos.y < 0);
    }

    public void ChangeOccupiedCellCount(int amount)
    {
        occupiedCellCount += amount;
    }

    public bool IsGridFull()
    {
        return (occupiedCellCount >= (cells.Length));
    }

    public int GetOccupiedCellCount()
    {
        return occupiedCellCount;
    }

    public float GetGridCellSize()
    {
        return cellSize;
    }

    public int GetGridWidth() { return gridWidth; }
    public int GetGridHeight() { return gridHeight; }
}
