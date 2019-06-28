using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpaceHandleController : MonoBehaviour
{
    public Transform handleObject;

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null) {
            RaycastHit hit;
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out hit, Mathf.Infinity)) {
                Globals.Instance.PorpList.DeSelectAll();
                Globals.Instance.ComponentList.DeSelectAll();
                handleObject.gameObject.SetActive(true);
                handleObject.position = hit.point;
            }
        }
    }

    public void setPos(Vector3 pos) {
        handleObject.position = pos;
    }
}
