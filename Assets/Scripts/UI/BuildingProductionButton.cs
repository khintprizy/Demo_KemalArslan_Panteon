using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingProductionButton : MonoBehaviour
{
    [Header("BUTTON VIEW ELEMENTS")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI buildingName;
    [SerializeField] private Button button;

    public void Init(BuildingType buildingType)
    {
        EntityModelData buildingData = DataManager.Instance.GetBuildingData(buildingType);

        image.color = buildingData.entityColor;
        buildingName.text = buildingData.entityName;

        button.onClick?.AddListener(() => EventManager.CreateBuilding?.Invoke(buildingType));
    }
}
