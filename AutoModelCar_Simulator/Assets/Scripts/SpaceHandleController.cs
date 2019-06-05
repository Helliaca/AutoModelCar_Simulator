﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpaceHandleController : MonoBehaviour
{
    public Transform handleObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null) {
            RaycastHit hit;
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out hit, Mathf.Infinity)) {
                handleObject.gameObject.SetActive(true);
                handleObject.position = hit.point;
            }
        }
    }

    public void setPos(Vector3 pos) {
        handleObject.position = pos;
    }
}