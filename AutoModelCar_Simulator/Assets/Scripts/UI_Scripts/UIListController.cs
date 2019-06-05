using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UIListController : MonoBehaviour
{
    public List<UIListEntry> entries;
    public RectTransform ListEntryPrefab;
    public RectTransform AddEntryButton;
    public bool allow_max_one_selected = false;
    public bool refresh_props_on_load = false;
    

    void Start()
    {
        foreach(UIListEntry e in GetComponentsInChildren<UIListEntry>()) {
                entries.Add(e);
            }
        if(refresh_props_on_load) {
            Flush();
            refresh_props();
        }
    }

    public void refresh_props() {
        Flush();
        Object[] objs = GameObject.FindObjectsOfType(typeof(PropComponentGroup));
        foreach(Object o in objs) {
            GameObject prop = ((PropComponentGroup)o).gameObject;
            if(!prop.isStatic) Add_Prop(prop);
        }
    }

    
    void Update()
    {
        for(int i=0; i<entries.Count; i++) {
            if(!entries[i]) entries.RemoveAt(i);
            else if(allow_max_one_selected && UIListEntry.last_selected && entries[i]!=UIListEntry.last_selected) {
                entries[i].DeSelect();
            }
        }
    }

    public void Add() {
		RectTransform ne = Instantiate(ListEntryPrefab, transform);
        entries.Add(ne.GetComponent<UIListEntry>());

        if(AddEntryButton) AddEntryButton.SetAsLastSibling();
    }

    public void Add_Prop(GameObject reference) {
		RectTransform ne = Instantiate(ListEntryPrefab, transform);
        entries.Add(ne.GetComponent<UIListEntry>());

        if(AddEntryButton) AddEntryButton.SetAsLastSibling();

        UIListEntry_Prop lep = ne.GetComponent<UIListEntry_Prop>();
        if(!lep) { Globals.Instance.DevConsole.error("Attempted to Add Prop to UIList, but Prefab does not include a UIListEntry_Prop Component!"); return; }
        else {
            lep.reference = reference;
        }
    }

    public void Add_Component(PropComponent comp) {
		RectTransform ne = Instantiate(ListEntryPrefab, transform);
        entries.Add(ne.GetComponent<UIListEntry>());

        if(AddEntryButton) AddEntryButton.SetAsLastSibling();

        UIListEntry_Component lep = ne.GetComponent<UIListEntry_Component>();
        if(!lep) { Globals.Instance.DevConsole.error("Attempted to Add Prop to UIList, but Prefab does not include a UIListEntry_Component Component!"); return; }
        else {
            lep.reference = comp;
        }
    }

    public void AddRandom() {
        Add();
    }

    public void Flush() {
        foreach(UIListEntry e in entries) {
            if(!e) continue;
            e.Delete();
        }
        entries.Clear();
    }
}
