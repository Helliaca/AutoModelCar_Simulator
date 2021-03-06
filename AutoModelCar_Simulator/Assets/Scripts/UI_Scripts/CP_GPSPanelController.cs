﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CP_GPSPanelController : CP_PanelController
{
    public InputField origin_x, origin_y, origin_z;
    public InputField pos_x, pos_y, pos_z;
    public InputField topicname;
    public InputField freq;
    private GPSController gps;

    void Start()
    {
        gps = reference.GetComponent<GPSController>();
    }


    void Update()
    {
        if(!pos_x.isFocused) { pos_x.text = reference.localPosition.x.ToString(); }
        if(!pos_y.isFocused) { pos_y.text = reference.localPosition.y.ToString(); }
        if(!pos_z.isFocused) { pos_z.text = reference.localPosition.z.ToString(); }

        if(!origin_x.isFocused) { origin_x.text = gps.origin.x.ToString(); }
        if(!origin_y.isFocused) { origin_y.text = gps.origin.y.ToString(); }
        if(!origin_z.isFocused) { origin_z.text = gps.origin.z.ToString(); }

        if(!topicname.isFocused) { topicname.text = gps.Topic; }
        if(!freq.isFocused) { freq.text = gps.odom_frequency.ToString(); }
    }

    public void forceUpdate() {
        try {
            if(pos_x.text!="") reference.localPosition = new Vector3(float.Parse(pos_x.text), reference.localPosition.y, reference.localPosition.z);
            if(pos_y.text!="") reference.localPosition = new Vector3(reference.localPosition.x, float.Parse(pos_y.text), reference.localPosition.z);
            if(pos_z.text!="") reference.localPosition = new Vector3(reference.localPosition.x, reference.localPosition.y, float.Parse(pos_z.text));

            if(origin_x.text!="") gps.origin = new Vector3(float.Parse(origin_x.text), gps.origin.y, gps.origin.z);
            if(origin_y.text!="") gps.origin.y = float.Parse(origin_y.text);
            if(origin_z.text!="") gps.origin.z = float.Parse(origin_z.text);

            if(topicname.text!="") gps.changeTopicName(topicname.text);

            if(freq.text!="") gps.odom_frequency = float.Parse(freq.text);
        }
        catch(System.Exception e) {Globals.Instance.DevConsole.error("Exception occurred while reading TransformPanel Components: " + e.ToString());}
    }
}