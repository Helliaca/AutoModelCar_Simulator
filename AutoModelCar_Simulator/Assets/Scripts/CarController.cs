using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.Messages.Standard;
using nav_msgs = RosSharp.RosBridgeClient.Messages.Navigation;
using geo_msgs = RosSharp.RosBridgeClient.Messages.Geometry;


public class CarController : MonoBehaviour
{
    [Header("Car Axles")]
    public PropulsionAxle backAxle;
    public SteeringAxle frontAxle;
    public LaserScanReader lidar;
    //public Transform frontWheel_right;
    //public Transform frontWheel_left;
    // public LaserScanVisualizerLines lsv_lines;
    // public LaserScanVisualizerMesh lsv_mesh;
    // public RosSharp.SensorVisualization.LaserScanVisualizerSpheres lsv_spheres;


    //Private variables
    private float L;    // Distance between axles
    private float R;    // Radius of current turning circle
    private Vector3 C;  // Center of current turning circle

    void Start()
    {
    }

    void Update()
    {
        float Phi = frontAxle.steering_real_rad;
        float Tan_Phi = (Mathf.Abs(Mathf.Tan(Phi))>0.000001f) ? Mathf.Tan(Phi) : 0.000001f; //Avoid Tan(Phi)=0, because it causes trouble

        L = Vector3.Distance(backAxle.transform.position, frontAxle.transform.position); // L is the distance between the front and back axle
        R = Mathf.Abs(L / Tan_Phi);

        float T = frontAxle.T; //Interwheel distance



        float amount_of_rotation=0, lw_rot=0, rw_rot=0;
        if(Phi>0) {
            C = backAxle.transform.position + transform.right * R;
            amount_of_rotation = (backAxle.speed_real*Time.deltaTime)/R;

            //Individual wheel rotation
            lw_rot = Mathf.Rad2Deg * Mathf.Atan(L/(R-T/2));
            rw_rot = Mathf.Rad2Deg * Mathf.Atan(L/(R+T/2));
        }
        else {
            C = backAxle.transform.position - transform.right * R;
            amount_of_rotation = -(backAxle.speed_real*Time.deltaTime)/R;

            //Individual wheel rotation
            lw_rot = -1f * Mathf.Rad2Deg * Mathf.Atan(L/(R-T/2));
            rw_rot = -1f * Mathf.Rad2Deg * Mathf.Atan(L/(R+T/2));
        }
        frontAxle.set_wheel_rotations(lw_rot, rw_rot);

        // GUI and HUD stuff
        if(Globals.Instance.c_marker.gameObject.activeSelf) {
            Globals.Instance.c_marker.position = C;
        }
        if(Globals.Instance.CircleDraw.gameObject.activeSelf) {
            Globals.Instance.CircleDraw.center = C;
            Globals.Instance.CircleDraw.xradius = R;
            Globals.Instance.CircleDraw.yradius = R;
        }
        if(Globals.Instance.Ack_HUD.gameObject.activeSelf) {
            Globals.Instance.Ack_HUD.setValues(Mathf.Rad2Deg*Phi, lw_rot, rw_rot, L, R, C, Mathf.Rad2Deg*amount_of_rotation*1f/Time.deltaTime);
        }

        transform.RotateAround(C, Vector3.up, Mathf.Rad2Deg*amount_of_rotation);
    }
}
