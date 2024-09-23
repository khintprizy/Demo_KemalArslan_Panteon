using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : EntityController, IHealth, IRightClickWhileSelected, IAdditionalActionWhileSelected
{
    private SoldierUtilities soldierUtilities;
    private Coroutine attackCr;

    private EntityController targetEntity;
    private SoldierFactory factory;

    private float currentHealth;

    public Action<float, float, float> OnHealthChange { get; set; }
    public Action OnDie { get; set; }

    public override void Init(EntityModelData entityData)
    {
        base.Init(entityData);
        factory = SoldierFactory.Instance;
        currentHealth = entityData.entityMaxHealth;
        OnHealthChange?.Invoke(GetEntityData().entityMaxHealth, currentHealth, 0);
    }

    public void InitSoldierUtilities(SoldierUtilities soldierUtilities)
    {
        this.soldierUtilities = soldierUtilities;

        Init(soldierUtilities.soldierEntityData);
    }

    private void StartTweenMovement(Grid grid, GridCell targetCell, EntityController targetEntity)
    {
        StartCoroutine(TweenMovementCr(grid, targetCell, targetEntity));
    }

    IEnumerator TweenMovementCr(Grid grid, GridCell targetCell, EntityController targetEntity)
    {
        GridCell startCell = GetFirstOccupiedCell();

        List<GridCell> path = GridManager.Instance.PathFindingManager.FindPath(startCell, targetCell);

        if (path == null)
        {
            EntityDeselected();
            yield break;
        }

        List<GridCell> tempCells = new List<GridCell>();

        tempCells.AddRange(path);

        EventManager.OnSoldierStartsToMove?.Invoke();

        SetEmptyOccupiedCells();

        // With this loop, we move to cell after cell of the path
        // I used Coroutine for a smooth movement
        for (int i = 0; i < path.Count; i++)
        {
            Vector2 start = transform.position;
            Vector2 destination = path[i].GetCellPosition();

            float speed = (destination - start).magnitude;
            float t = 0;
            path[i].SetCellOccupation(this);

            while (t < 1)
            {
                t += ((Time.deltaTime / speed) * soldierUtilities.movementSpeed);
                transform.position = Vector2.Lerp(start, destination, t);
                yield return null;
            }

            path[i].SetCellOccupation(null);

            tempCells.Remove(path[i]);


            if (CheckIfPathIsOccupied(tempCells))
            {
                tempCells.Clear();

                //Eger soldier hareket ederken targetcell e bi sey konulursa soldier duracak ve target a yakin bi yere gitmeye calisacak
                if (targetCell.IsCellOccupied())
                {
                    SetEntityOnTheGrid(grid, path[i]);

                    EntityController entity = targetCell.GetOccupierEntity();
                    GridCell neighborCell = entity.GetClosestEmptyCell(path[i]);

                    StartTweenMovement(grid, neighborCell, null);
                    yield break;
                }

                SetEntityOnTheGrid(grid, path[i]);
                StartTweenMovement(grid, targetCell, targetEntity);
                yield break;
            }
        }

        SetEntityOnTheGrid(grid, targetCell);

        // Eger saldirdigi bir entity varsa burada attack coroutine i baslicak
        if (targetEntity != null)
            StartAttacking(targetEntity);
    }

    private bool CheckIfPathIsOccupied(List<GridCell> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i].IsCellOccupied()) return true;
        }

        return false;
    }

    private void StartAttacking(EntityController entityController)
    {
        this.targetEntity = entityController;

        IHealth iHealth = targetEntity.GetComponent<IHealth>();
        if (iHealth == null) return;

        iHealth.OnDie += StopAttacking;

        attackCr = StartCoroutine(AttackCr(iHealth));
    }

    IEnumerator AttackCr(IHealth targetIHealth)
    {
        if (targetIHealth.IsDead()) yield break;
        targetIHealth?.GetDamage(soldierUtilities.attackPower);

        yield return new WaitForSeconds(soldierUtilities.attackSpeed);
        if (targetIHealth.IsDead()) yield break;
        
        attackCr = StartCoroutine(AttackCr(targetIHealth));
    }

    private void StopAttacking()
    {
        if (attackCr != null)
        {
            StopCoroutine(attackCr);
            attackCr = null;
        }
    }

    public override void EntityClickedOnBoard()
    {
        base.EntityClickedOnBoard();

        EventManager.OnSoldierSelected?.Invoke();
    }

    public void GetDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        OnHealthChange?.Invoke(GetEntityData().entityMaxHealth, currentHealth, damageAmount);

        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        if (targetEntity != null)
        {
            IHealth iHealth = targetEntity.GetComponent<IHealth>();
            if (iHealth != null)
                iHealth.OnDie -= StopAttacking;
        }

        isDead = true;
        OnDie?.Invoke();
        SetEmptyOccupiedCells();
        factory.SendObjectToPool(gameObject, EntityType.Soldier);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void OnRightClickWhileSelected(GridCell cell, Grid grid)
    {
        // cannot attack to yourself
        if (cell == GetFirstOccupiedCell()) return;

        StopAttacking();

        if (cell.IsCellOccupied())
        {
            EntityController targetEntity = cell.GetOccupierEntity();
            GridCell closestCell = targetEntity.GetClosestEmptyCell(cell, GetFirstOccupiedCell());
            if (closestCell == null) return;

            StartTweenMovement(grid, closestCell, targetEntity);
        }
        else
            StartTweenMovement(grid, cell, null);

        EntityDeselected();
    }

    public void OnAdditionalActionWhileSelected(GridCell cell)
    {
        EventManager.OnCellHoveredWhileSoldierSelected?.Invoke(GetFirstOccupiedCell(), cell);
    }
}
