using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class EventManager
{
    public static Action OnManagersInit { get; set; }
    public static Action OnGameInit { get; set; }
    public static Action<Grid> OnGridInit { get; set; }

    public static Action<EntityController> OnEntitySelected { get; set; }

    public static Action<BuildingBase, EntityModelData> OnBuildingClickedOnTheGrid { get; set; }
    public static Action<BuildingBase, EntityModelData, List<SoldierType>> OnBuildingWithSoldiersClickedOnTheGrid { get; set; }
    public static Action<float> OnHealthPanelElementsSet { get; set; }
    public static Action OnSoldierSelected { get; set; }

    public static Action OnSoldierStartsToMove { get; set; }
    public static Action<GridCell, GridCell> OnCellHoveredWhileSoldierSelected { get; set; }
}
