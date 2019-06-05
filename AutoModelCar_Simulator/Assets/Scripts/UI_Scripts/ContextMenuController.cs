using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(Input.GetMouseButtonDown(0)) StartCoroutine(hide_after_wait());
    }

    //NOTE: Very unelegant-solution.
    //We need clicks on buttons to register while still hiding these panels on any mouse-click. Solution: wait 0.3s before hiding panel
    private IEnumerator hide_after_wait() {
        yield return new WaitForSecondsRealtime(0.3f);
        gameObject.SetActive(false);
    }

    public void show() {
        Vector3 pos;
        if(gameObject.layer == 9) pos = Globals.Instance.UI_GRAPH_CAMERA.ScreenToWorldPoint(Input.mousePosition); //Layer 9 is GRAPH_UI, requires special treatment
        else pos = Input.mousePosition;

        GetComponent<RectTransform>().position = pos;

        Vector3 p = GetComponent<RectTransform>().anchoredPosition3D;
        p.z = 0;
        GetComponent<RectTransform>().anchoredPosition3D = p;

        gameObject.SetActive(true);
    }
}
