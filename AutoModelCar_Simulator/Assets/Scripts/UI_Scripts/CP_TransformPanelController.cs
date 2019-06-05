using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CP_TransformPanelController : CP_PanelController
{

    public InputField pos_x, pos_y, pos_z;
    public InputField rot_x, rot_y, rot_z;
    public InputField scl_x, scl_y, scl_z;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!pos_x.isFocused) { pos_x.text = reference.localPosition.x.ToString(); }
        if(!pos_y.isFocused) { pos_y.text = reference.localPosition.y.ToString(); }
        if(!pos_z.isFocused) { pos_z.text = reference.localPosition.z.ToString(); }

        if(!rot_x.isFocused) { rot_x.text = reference.localEulerAngles.x.ToString(); }
        if(!rot_y.isFocused) { rot_y.text = reference.localEulerAngles.y.ToString(); }
        if(!rot_z.isFocused) { rot_z.text = reference.localEulerAngles.z.ToString(); }

        if(!scl_x.isFocused) { scl_x.text = reference.localScale.x.ToString(); }
        if(!scl_y.isFocused) { scl_y.text = reference.localScale.y.ToString(); }
        if(!scl_z.isFocused) { scl_z.text = reference.localScale.z.ToString(); }
    }

    public void forceUpdate() {
        try {
            if(pos_x.text!="") reference.localPosition = new Vector3(float.Parse(pos_x.text), reference.localPosition.y, reference.localPosition.z);
            if(pos_y.text!="") reference.localPosition = new Vector3(reference.localPosition.x, float.Parse(pos_y.text), reference.localPosition.z);
            if(pos_z.text!="") reference.localPosition = new Vector3(reference.localPosition.x, reference.localPosition.y, float.Parse(pos_z.text));

            if(rot_x.text!="") reference.localEulerAngles = new Vector3(float.Parse(rot_x.text), reference.localEulerAngles.y, reference.localEulerAngles.z);
            if(rot_y.text!="") reference.localEulerAngles = new Vector3(reference.localEulerAngles.x, float.Parse(rot_y.text), reference.localEulerAngles.z);
            if(rot_z.text!="") reference.localEulerAngles = new Vector3(reference.localEulerAngles.x, reference.localEulerAngles.y, float.Parse(rot_z.text));

            if(scl_x.text!="") reference.localScale = new Vector3(float.Parse(scl_x.text), reference.localScale.y, reference.localScale.z);
            if(scl_y.text!="") reference.localScale = new Vector3(reference.localScale.x, float.Parse(scl_y.text), reference.localScale.z);
            if(scl_z.text!="") reference.localScale = new Vector3(reference.localScale.x, reference.localScale.y, float.Parse(scl_z.text));
        }
        catch(System.Exception e) {Globals.Instance.DevConsole.error("Exception occurred while reading TransformPanel Components: " + e.ToString());}
    }
}
