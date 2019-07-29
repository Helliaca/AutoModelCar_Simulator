using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.Messages.Standard;
using nav_msgs = RosSharp.RosBridgeClient.Messages.Navigation;
using geo_msgs = RosSharp.RosBridgeClient.Messages.Geometry;

public class GPSController : Publisher<nav_msgs.Odometry>
{
    public float odom_frequency = 30;
    public Vector3 origin;
    private string odom_pub;
    private float stime = 0.0f;


    protected override void Start()
    {
        naming();
        base.Start();
    }

    private void naming() {
        PropComponentGroup master = GetComponentInParent(typeof(PropComponentGroup)) as PropComponentGroup; 
        if(master==null) {Globals.Instance.DevConsole.error("Component without master group encountered!"); return;}
        this.gameObject.name = master.gameObject.name + "_gps";
        Topic = Globals.Instance.normalize_from_settings("Default_TopicNames_Gps", master.id.ToString(), master.gameObject.name, "gps");
    }

    void FixedUpdate() {
        if(Time.time < stime + 1.0f/odom_frequency) return;

        nav_msgs.Odometry co = new nav_msgs.Odometry();
        co.pose.pose.position = new geo_msgs.Point();
        Vector3 pos = RosSharp.TransformExtensions.Unity2Ros(origin + transform.position);
        co.pose.pose.position.x = pos.x;
        co.pose.pose.position.y = pos.y;
        co.pose.pose.position.z = pos.z;

        co.pose.pose.orientation = new geo_msgs.Quaternion();
        Quaternion rot = RosSharp.TransformExtensions.Unity2Ros(transform.rotation);
        co.pose.pose.orientation.x = rot.x;
        co.pose.pose.orientation.y = rot.y;
        co.pose.pose.orientation.z = rot.z;
        co.pose.pose.orientation.w = rot.w;

        Publish(co);
        stime = Time.time;
    }
}
