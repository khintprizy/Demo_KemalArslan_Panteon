using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector2 limitX;
    [SerializeField] private Vector2 limitY;

    [SerializeField] private float cameraSpeed = 2f;

    private void Update()
    {
        float horiz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        Vector3 currentPos = transform.position;

        Vector3 targetPos = new Vector3(currentPos.x + (horiz * Time.deltaTime * cameraSpeed), currentPos.y + (vert * Time.deltaTime * cameraSpeed), currentPos.z);

        targetPos.x = Mathf.Clamp(targetPos.x, limitX.x, limitX.y);
        targetPos.y = Mathf.Clamp(targetPos.y, limitY.x, limitY.y);

        transform.position = targetPos;
    }
}
