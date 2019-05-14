using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WindowHandle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public RectTransform window;
    private bool isPressed = false;
    private Vector3 offset;
    void Update()
    {
        if(isPressed && Input.GetMouseButton(0)) {
            window.position = Globals.Instance.UI_GRAPH_CAMERA.ScreenToWorldPoint(Input.mousePosition + offset);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        offset = Globals.Instance.UI_GRAPH_CAMERA.WorldToScreenPoint(window.position) - Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
