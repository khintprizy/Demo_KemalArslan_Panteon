using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellIndicatorView : MonoBehaviour, ICellIndicatorInit
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private LineRenderer lineRenderer;

    public void OnCellIndicatorInit(CellIndicatorController cellIndicatorController)
    {
        cellIndicatorController.SetRendererActivation += SetRendererActivation;
        cellIndicatorController.SetRendererColor += SetRendererColor;
        cellIndicatorController.SetLineRendererActivation += SetLineRendererActivation;
        cellIndicatorController.SetLineRendererPoses += SetLineRendererPoses;
        cellIndicatorController.SetRendererSprite += SetRendererSprite;
    }

    private void SetLineRendererPoses(Vector3[] poses)
    {
        lineRenderer.positionCount = poses.Length;
        lineRenderer.SetPositions(poses);
    }

    private void SetRendererSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    private void SetLineRendererActivation(bool isActive)
    {
        lineRenderer.gameObject.SetActive(isActive);
    }

    private void SetRendererColor(Color color)
    {
        spriteRenderer.color = color;
    }

    private void SetRendererActivation(bool isActive)
    {
        spriteRenderer.gameObject.SetActive(isActive);
    }
}
