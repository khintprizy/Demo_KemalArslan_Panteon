using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteSWController : MonoBehaviour
{
    public Action<RectTransform> OnContentButtonRectAdded;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private VerticalLayoutGroup verticalGroup;
    [SerializeField] private ScrollRect scrollRect;
    private List<RectTransform> buttons = new List<RectTransform>();

    private bool isChanged;
    private Vector2 lastVelocity;

    private float heightWithSpacing;
    private float heightWithoutSpacing;

    private void Start()
    {
        OnContentButtonRectAdded += AddButton;

        IInfiniteSWInit[] inits = GetComponentsInChildren<IInfiniteSWInit>();
        for (int i = 0; i < inits.Length; i++)
        {
            inits[i]?.OnInfiniteScrollViewInit(this);
        }

        if (buttons.Count < 1) return;

        heightWithSpacing = MathF.Sqrt(buttons.Count) * (buttons[0].rect.height + verticalGroup.spacing);
        heightWithoutSpacing = MathF.Sqrt(buttons.Count) * (buttons[0].rect.height);

        SetToMiddle();
    }

    private void Update()
    {
        // Here, I controlled the changed frame of velocity, and if changed set the velocity to prevent any jittered movement
        if (isChanged)
        {
            scrollRect.velocity = lastVelocity;
            isChanged = false;
        }

        if (contentPanel.localPosition.y < 0)
        {
            isChanged = true;
            lastVelocity = scrollRect.velocity;
            contentPanel.localPosition += new Vector3(0, heightWithSpacing, 0);

        }

        if (contentPanel.localPosition.y > (heightWithoutSpacing))
        {
            isChanged = true;
            lastVelocity = scrollRect.velocity;
            contentPanel.localPosition -= new Vector3(0, heightWithSpacing, 0);
        }
    }

    private void AddButton(RectTransform buttonRectTransform)
    {
        buttons.Add(buttonRectTransform);
    }

    private void SetToMiddle()
    {
        contentPanel.localPosition = new Vector3(contentPanel.localPosition.x, heightWithSpacing, contentPanel.localPosition.z);
    }
}
