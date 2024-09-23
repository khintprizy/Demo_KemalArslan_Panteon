using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFactory : FactoryBase
{
    public static SoldierFactory Instance;

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

    public SoldierController GetSoldier(SoldierType soldierType)
    {
        SoldierController soldier = GetProduct(EntityType.Soldier).GetComponent<SoldierController>();
        SoldierUtilities soldierUtilities = dataManager.GetSoldierUtilityData(soldierType);
        soldier.InitSoldierUtilities(soldierUtilities);
        return soldier;
    }
}
