using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float sizeByPixel;

    private Grid grid;
    private GridCell currentCell;
    private GridCell hoveredCell;

    private EntityController currentEntity;
    private EntityController selectedEntity;

    private InputManager inputManager;

    private Pathfinder pathFindingManager;

    public Pathfinder PathFindingManager => pathFindingManager;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        EventManager.OnGameInit += OnGameInit;
    }

    private void Start()
    {
        inputManager = InputManager.Instance;

        EventManager.OnMouseLeftClick += OnLeftMouseClick;
        EventManager.OnMouseRightClick += OnRightMouseClick;

        EventManager.OnEntitySelected += SetSelectedEntity;

        EventManager.CreateSoldier += CreateSoldier;
        EventManager.CreateBuilding += CreateBuilding;
    }

    private void OnGameInit()
    {
        //factoryManager = FactoryManager.Instance;

        grid = new Grid(gridWidth, gridHeight, PixelToWorldSize(sizeByPixel));
        EventManager.OnGridInit?.Invoke(grid);
        pathFindingManager = new Pathfinder(grid);
    }

    private void Update()
    {
        hoveredCell = GetCellAccordingTheMovingCursor(inputManager.MouseWorldPosition);
        OnCurrentCellChange(hoveredCell);

        if (Input.GetKeyDown(KeyCode.B))
        {
            EventManager.CreateBuilding?.Invoke(BuildingType.PowerPlantBuilding);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            EventManager.CreateBuilding?.Invoke(BuildingType.BarracksBuilding);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (selectedEntity != null)
            {
                selectedEntity.GetComponent<IHealth>()?.Die();
            }
        }
    }

    private void OnLeftMouseClick()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        OnClickOnCell(hoveredCell, inputManager.MouseWorldPosition);
    }
    public void OnRightMouseClick()
    {
        if ((selectedEntity != null) && currentCell != null)
        {
            //selectedOccupier.OnRightClicked(currentCell, grid);
            selectedEntity.GetComponent<IRightClickWhileSelected>()?.OnRightClickWhileSelected(currentCell, grid);
        }
    }

    public GridCell GetCellAccordingTheMovingCursor(Vector2 worldPos)
    {
        GridCell cell = null;

        if (currentEntity != null)
            cell = grid.GetGridCell(worldPos + currentEntity.GetMouseOffset(), currentEntity.GetEntityWidth(), currentEntity.GetEntityHeight());
        else
            cell = grid.GetGridCell(worldPos, 1, 1);

        return cell;
    }

    public void OnClickOnCell(GridCell cell, Vector2 worldPos)
    {
        if (currentEntity == null)
        {
            if (cell.IsCellOccupied() && !grid.IsOutOfBounds(worldPos))
            {
                if (selectedEntity != null)
                    selectedEntity.EntityDeselected();

                // This actor could be building or soldier
                cell.GetOccupierEntity().EntityClickedOnBoard();
            }
        }
        else
        {
            TryToPlaceEntity(cell, currentEntity);
        }
    }

    private void TryToPlaceEntity(GridCell gridCell, EntityController occupier)
    {
        if (occupier.CheckIfCanBePlaced(grid, gridCell))
        {
            occupier.SetEntityLocation(grid, gridCell);
            SetCurrentEntity(null);
        }
        // buraya buraya insaa edemezsin gibi bir pop-up gelebilir
    }

    public void OnCurrentCellChange(GridCell cell)
    {
        if (currentCell == cell) return;
        currentCell = cell;

        if (currentEntity != null)
        {
            currentEntity.GetComponent<IOnCreation>()?.OnActionWhileMovingWhenCreated(cell);
            currentEntity.CheckIfCanBePlaced(grid, cell);
        }

        if (selectedEntity != null)
        {
            //selectedOccupier.ActionWhileSelected(cell);
            selectedEntity.GetComponent<IAdditionalActionWhileSelected>()?.OnAdditionalActionWhileSelected(cell);
        }
    }

    public void SetCurrentEntity(EntityController occupier)
    {
        currentEntity = occupier;
    }

    private GridCell GetEmptyCellRecursive(EntityController barracksEntity)
    {
        int degree = 1;

        return GetCell(degree);

        GridCell GetCell(int dgr)
        {
            if (grid.IsGridFull()) return null;

            List<GridCell> emptyCells = selectedEntity.GetEmptyNeighbors(dgr);
            GridCell emptyCell = null;

            // If there are no empty cell of the neighbor of the building, I placed it right by the cell until it finds empty cell
            if (emptyCells.Count < 1)
            {
                degree++;
                return GetCell(degree);
            }
            else
            {
                emptyCell = selectedEntity.GetEmptyNeighbors(dgr)[0];
                return emptyCell;
            }
        }
    }

    public void SetSelectedEntity(EntityController occupier)
    {
        this.selectedEntity = occupier;
    }
    public EntityController GetSelectedEntity()
    {
        return selectedEntity;
    }

    private float PixelToWorldSize(float pixel)
    {
        return pixel / 100;
    }

    public float GetCellSize()
    {
        return PixelToWorldSize(sizeByPixel);
    }

    //public void CreatePowerPlant()
    //{
    //    if (currentEntity != null) return;
    //    if (selectedEntity != null)
    //        selectedEntity.EntityDeselected();

    //    BuildingBase powerPlant = BuildingFactory.Instance.GetProduct().GetComponent<BuildingBase>();
    //    powerPlant.Init(powerPlantData);
    //    SetCurrentEntity(powerPlant);
    //}

    //public void CreateBarracks()
    //{
    //    if (currentEntity != null) return;
    //    if (selectedEntity != null)
    //        selectedEntity.EntityDeselected();

    //    BuildingBase barracks = BarracksFactory.Instance.GetProduct().GetComponent<BuildingBase>();
    //    barracks.Init(barracksData);
    //    SetCurrentEntity(barracks);
    //}

    public void CreateBuilding(BuildingType buildingType)
    {
        if (currentEntity != null) return;
        if (selectedEntity != null)
            selectedEntity.EntityDeselected();

        BuildingBase building = BuildingFactory.Instance.GetTheBuilding(buildingType);
        SetCurrentEntity(building);
    }

    public void CreateSoldier(SoldierType soldierType)
    {
        if (currentEntity != null) return;
        if (selectedEntity == null) return;
        if (selectedEntity.GetComponent<IHealth>().IsDead()) return;

        GridCell emptyCell = GetEmptyCellRecursive(selectedEntity);
        // that means there is no empty place on the grid
        if (emptyCell == null) return;


        SoldierController soldier = SoldierFactory.Instance.GetSoldier(soldierType);
        TryToPlaceEntity(emptyCell, soldier);
    }
}

public static partial class EventManager
{
    public static Action<SoldierType> CreateSoldier { get; set; }
    public static Action<BuildingType> CreateBuilding { get; set; }
}
