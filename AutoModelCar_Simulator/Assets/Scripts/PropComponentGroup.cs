using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PropComponent {
    [System.Serializable]
    public enum ComponentType { TRANSFORM, CAMERA, LASERSCANNER, PROPAXLE, STEERAXLE, GPS, COLLISIONDETECTION };
    public GameObject reference;
    public ComponentType type;
    public PropComponent(ComponentType type, GameObject reference) {
        this.type = type;
        this.reference = reference;
    }
}

public class PropComponentGroup : MonoBehaviour
{
    public static int id_counter = 0;
    public int id;
    private bool initialized = false;
    public Transform Component_Container;

    public void add_GPS() {
        GameObject go = Instantiate(Globals.Instance.GPS_prefab, Component_Container);
        PropComponent new_comp = new PropComponent(PropComponent.ComponentType.GPS, go);
        GetComponent<PropComponentGroup>().components.Add(new_comp);
        Globals.Instance.ComponentList.Add_Component(new_comp);
    }

    public void add_LaserScanner() {
        GameObject go = Instantiate(Globals.Instance.LaserScanner_prefab, Component_Container);
        PropComponent new_comp = new PropComponent(PropComponent.ComponentType.LASERSCANNER, go);
        GetComponent<PropComponentGroup>().components.Add(new_comp);
        Globals.Instance.ComponentList.Add_Component(new_comp);
    }

    public void add_Camera() {
        GameObject go = Instantiate(Globals.Instance.Camera_prefab, Component_Container);
        PropComponent new_comp = new PropComponent(PropComponent.ComponentType.CAMERA, go);
        GetComponent<PropComponentGroup>().components.Add(new_comp);
        Globals.Instance.ComponentList.Add_Component(new_comp);
    }

    public void add_Collisiondetection() {
        GameObject go = Instantiate(Globals.Instance.CollisionDetection_prefab, Component_Container);
        PropComponent new_comp = new PropComponent(PropComponent.ComponentType.COLLISIONDETECTION, go);
        GetComponent<PropComponentGroup>().components.Add(new_comp);
        Globals.Instance.ComponentList.Add_Component(new_comp);
    }

    public List<PropComponent> components = new List<PropComponent>();
    
    void Start()
    {
        if(Component_Container==null) Component_Container = transform;
        if(!initialized) init();
    }

    public void init() {
        if(gameObject.isStatic) {initialized = true; return;}
        this.id = id_counter++;
        if(GetComponent<CarController>()!=null) {
            gameObject.name = Globals.Instance.normalize_from_settings("Default_CarNames", id.ToString(), "unnamed", "car");
        }
        else {
            gameObject.name = Globals.Instance.normalize_from_settings("Default_PropNames", id.ToString(), "unnamed", "prop");
        }
        initialized = true;
    }
}
