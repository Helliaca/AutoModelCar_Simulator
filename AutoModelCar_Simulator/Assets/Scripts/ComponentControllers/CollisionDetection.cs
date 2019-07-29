using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public GameObject collisionmarker;
    public bool stop_car_on_collision = true;

    private void naming() {
        PropComponentGroup master = GetComponentInParent(typeof(PropComponentGroup)) as PropComponentGroup; 
        if(master==null) {Globals.Instance.DevConsole.error("Component without master group encountered!"); return;}
        this.gameObject.name = master.gameObject.name + "_collisiondetection";
    }

    void Start() {
        naming();
    }

    public void OnTriggerEnter(Collider other)
    {
        Vector3 col = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        Instantiate(collisionmarker, col, Quaternion.identity);
        Globals.Instance.DevConsole.warn("Car collision detected at: " + col.ToString() + " (" + gameObject.name + ")");

        if(stop_car_on_collision) {
            gameObject.GetComponentsInParent<CarController>()[0].backAxle.stop();
        }
    }
}
