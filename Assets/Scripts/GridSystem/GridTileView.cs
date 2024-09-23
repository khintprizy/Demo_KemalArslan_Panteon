using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileView : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    private void Awake()
    {
        EventManager.OnGridInit += SetGridMap;
    }

    public void SetGridMap(Grid grid)
    {
        int gridHeight = grid.GetGridWidth();
        int gridWidth = grid.GetGridWidth();

        meshRenderer.material.mainTextureScale = new Vector2(gridWidth, gridHeight);
        transform.localScale = new Vector3(gridWidth, gridHeight, 1) * grid.GetGridCellSize();
    }
}
