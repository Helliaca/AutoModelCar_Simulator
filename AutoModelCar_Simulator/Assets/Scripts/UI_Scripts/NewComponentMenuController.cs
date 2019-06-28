using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewComponentMenuController : MonoBehaviour
{
    public void add_GPS() {
        PropComponentGroup group = Globals.Instance.Selected_Prop.reference.GetComponent<PropComponentGroup>();
        if(group != null) {
            group.add_GPS();
            Globals.Instance.ComponentList.refresh_components(group);
        }
        else {
            Globals.Instance.DevConsole.error("Selected Prop has no PropComponentGroup Component!");
        }
    }

    public void add_LaserScanner() {
        PropComponentGroup group = Globals.Instance.Selected_Prop.reference.GetComponent<PropComponentGroup>();
        if(group != null) {
            group.add_LaserScanner();
            Globals.Instance.ComponentList.refresh_components(group);
        }
        else {
            Globals.Instance.DevConsole.error("Selected Prop has no PropComponentGroup Component!");
        }
    }

    public void add_Camera() {
        PropComponentGroup group = Globals.Instance.Selected_Prop.reference.GetComponent<PropComponentGroup>();
        if(group != null) {
            group.add_Camera();
            Globals.Instance.ComponentList.refresh_components(group);
        }
        else {
            Globals.Instance.DevConsole.error("Selected Prop has no PropComponentGroup Component!");
        }
    }
}
