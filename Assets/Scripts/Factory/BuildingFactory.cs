using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFactory : FactoryBase
{
    public static BuildingFactory Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public override GameObject GetProduct(EntityType entityType)
    {
        GameObject go = GetObjectFromPool(entityType);
        return go;
    }

    public BuildingBase GetTheBuilding(BuildingType buildingType)
    {
        EntityType type = dataManager.GetEntityTypeFromBuilding(buildingType);
        BuildingBase buildingBase = GetProduct(type).GetComponent<BuildingBase>();
        buildingBase.Init(dataManager.GetBuildingData(buildingType));
        return buildingBase;
    }
}
