using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private List<EntityModelData> buildingDatas;
    [SerializeField] private List<SoldierUtilities> soldierUtilityDatas;
    [SerializeField] private List<BuildingToEntityTypeConverter> buildingTypeConverterList;

    public static DataManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public EntityModelData GetBuildingData(BuildingType buildingType)
    {
        EntityType type = GetEntityTypeFromBuilding(buildingType);

        for (int i = 0; i < buildingDatas.Count; i++)
        {
            if (buildingDatas[i].entityType == type)
                return buildingDatas[i];
        }
        return buildingDatas[0];
    }

    public SoldierUtilities GetSoldierUtilityData(SoldierType soldierType)
    {
        for (int i = 0; i < soldierUtilityDatas.Count; i++)
        {
            if (soldierUtilityDatas[i].soldierType == soldierType)
                return soldierUtilityDatas[i];
        }
        return soldierUtilityDatas[0];
    }

    public EntityType GetEntityTypeFromBuilding(BuildingType buildingType)
    {
        for (int i = 0; i < buildingTypeConverterList.Count; i++)
        {
            if (buildingTypeConverterList[i].buildingType == buildingType)
                return buildingTypeConverterList[i].entityType;
        }
        return buildingTypeConverterList[0].entityType;
    }
}

[Serializable]
public struct BuildingToEntityTypeConverter
{
    public EntityType entityType;
    public BuildingType buildingType;
}
