using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarView : MonoBehaviour, IEntityInit
{
    [SerializeField] private Transform bar;

    public void OnEntityInit(EntityController entityController, EntityModelData entityData)
    {
        IHealth iHealth = entityController.GetComponent<IHealth>();

        if (iHealth != null)
            iHealth.OnHealthChange += OnHealthChange;

    }

    private void OnHealthChange(float maxHealth, float currentHealth, float damageAmount)
    {
        bar.localScale = new Vector3(currentHealth / maxHealth, bar.localScale.y, 1);
    }
}
