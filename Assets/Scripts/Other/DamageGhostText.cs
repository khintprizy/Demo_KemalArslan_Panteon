using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageGhostText : MonoBehaviour, IEntityInit
{
    [SerializeField] private TextMeshPro ghostText;

    private Coroutine ghostCoroutine;

    public void OnEntityInit(EntityController entityController, EntityModelData entityData)
    {
        IHealth iHealth = entityController.GetComponent<IHealth>();
        if (iHealth == null) return;
        iHealth.OnHealthChange += OnGetDamage;
        iHealth.OnDie += OnDie;
    }

    private void OnGetDamage(float damageAmount)
    {
        SetGhostText(damageAmount);
    }

    private void OnDie()
    {
        StopGhostText();
    }

    IEnumerator SetGhostCr(float damageAmount)
    {
        float activeTime = .25f;
        ghostText.gameObject.SetActive(true);
        ghostText.text = "-" + damageAmount;
        yield return new WaitForSeconds(activeTime);
        ghostText.gameObject.SetActive(false);
        ghostCoroutine = null;
    }

    private void SetGhostText(float damageAmount)
    {
        StopGhostText();

        ghostCoroutine = StartCoroutine(SetGhostCr(damageAmount));
    }

    private void StopGhostText()
    {
        if (ghostCoroutine != null)
        {
            StopCoroutine(ghostCoroutine);
            ghostText.gameObject.SetActive(false);
            ghostCoroutine = null;
        }
    }
}
