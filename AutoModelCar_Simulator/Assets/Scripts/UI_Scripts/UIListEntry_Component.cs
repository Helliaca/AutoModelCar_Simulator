using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIListEntry_Component : UIListEntry
{
    public PropComponent reference;

    public override void Start() {
        switch(reference.type) {
            case PropComponent.ComponentType.CAMERA : {
                this.id = "Camera";
                this.entryPanel_symbol.text = ((char)int.Parse("f030", System.Globalization.NumberStyles.HexNumber)).ToString();
                break;
            }
            case PropComponent.ComponentType.LASERSCANNER : {
                this.id = "LaserScanner";
                this.entryPanel_symbol.text = ((char)int.Parse("f110", System.Globalization.NumberStyles.HexNumber)).ToString();
                break;
            }
            case PropComponent.ComponentType.TRANSFORM : {
                this.id = "Transform";
                this.entryPanel_symbol.text = ((char)int.Parse("f0b2", System.Globalization.NumberStyles.HexNumber)).ToString();
                break;
            }
            case PropComponent.ComponentType.PROPAXLE: {
                this.id = "PropulsionAxle";
                this.entryPanel_symbol.text = ((char)int.Parse("f085", System.Globalization.NumberStyles.HexNumber)).ToString();
                break;
            }
            case PropComponent.ComponentType.STEERAXLE: {
                this.id = "SteeringAxle";
                this.entryPanel_symbol.text = ((char)int.Parse("f085", System.Globalization.NumberStyles.HexNumber)).ToString();
                break;
            }
            case PropComponent.ComponentType.GPS : {
                this.id = "GPS";
                this.entryPanel_symbol.text = ((char)int.Parse("f5a0", System.Globalization.NumberStyles.HexNumber)).ToString();
                break;
            }
            default : { break; }
        }
        base.Start();
    }

    public override void Delete() {
        Globals.Instance.ContextPanel.gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
    }

    public override void Select() {
        base.Select();
        Globals.Instance.ContextPanel.gameObject.SetActive(true);
        Globals.Instance.ContextPanel.setComponent(reference);
        Globals.Instance.spaceHandle.setPos(reference.reference.transform.position);
    }
}
