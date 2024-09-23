using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    private Vector2 mouseWorldPosition;
    public static InputManager Instance;

    public Vector2 MouseWorldPosition => mouseWorldPosition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        mouseWorldPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            EventManager.OnMouseLeftClick?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            EventManager.OnMouseRightClick?.Invoke();
        }
    }
}

public static partial class EventManager
{
    public static Action OnMouseLeftClick { get; set; }
    public static Action OnMouseRightClick { get; set; }
}
