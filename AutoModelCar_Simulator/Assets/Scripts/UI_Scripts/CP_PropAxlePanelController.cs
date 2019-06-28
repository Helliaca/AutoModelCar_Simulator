using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CP_PropAxlePanelController : CP_PanelController
{

    public InputField pos_x, pos_y, pos_z;
    public InputField topicname, interwheeldist;
    private PropulsionAxle axle;

    void Start()
    {
        axle = reference.GetComponent<PropulsionAxle>();
    }


    void Update()
    {
        if(!pos_x.isFocused) { pos_x.text = reference.localPosition.x.ToString(); }
        if(!pos_y.isFocused) { pos_y.text = reference.localPosition.y.ToString(); }
        if(!pos_z.isFocused) { pos_z.text = reference.localPosition.z.ToString(); }
        
        if(!topicname.isFocused) { topicname.text = axle.topic; }
        if(!interwheeldist.isFocused) { interwheeldist.text = axle.T.ToString(); }
    }

    public void forceUpdate() {
        try {
            if(pos_x.text!="") reference.localPosition = new Vector3(float.Parse(pos_x.text), reference.localPosition.y, reference.localPosition.z);
            if(pos_y.text!="") reference.localPosition = new Vector3(reference.localPosition.x, float.Parse(pos_y.text), reference.localPosition.z);
            if(pos_z.text!="") reference.localPosition = new Vector3(reference.localPosition.x, reference.localPosition.y, float.Parse(pos_z.text));

            if(topicname.text!="") axle.topic = topicname.text;
            if(interwheeldist.text!="") axle.T = float.Parse(interwheeldist.text);
        }
        catch(System.Exception e) {Globals.Instance.DevConsole.error("Exception occurred while reading TransformPanel Components: " + e.ToString());}
    }
}