using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : BuildingBase
{
    private BuildingFactory factory;

    public override void Init(EntityModelData entityData)
    {
        base.Init(entityData);

        factory = BuildingFactory.Instance;
    }

    public override void EntityClickedOnBoard()
    {
        base.EntityClickedOnBoard();

        EventManager.OnBuildingClickedOnTheGrid?.Invoke(this, GetEntityData());
    }
}
