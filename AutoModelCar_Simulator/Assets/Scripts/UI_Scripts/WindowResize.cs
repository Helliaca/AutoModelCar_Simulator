using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WindowResize : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public RectTransform window;
    private bool isPressed = false;
    void Update()
    {
        //NOTE: This script will only work so long as the pivot of the window is in the top left and resize button on the bottom right
        if(isPressed && Input.GetMouseButton(0)) {
            Vector2 old_pos;
            if(gameObject.layer == 9) {
                old_pos = Globals.Instance.UI_GRAPH_CAMERA.WorldToScreenPoint(window.position); //Layer 9 is GRAPH_UI, requires special treatment
                window.sizeDelta = new Vector2(Mathf.Abs(old_pos.x - Input.mousePosition.x), Mathf.Abs(old_pos.y - Input.mousePosition.y));
            }
            else {
                old_pos = window.anchoredPosition; //Camera.main.WorldToScreenPoint(window.position);
                float m_u_y = Screen.height - Input.mousePosition.y; //mouse position y Coordinate with the top row being 0 and the bottom row being the maximum
                window.sizeDelta = new Vector2(Mathf.Abs(old_pos.x - Input.mousePosition.x), Mathf.Abs(-old_pos.y - m_u_y));
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
