using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlantController : BuildingBase, IHealth
{
    private BuildingFactory factory;
    private float currentHealth;
    [SerializeField] private float maxHealth;

    public Action<float> OnHealthChange { get; set; }
    public Action<float, float> OnSetHealth { get; set; }
    public Action OnDie { get; set; }
    public bool IsDead() => isDead;
    public float MaxHealth() => maxHealth;

    public override void Init(EntityModelData entityData)
    {
        base.Init(entityData);

        factory = BuildingFactory.Instance;
        currentHealth = maxHealth;
        OnSetHealth?.Invoke(maxHealth, currentHealth);
    }

    public override void EntityClickedOnBoard()
    {
        base.EntityClickedOnBoard();

        EventManager.OnBuildingClickedOnTheGrid?.Invoke(this, GetEntityData());
        EventManager.OnHealthPanelElementsSet?.Invoke(maxHealth);
    }

    public void GetDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        OnHealthChange?.Invoke(damageAmount);
        OnSetHealth?.Invoke(maxHealth, currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        isDead = true;

        OnDie?.Invoke();

        SetEmptyOccupiedCells();

        factory.SendObjectToPool(gameObject, EntityType.PowerPlant);
    }
}
