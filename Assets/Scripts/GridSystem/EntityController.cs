using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS CLASS IS THE PARENT CLASS OF ALL THE BUILDINGS AND SOLDIERS ON THE GRID

public abstract class EntityController : MonoBehaviour
{
    public Action<Color> OnColorChange;
    public Action OnEntitySelected;
    public Action OnEntityDeselected;

    private List<GridCell> occupiedCells = new List<GridCell>();

    protected EntityModelData entityModel;

    protected bool isDead;

    public virtual void Init(EntityModelData entityData)
    {
        entityModel = entityData;
        isDead = false;

        IEntityInit[] inits = GetComponentsInChildren<IEntityInit>();
        for (int i = 0; i < inits.Length; i++)
        {
            inits[i].OnEntityInit(this, entityModel);
        }
    }

    public int GetEntityWidth()
    {
        return entityModel.entityWidth;
    }
    public int GetEntityHeight()
    {
        return entityModel.entityHeight;
    }

    public virtual EntityModelData GetEntityData()
    {
        return entityModel;
    }

    // this is used while moving entity on the grid to center to the mouse cursor
    public Vector2 GetMouseOffset()
    {
        return entityModel.GetMouseOffset();
    }

    public virtual void EntityClickedOnBoard()
    {
        OnEntitySelected?.Invoke();
        EventManager.OnEntitySelected?.Invoke(this);
    }

    public virtual void EntityDeselected()
    {
        OnEntityDeselected?.Invoke();
        EventManager.OnEntitySelected?.Invoke(null);
    }

    // Returns true if all the current cells are empty
    public bool CheckIfCanBePlaced(Grid grid, GridCell targetCell)
    {
        bool canBePlaced = true;

        int currentXIndex = targetCell.GetCellXIndex();
        int currentYIndex = targetCell.GetCellYIndex();

        for (int i = 0; i < GetEntityWidth(); i++)
        {
            for (int j = 0; j < GetEntityHeight(); j++)
            {
                GridCell cell = grid.GetGridCell(currentXIndex + i, currentYIndex + j);
                canBePlaced = !cell.IsCellOccupied();

                if (!canBePlaced)
                {
                    SetIndicator(Color.red);
                    return canBePlaced;
                }
            }
        }

        SetIndicator(Color.green);
        return canBePlaced;
    }

    // Occupies the grid for this entity
    public void SetEntityOnTheGrid(Grid grid, GridCell targetCell)
    {
        int x = targetCell.GetCellXIndex();
        int y = targetCell.GetCellYIndex();

        OnColorChange?.Invoke(entityModel.entityColor);

        for (int i = 0; i < GetEntityWidth(); i++)
        {
            for (int j = 0; j < GetEntityHeight(); j++)
            {
                GridCell cell = grid.GetGridCell(x + i, y + j);
                cell.SetCellOccupation(this);
                occupiedCells.Add(cell);
            }
        }
    }

    // When entity is removed from its place, this function clears the cells
    protected void SetEmptyOccupiedCells()
    {
        for (int i = 0; i < occupiedCells.Count; i++)
        {
            occupiedCells[i].SetCellOccupation(null);
        }

        occupiedCells.Clear();
    }

    public GridCell GetFirstOccupiedCell()
    {
        return occupiedCells[0];
    }

    // Returns the list of non occupied cells around the entity, according the degree for its range
    public List<GridCell> GetEmptyNeighbors(int degree, GridCell additionalCell = null)
    {
        List<GridCell> emptyNeigbrs = new List<GridCell>();

        for (int i = 0; i < occupiedCells.Count; i++)
        {
            List<GridCell> neiCells = occupiedCells[i].GetNeighborsDynamic(degree);
            for (int j = 0; j < neiCells.Count; j++)
            {
                GridCell cell = neiCells[j];
                if (!cell.IsCellOccupied() && !emptyNeigbrs.Contains(cell))
                {
                    emptyNeigbrs.Add(cell);
                }

                if (additionalCell != null)
                {
                    if (additionalCell == cell)
                        emptyNeigbrs.Add(cell);
                }
            }
        }

        return emptyNeigbrs;
    }

    // Returns the closest empty cell to the entity
    public GridCell GetClosestEmptyCell(GridCell cell, GridCell soldierCell = null)
    {
        List<GridCell> neighbors = GetEmptyNeighbors(1, soldierCell);

        Vector2 pos = cell.GetCellPosition();
        if (neighbors.Count < 1) return null;

        float closestDist = 1000f;
        GridCell closestCell = null;
        for (int i = 0; i < neighbors.Count; i++)
        {
            float dist = Vector2.Distance(neighbors[i].GetCellPosition(), pos);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestCell = neighbors[i];
            }
        }
        return closestCell;
    }

    public void SetEntityLocation(Grid grid, GridCell targetCell)
    {
        transform.position = targetCell.GetCellPosition();
        SetEntityOnTheGrid(grid, targetCell);
    }

    protected void SetIndicator(Color color)
    {
        OnColorChange?.Invoke(color);
    }
}
