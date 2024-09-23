using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteSWContent : MonoBehaviour, IInfiniteSWInit
{
    [SerializeField] private List<BuildingType> buildingTypes;
    [SerializeField] private GameObject buildingButtonPrefab;
    [SerializeField] private Transform contentPanel;

    public void OnInfiniteScrollViewInit(InfiniteSWController infiniteSWController)
    {
        for (int i = 0; i < buildingTypes.Count; i++)
        {
            for(int j = 0; j < buildingTypes.Count; j++)
            {
                BuildingProductionButton buildingButton = Instantiate(buildingButtonPrefab, contentPanel).GetComponent<BuildingProductionButton>();
                buildingButton.Init(buildingTypes[j]);
                infiniteSWController.OnContentButtonRectAdded?.Invoke(buildingButton.GetComponent<RectTransform>());
            }
        }
    }
}
