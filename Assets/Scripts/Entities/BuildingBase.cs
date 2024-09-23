using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : EntityController, IOnCreation, IPickableFromGrid
{
    //public override void ActionWhileMovingWhenCreated(GridCell hoveringCell)
    //{
    //    transform.position = hoveringCell.GetCellPosition();
    //}

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

    //protected override void SetIndicator(Color color)
    //{
    //    OnColorChange?.Invoke(color);
    //}

    //public void PickBuildingFromGrid()
    //{
    //    if (isDead) return;

    //    GridManager.Instance.SetCurrentEntity(this);
    //    SetEmptyOccupiedCells();
    //}

    //void SetIndicator(Color indicatorColor)
    //{
    //    OnColorChange?.Invoke(indicatorColor);
    //}

    //public void SetIndicator(Color indicatorColor)
    //{
    //    OnColorChange?.Invoke(indicatorColor);
    //}
}
