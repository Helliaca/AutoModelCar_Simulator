using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.Messages.Standard;
using nav_msgs = RosSharp.RosBridgeClient.Messages.Navigation;
using geo_msgs = RosSharp.RosBridgeClient.Messages.Geometry;
using autominy_msgs = RosSharp.RosBridgeClient.Messages.Autominy;


public class CarController : MonoBehaviour
{
    [Header("Car Axles")]
    public PropulsionAxle backAxle;
    public SteeringAxle frontAxle;
    public LaserScanReader lidar;


    //Private variables
    private float L;    // Distance between axles
    private float R;    // Radius of current turning circle
    private Vector3 C;  // Center of current turning circle
    private float Phi=0;    // Rotation of virtual front-center wheel
    private float lw_rot=0, rw_rot=0; // Rotation of left and right wheel
    private float amount_of_rotation=0; // Amount (in RAD) the car rotated this frame

    void Start()
    {
        if(Globals.Instance.CurrentCar==null) Globals.Instance.CurrentCar = this;
    }

    void Update()
    {
        Phi = frontAxle.steering_real_rad;
        float Tan_Phi = (Mathf.Abs(Mathf.Tan(Phi))>0.000001f) ? Mathf.Tan(Phi) : 0.000001f; //Avoid Tan(Phi)=0, because it causes trouble

        L = Vector3.Distance(backAxle.transform.position, frontAxle.transform.position); // L is the distance between the front and back axle
        R = Mathf.Abs(L / Tan_Phi);

        float T = frontAxle.T; //Interwheel distance



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

        backAxle.add_distance_travelled(amount_of_rotation * R);

        transform.RotateAround(C, Vector3.up, Mathf.Rad2Deg*amount_of_rotation);
    }

    public void UpdateAckermannValues(AckController ack) {
        ack.setValues(Mathf.Rad2Deg*Phi, lw_rot, rw_rot, L, R, C, Mathf.Rad2Deg*amount_of_rotation*1f/Time.deltaTime);
    }

    public void UpdateTurningCircleValues(CircleRenderer cir) {
        cir.center = C;
        cir.xradius = R;
        cir.yradius = R;
    }
}
