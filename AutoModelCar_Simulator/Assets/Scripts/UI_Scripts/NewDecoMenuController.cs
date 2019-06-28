using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDecoMenuController : MonoBehaviour
{
    public UIListController prop_list;
    public GameObject cbox_prefab;
    public GameObject devcube_prefab;
    public GameObject devsphere_prefab;
    public GameObject devcylinder_prefab;
    public GameObject svehicle_prefab;
    public GameObject bishop_prefab;
    public GameObject king_prefab;
    public GameObject knight_prefab;
    public GameObject pawn_prefab;

    public void spawn_cbox() {
        GameObject o = Instantiate(cbox_prefab, Globals.Instance.spaceHandle.handleObject.position, Quaternion.identity);
        prop_list.Add_Prop(o);
    }

    public void spawn_devcube() {
        GameObject o = Instantiate(devcube_prefab, Globals.Instance.spaceHandle.handleObject.position, Quaternion.identity);
        prop_list.Add_Prop(o);
    }

    public void spawn_devsphere() {
        GameObject o = Instantiate(devsphere_prefab, Globals.Instance.spaceHandle.handleObject.position, Quaternion.identity);
        prop_list.Add_Prop(o);
    }

    public void spawn_devcylinder() {
        GameObject o = Instantiate(devcylinder_prefab, Globals.Instance.spaceHandle.handleObject.position, Quaternion.identity);
        prop_list.Add_Prop(o);
    }

    public void spawn_svehicle() {
        GameObject o = Instantiate(svehicle_prefab, Globals.Instance.spaceHandle.handleObject.position, Quaternion.identity);
        prop_list.Add_Prop(o);
    }

    public void spawn_bishop() {
        GameObject o = Instantiate(bishop_prefab, Globals.Instance.spaceHandle.handleObject.position, Quaternion.identity);
        prop_list.Add_Prop(o);
    }

    public void spawn_king() {
        GameObject o = Instantiate(king_prefab, Globals.Instance.spaceHandle.handleObject.position, Quaternion.identity);
        prop_list.Add_Prop(o);
    }

    public void spawn_knight() {
        GameObject o = Instantiate(knight_prefab, Globals.Instance.spaceHandle.handleObject.position, Quaternion.identity);
        prop_list.Add_Prop(o);
    }

    public void spawn_pawn() {
        GameObject o = Instantiate(pawn_prefab, Globals.Instance.spaceHandle.handleObject.position, Quaternion.identity);
        prop_list.Add_Prop(o);
    }

    
}
