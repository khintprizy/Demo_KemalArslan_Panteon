using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoldierProductionButton : MonoBehaviour
{
    [Header("BUTTON VIEW ELEMENTS")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI soldierName;
    [SerializeField] private Button button;

    public void Init(SoldierType soldierType)
    {
        SoldierUtilities soldierUtilities = DataManager.Instance.GetSoldierUtilityData(soldierType);

        image.color = soldierUtilities.soldierEntityData.entityColor;
        soldierName.text = soldierUtilities.soldierEntityData.entityName;

        button.onClick?.AddListener(()=> EventManager.CreateSoldier?.Invoke(soldierType));
    }
}
