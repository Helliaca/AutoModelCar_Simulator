using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CP_LaserPanelController : CP_PanelController
{

    public InputField pos_x, pos_y, pos_z;
    public InputField rot_x, rot_y, rot_z;

    public InputField samples, angle_min, angle_max, range_min, range_max, topicname, update_rate;

    private RosSharp.RosBridgeClient.LaserScanReader ls_reader;
    private RosSharp.RosBridgeClient.LaserScanPublisher ls_pub;

    void Update()
    {
        if(!pos_x.isFocused) { pos_x.text = reference.localPosition.x.ToString(); }
        if(!pos_y.isFocused) { pos_y.text = reference.localPosition.y.ToString(); }
        if(!pos_z.isFocused) { pos_z.text = reference.localPosition.z.ToString(); }

        if(!rot_x.isFocused) { rot_x.text = reference.localEulerAngles.x.ToString(); }
        if(!rot_y.isFocused) { rot_y.text = reference.localEulerAngles.y.ToString(); }
        if(!rot_z.isFocused) { rot_z.text = reference.localEulerAngles.z.ToString(); }

        if(!samples.isFocused) { samples.text = ls_reader.samples.ToString(); }
        if(!angle_min.isFocused) { angle_min.text = ls_reader.angle_min.ToString(); }
        if(!angle_max.isFocused) { angle_max.text = ls_reader.angle_max.ToString(); }
        if(!range_min.isFocused) { range_min.text = ls_reader.range_min.ToString(); }
        if(!range_max.isFocused) { range_max.text = ls_reader.range_max.ToString(); }
        if(!topicname.isFocused) { topicname.text = ls_pub.Topic; }
        if(!update_rate.isFocused) { update_rate.text = ls_reader.update_rate.ToString(); }
    }

    public void forceUpdate() {
        try {
            if(pos_x.text!="") reference.localPosition = new Vector3(float.Parse(pos_x.text), reference.localPosition.y, reference.localPosition.z);
            if(pos_y.text!="") reference.localPosition = new Vector3(reference.localPosition.x, float.Parse(pos_y.text), reference.localPosition.z);
            if(pos_z.text!="") reference.localPosition = new Vector3(reference.localPosition.x, reference.localPosition.y, float.Parse(pos_z.text));

            if(rot_x.text!="") reference.localEulerAngles = new Vector3(float.Parse(rot_x.text), reference.localEulerAngles.y, reference.localEulerAngles.z);
            if(rot_y.text!="") reference.localEulerAngles = new Vector3(reference.localEulerAngles.x, float.Parse(rot_y.text), reference.localEulerAngles.z);
            if(rot_z.text!="") reference.localEulerAngles = new Vector3(reference.localEulerAngles.x, reference.localEulerAngles.y, float.Parse(rot_z.text));

            if(samples.text!="") {
                ls_reader.samples = (int)float.Parse(samples.text);
                ls_reader.angle_increment = Mathf.Abs(ls_reader.angle_max - ls_reader.angle_min) / ls_reader.samples;
            }
            if(angle_min.text!="") {
                ls_reader.angle_min = float.Parse(angle_min.text);
                ls_reader.angle_increment = Mathf.Abs(ls_reader.angle_max - ls_reader.angle_min) / ls_reader.samples;
            }
            if(angle_max.text!="") {
                ls_reader.angle_max = float.Parse(angle_max.text);
                ls_reader.angle_increment = Mathf.Abs(ls_reader.angle_max - ls_reader.angle_min) / ls_reader.samples;
            }
            if(range_min.text!="") ls_reader.range_min = float.Parse(range_min.text);
            if(range_max.text!="") ls_reader.range_max = float.Parse(range_max.text);
            if(topicname.text!="") ls_pub.Topic = topicname.text;
            if(update_rate.text!="") ls_reader.update_rate = (int)float.Parse(update_rate.text);
        }
        catch(System.Exception e) {Globals.Instance.DevConsole.error("Exception occurred while reading TransformPanel Components: " + e.ToString());}
    }

    public override void set_reference(Transform o) {
        reference = o;
        ls_pub = o.GetComponent<RosSharp.RosBridgeClient.LaserScanPublisher>();
        ls_reader = o.GetComponent<RosSharp.RosBridgeClient.LaserScanReader>();
    }
}
