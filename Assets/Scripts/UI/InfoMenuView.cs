using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenuView : MonoBehaviour, IInfoMenuInit
{
    [SerializeField] private TextMeshProUGUI buildingName;
    [SerializeField] private Image buildingImage;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI healthText;

    public void OnInfoMenuInit(InfoMenuController infoMenuController)
    {
        infoMenuController.OnInfoMenuViewChange += SetInfoMenuElements;
    }

    private void SetInfoMenuElements(EntityModelData occupierData)
    {
        buildingName.text = occupierData.entityName;
        buildingImage.sprite = occupierData.entityUISprite;
        description.text = occupierData.entityDescription;
        healthText.text = "Total HP " + occupierData.entityMaxHealth;
    }
}
