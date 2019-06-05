using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIListEntry_Prop : UIListEntry
{
    public GameObject reference;

    public override void Start() {
        if(reference) this.id = reference.name;
        if(!reference || !reference.GetComponent<CarController>()) this.entryPanel_symbol.text = ((char)int.Parse("f1b2", System.Globalization.NumberStyles.HexNumber)).ToString();

        base.Start();
    }

    new public void Delete() {
        Globals.Instance.ContextPanel.gameObject.SetActive(false);
        if(!reference) Globals.Instance.DevConsole.error("UIListEntry_Prop does not have a reference!");
        else GameObject.Destroy(reference);
        GameObject.Destroy(gameObject);
    }

    public override void Select() {
        Globals.Instance.ComponentList.Flush();

        if(!reference) { Globals.Instance.DevConsole.error("UIListEntry_Prop does not have a reference!"); return; }
        PropComponentGroup group = reference.GetComponent<PropComponentGroup>();
        if(!group)  { Globals.Instance.DevConsole.error("UIListEntry_Prop points to reference without a PropComponentGroup Component!"); return; }

        if(reference.GetComponent<CarController>()) {
            Globals.Instance.CurrentCar = reference.GetComponent<CarController>();
        }

        foreach(PropComponent c in group.components) {
            Globals.Instance.ComponentList.Add_Component(c);
        }

        Globals.Instance.spaceHandle.setPos(reference.transform.position);

        base.Select();
    }
}
