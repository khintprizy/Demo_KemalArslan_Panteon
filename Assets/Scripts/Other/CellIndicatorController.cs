using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellIndicatorController : MonoBehaviour
{
    [SerializeField] private CellIndicatorModel cellIndicatorModel;

    public Action<bool> SetRendererActivation;
    public Action<Color> SetRendererColor;
    public Action<Sprite> SetRendererSprite;
    public Action<bool> SetLineRendererActivation;
    public Action<Vector3[]> SetLineRendererPoses;

    private GridManager gridManager;

    private void Start()
    {
        ICellIndicatorInit[] inits = GetComponentsInChildren<ICellIndicatorInit>();
        for (int i = 0; i < inits.Length; i++)
        {
            inits[i]?.OnCellIndicatorInit(this);
        }
        gridManager = GridManager.Instance;

        EventManager.OnCellHoveredWhileSoldierSelected += OnCellChange;
        EventManager.OnSoldierStartsToMove += CloseEverything;
        EventManager.OnEntitySelected += OnEntitySelected;
    }

    private void OnCellChange(GridCell currentCell, GridCell newCell)
    {
        if (newCell == null) return;

        SetRendererActivation?.Invoke(true);
        transform.position = newCell.GetCellPosition();

        if (newCell.IsCellOccupied())
        {
            SetRendererColor?.Invoke(cellIndicatorModel.attackCellColor);
            SetLineRendererActivation?.Invoke(false);
            SetRendererSprite?.Invoke(cellIndicatorModel.attackSprite);
        }
        else
        {
            SetRendererColor?.Invoke(cellIndicatorModel.freeCellColor);
            SetLineRenderer(currentCell, newCell);
            SetRendererSprite?.Invoke(cellIndicatorModel.normalSprite);
        }
    }

    private void SetLineRenderer(GridCell currentCell, GridCell newCell)
    {
        List<GridCell> cells = gridManager.PathFindingManager.FindPath(currentCell, newCell);

        if (cells == null)
        {
            SetRendererColor?.Invoke(Color.red);
            return;
        }

        SetLineRendererActivation?.Invoke(true);

        cells.Insert(0, currentCell);

        Vector3[] poses = new Vector3[cells.Count];

        for (int i = 0; i < poses.Length; i++)
        {
            poses[i] = cells[i].GetMiddlePointOfTheCell();
        }

        SetLineRendererPoses?.Invoke(poses);
    }

    private void CloseEverything()
    {
        SetRendererActivation?.Invoke(false);
        SetLineRendererActivation?.Invoke(false);
    }

    private void OnEntitySelected(EntityController entity)
    {
        CloseEverything();
    }
}

[Serializable]
public struct CellIndicatorModel
{
    public Color freeCellColor;
    public Color attackCellColor;
    public Sprite normalSprite;
    public Sprite attackSprite;
}
