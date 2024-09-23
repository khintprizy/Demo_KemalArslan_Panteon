using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenuController : MonoBehaviour
{
    [SerializeField] private Button replaceButton;
    [SerializeField] private GameObject panel;
    private BuildingBase currentBuilding;

    public Action<EntityModelData> OnInfoMenuViewChange { get; set; }
    public Action OnNormalBuildingSet { get; set; }
    public Action<List<SoldierType>> OnBuildingWithSoldierSet { get; set; }

    private void Start()
    {
        IInfoMenuInit[] inits = GetComponentsInChildren<IInfoMenuInit>();
        for (int i = 0; i < inits.Length; i++)
        {
            inits[i]?.OnInfoMenuInit(this);
        }

        replaceButton.onClick.AddListener(OnReplaceButtonClicked);

        EventManager.OnBuildingClickedOnTheGrid += BuildingClickListener;
        EventManager.OnBuildingWithSoldiersClickedOnTheGrid += BuildingWithSoldierClickListener;
        EventManager.OnSoldierSelected += OnSoldierSelected;

        EventManager.CreateBuilding += OnBuildingCreated;

        PanelActivation(false);
    }

    

    private void BuildingClickListener(BuildingBase building, EntityModelData occupierData)
    {
        currentBuilding = building;
        PanelActivation(true);
        OnInfoMenuViewChange?.Invoke(occupierData);

        OnNormalBuildingSet?.Invoke();
    }

    private void BuildingWithSoldierClickListener(BuildingBase building, EntityModelData occupierData, List<SoldierType> soldierTypes)
    {
        currentBuilding = building;
        PanelActivation(true);
        OnInfoMenuViewChange?.Invoke(occupierData);

        // ekstra askerlerle ilgili bilgi gonderilecek
        OnBuildingWithSoldierSet?.Invoke(soldierTypes);
    }

    private void OnSoldierSelected()
    {
        PanelActivation(false);
    }

    private void PanelActivation(bool isActive)
    {
        panel.SetActive(isActive);
    }

    private void OnBuildingCreated(BuildingType type)
    {
        PanelActivation(false);
    }

    private void OnReplaceButtonClicked()
    {
        if (currentBuilding == null) return;

        currentBuilding.GetComponent<IPickableFromGrid>()?.PickBuildingFromGrid();
        PanelActivation(false);
    }
}
