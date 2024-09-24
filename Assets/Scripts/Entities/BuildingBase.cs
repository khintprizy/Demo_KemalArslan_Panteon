using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : EntityController, IOnCreation, IPickableFromGrid
{
    public void OnActionWhileMovingWhenCreated(GridCell hoveringCell)
    {
        transform.position = hoveringCell.GetCellPosition();
    }

    public void PickBuildingFromGrid()
    {
        if (isDead) return;

        GridManager.Instance.SetCurrentEntity(this);
        SetEmptyOccupiedCells();
    }
}
