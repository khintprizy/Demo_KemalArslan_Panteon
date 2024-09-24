using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksController : BuildingBase, IHealth
{
    private BuildingFactory factory;
    private float currentHealth;
    [SerializeField] private float maxHealth;

    [SerializeField] private List<SoldierType> soldierTypes;

    public Action<float> OnHealthChange { get; set; }
    public Action<float, float> OnSetHealth { get; set; }
    public Action OnDie { get; set; }
    public float MaxHealth() => maxHealth;

    public bool IsDead() => isDead;

    

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

        EventManager.OnBuildingWithSoldiersClickedOnTheGrid?.Invoke(this, GetEntityData(), soldierTypes);
        EventManager.OnHealthPanelElementsSet?.Invoke(maxHealth);
    }

    public void Die()
    {
        isDead = true;

        OnDie?.Invoke();

        SetEmptyOccupiedCells();

        factory.SendObjectToPool(gameObject, EntityType.Barracks);
    }

    public void GetDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        OnHealthChange?.Invoke(damageAmount);
        OnSetHealth?.Invoke(maxHealth, currentHealth);

        if (currentHealth <= 0)
            Die();
    }
}
