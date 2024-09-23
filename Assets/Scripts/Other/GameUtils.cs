using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public struct EntityModelData
{
    public int entityWidth;
    public int entityHeight;
    public string entityName;
    public string entityDescription;
    public Color entityColor;
    public Sprite entityUISprite;
    public Sprite entitySprite;
    public float entityMaxHealth;
    public EntityType entityType;

    public Vector2 GetMouseOffset()
    {
        // this offset used while moving building on the grid to center to the mouse cursor

        float x = ((float)entityWidth / 2) - .5f;
        float y = ((float)entityHeight / 2) - .5f;
        return -(GridManager.Instance.GetCellSize()) * (new Vector2(x, y));
    }
}

[Serializable]
public struct SoldierUtilities
{
    public float attackPower;
    public float attackSpeed;
    public float movementSpeed;
    public SoldierType soldierType;
    public EntityModelData soldierEntityData;
}

public enum BuildingType
{
    PowerPlantBuilding = 0,
    BarracksBuilding = 1,
}

public enum SoldierType
{
    Soldier1 = 0,
    Soldier2 = 1,
    Soldier3 = 2,
}

public enum EntityType
{
    PowerPlant = 0,
    Barracks = 1,
    Soldier = 2,
}

public interface IEntityInit
{
    void OnEntityInit(EntityController entityController, EntityModelData entityData);
}

public interface IPooledObjectInstantiated
{
    void OnObjectInstantiate();
}

public interface IPooledObjectGetFromPool
{
    void OnObjectGetFromPool();
}

public interface IPooledObjectSendToPool
{
    void OnObjectSendToPool();
}

public interface IHealth
{
    void GetDamage(float damageAmount);
    void Die();
    bool IsDead();

    Action<float, float, float> OnHealthChange { get; set; }
    Action OnDie { get; set; }
}

public interface IInfoMenuInit
{
    void OnInfoMenuInit(InfoMenuController infoMenuController);
}

public interface IRightClickWhileSelected
{
    void OnRightClickWhileSelected(GridCell cell, Grid grid);
}

public interface IAdditionalActionWhileSelected
{
    void OnAdditionalActionWhileSelected(GridCell cell);
}

public interface IPlacementIndicator
{
    void SetIndicator(Color indicatorColor);
}

public interface IOnCreation
{
    void OnActionWhileMovingWhenCreated(GridCell hoveringCell);
}

public interface IPickableFromGrid
{
    void PickBuildingFromGrid();
}

public interface ICellIndicatorInit
{
    void OnCellIndicatorInit(CellIndicatorController cellIndicatorController);
}

public interface IInfiniteSWInit
{
    void OnInfiniteScrollViewInit(InfiniteSWController infiniteSWController);
}
