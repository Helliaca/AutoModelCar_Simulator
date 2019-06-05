using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComponentPanel
{
    public RectTransform panel;
    public PropComponent.ComponentType cType;
}


public class ContextPanelController : MonoBehaviour
{
    public ComponentPanel[] panels;

    private PropComponent reference;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setComponent(PropComponent comp) {
        foreach(ComponentPanel p in panels) {
            if(p.cType==comp.type) {
                p.panel.GetComponent<CP_PanelController>().set_reference(comp.reference.transform);
                p.panel.gameObject.SetActive(true);
            }
            else p.panel.gameObject.SetActive(false);
        }
    }
}
