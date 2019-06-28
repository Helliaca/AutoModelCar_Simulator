using UnityEngine;
using System.Collections;
 
public class FlyCamera : MonoBehaviour {
    public float mainSpeed = 100.0f; //regular speed
    public float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun= 1.0f;
     
    void Update () {
        handle_rotation();
        handle_movement();
        

        lastMouse =  Input.mousePosition;
       
    }

    private void handle_movement() {
        Vector3 p = GetBaseInput();
        totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
        p = p * mainSpeed * Time.deltaTime;
       
        transform.Translate(p);
    }

    private void handle_rotation() {
        lastMouse = Input.mousePosition - lastMouse ;
        lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0 );

        if(transform.parent && Input.GetMouseButton(1)) {
            transform.RotateAround(transform.parent.position, Vector3.up, lastMouse.y);
            transform.RotateAround(transform.parent.position, Vector3.forward, lastMouse.x);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0.0f); // We want roll to be 0 at all times
        }
        else {
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x , transform.eulerAngles.y + lastMouse.y, 0);
            if(Input.GetMouseButton(1)) transform.eulerAngles = lastMouse;
        }
    }

    private void handle_zoom() {

    }
     
    private Vector3 GetBaseInput() { //returns the basic values, if it's 0 than it's not active.
        if(!Input.GetMouseButton(1)) return Vector3.zero;
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey (KeyCode.W)){
            p_Velocity += new Vector3(0, 0 , 1);
        }
        if (Input.GetKey (KeyCode.S)){
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey (KeyCode.A)){
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey (KeyCode.D)){
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}