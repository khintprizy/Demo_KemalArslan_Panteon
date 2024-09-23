using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierPanelController : MonoBehaviour, IInfoMenuInit
{
    [SerializeField] private GameObject soldierProductionButtonPrefab;
    [SerializeField] private GameObject panelBody;
    private List<SoldierProductionButton> soldierButtons = new List<SoldierProductionButton>();

    public void OnInfoMenuInit(InfoMenuController infoMenuController)
    {
        infoMenuController.OnNormalBuildingSet += ClosePanel;
        infoMenuController.OnBuildingWithSoldierSet += ActivatePanel;
    }

    private void ActivatePanel(List<SoldierType> soldierTypes)
    {
        panelBody.SetActive(true);

        if (soldierTypes.Count > soldierButtons.Count)
        {
            int newButtonAmount = soldierTypes.Count - soldierButtons.Count;

            for (int i = 0; i < newButtonAmount; i++)
            {
                SoldierProductionButton newButton = Instantiate(soldierProductionButtonPrefab, panelBody.transform).GetComponent<SoldierProductionButton>();
                soldierButtons.Add(newButton);
            }

            for (int i = 0;i < soldierTypes.Count;i++)
            {
                soldierButtons[i].Init(soldierTypes[i]);
            }
        }
    }

    private void ClosePanel()
    {
        panelBody.SetActive(false);
    }
}
