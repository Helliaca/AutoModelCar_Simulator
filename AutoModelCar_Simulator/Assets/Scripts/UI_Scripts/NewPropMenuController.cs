using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPropMenuController : MonoBehaviour
{
    public GameObject min_car_prefab;
    public GameObject max_car_prefab;
    public GameObject cless_car_prefab;
    public UIListController prop_list;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawn_min_car() {
        GameObject o = Instantiate(min_car_prefab, Globals.Instance.spaceHandle.handleObject.position, Quaternion.identity);
        prop_list.Add_Prop(o);
    }
    public void spawn_max_car() {
        GameObject o = Instantiate(max_car_prefab, Globals.Instance.spaceHandle.handleObject.position, Quaternion.identity);
        prop_list.Add_Prop(o);
    }
    public void spawn_cless_car() {
        GameObject o = Instantiate(cless_car_prefab, Globals.Instance.spaceHandle.handleObject.position, Quaternion.identity);
        prop_list.Add_Prop(o);
    }
}
