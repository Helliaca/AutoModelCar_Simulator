using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.Messages.Standard;

public class CarController : MonoBehaviour
{

    private RosSocket rosSocket;

    private string speed_sub;
    private string steering_sub;

    private float speed;
    private float steering;

    // Start is called before the first frame update
    void Start()
    {
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol("ws://192.168.178.23:9090"));

        speed_sub = rosSocket.Subscribe<std_msgs.Int16>("/manual_control/speed", speed_callback);
        steering_sub = rosSocket.Subscribe<std_msgs.UInt8>("/steering", steering_callback);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = new Vector3(0,0,0);
        offset += transform.forward * speed * Mathf.Cos(Mathf.Deg2Rad * steering);
        offset += transform.right * speed * Mathf.Sin(Mathf.Deg2Rad * steering);
        transform.position += offset * Time.deltaTime * 0.01f;
        transform.Rotate(Vector3.up, speed * steering * Time.deltaTime * 0.01f);
    }

    private void speed_callback(std_msgs.Int16 data) {
        speed = (float)data.data;
        Globals.Instance.DevConsole.print("New speed: " + data.data);
    }

    private void steering_callback(std_msgs.UInt8 data) {
        steering = (float)data.data;
        steering = (steering/180.0f) * 50.0f - 25.0f; //converting coordinates from [0,180] to [-25,25]
        Globals.Instance.DevConsole.print("New steering: " + data.data);
    }
}
