using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactoryBase : MonoBehaviour
{
    [SerializeField] private List<EntityPoolData> entityPoolDatas = new List<EntityPoolData>();
    [SerializeField] private int instantiateCount;
    protected DataManager dataManager;

    private void Start()
    {
        dataManager = DataManager.Instance;
    }

    public abstract GameObject GetProduct(EntityType entityType);

    protected GameObject CreateNewObject(EntityType entityType)
    {
        GameObject go = Instantiate(GetThePoolData(entityType).entityPrefab);
        GetThePoolData(entityType).entityPoolList.Add(go);
        go.GetComponent<IPooledObjectInstantiated>()?.OnObjectInstantiate();
        go.SetActive(false);
        return go;
    }

    public GameObject GetObjectFromPool(EntityType entityType)
    {
        EntityPoolData poolData = GetThePoolData(entityType);

        if (poolData.entityPoolList.Count < 1)
        {
            for (int i = 0; i < instantiateCount; i++)
            {
                CreateNewObject(entityType);
            }
        }

        GameObject obj = poolData.entityPoolList[0];
        poolData.entityPoolList.Remove(obj);
        obj.GetComponent<IPooledObjectGetFromPool>()?.OnObjectGetFromPool();
        obj.SetActive(true);
        return obj;
    }

    public void SendObjectToPool(GameObject pooledObject, EntityType entityType)
    {
        pooledObject.GetComponent<IPooledObjectSendToPool>()?.OnObjectSendToPool();
        pooledObject.SetActive(false);
        GetThePoolData(entityType).entityPoolList.Add(pooledObject);
    }

    private EntityPoolData GetThePoolData(EntityType entityType)
    {
        for (int i = 0; i < entityPoolDatas.Count; i++)
        {
            if (entityPoolDatas[i].entityType == entityType)
                return entityPoolDatas[i];
        }
        return entityPoolDatas[0];
    }
}

[Serializable]
public class EntityPoolData
{
    public GameObject entityPrefab;
    public EntityType entityType;
    public List<GameObject> entityPoolList = new List<GameObject>();
}


