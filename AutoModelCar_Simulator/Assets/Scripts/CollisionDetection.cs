using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public GameObject collisionmarker;
    public bool stop_car_on_collision = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        Vector3 col = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        Instantiate(collisionmarker, col, Quaternion.identity);
        Globals.Instance.DevConsole.warn("Car collision detected at: " + col.ToString());

        if(stop_car_on_collision) {
            gameObject.GetComponentsInParent<CarController>()[0].backAxle.stop();
        }
    }
}
