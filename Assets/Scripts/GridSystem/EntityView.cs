using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class EntityView : MonoBehaviour, IEntityInit
{
    [SerializeField] private Transform entityGfx;
    [SerializeField] private SpriteRenderer entitySpriteRenderer;
    [SerializeField] private TextMeshPro entityNameText;
    [SerializeField] private GameObject fakeOutline;


    public void OnEntityInit(EntityController entityController, EntityModelData occupierData)
    {
        entityController.OnColorChange += SetEntityColor;
        entityController.OnEntitySelected += () => SetFakeOutline(true);
        entityController.OnEntityDeselected += () => SetFakeOutline(false);

        entityGfx.localScale = new Vector3(occupierData.entityWidth, occupierData.entityHeight, 1f);
        entitySpriteRenderer.sprite = occupierData.entitySprite;

        SetEntityColor(occupierData.entityColor);
        entityNameText.text = occupierData.entityName;
        entityNameText.transform.localPosition = (new Vector2(occupierData.entityWidth, occupierData.entityHeight)) * GridManager.Instance.GetCellSize() / 2;
    }

    protected void SetEntityColor(Color color)
    {
        entitySpriteRenderer.color = color;
    }

    private void SetFakeOutline(bool isActive)
    {
        fakeOutline.SetActive(isActive);
    }
}
