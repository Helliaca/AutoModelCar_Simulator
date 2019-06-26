using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlPanelController : MonoBehaviour
{
    private Vector3 og_position;
    private Quaternion og_rotation;
    
    void Start()
    {
        og_position = Camera.main.transform.position;
        og_rotation = Camera.main.transform.rotation;
    }

    
    void Update()
    {
        
    }

    public void look_at_selected() {
        Camera.main.transform.LookAt(Globals.Instance.spaceHandle.handleObject);
    }

    public void reset() {
        Camera.main.transform.SetParent(null);
        Camera.main.transform.position = og_position;
        Camera.main.transform.rotation = og_rotation;
    }

    public void track_selected() {
        Transform sel = Globals.Instance.Selected_Prop.reference.transform;
        Camera.main.transform.position = sel.position  + sel.up*0.8f - sel.forward*1.0f;
        Camera.main.transform.LookAt(sel.position);
        Camera.main.transform.SetParent(sel);
    }

    public void toggle_console() {
        Globals.Instance.DevConsole.toggle();
    }

    public void toggle_inspector() {
        Globals.Instance.DevConsole.execute("toggle Objects_Panel");
    }
}
