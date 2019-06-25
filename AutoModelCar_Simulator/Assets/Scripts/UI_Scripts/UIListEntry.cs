using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIListEntry : MonoBehaviour
{
    public RectTransform entryPanel;
    public Text entryPanel_text;
    public Text entryPanel_symbol;
    public string id;
    protected bool is_selected = false;
    public static UIListEntry last_selected;
    
    public virtual void Start()
    {
        entryPanel = GetComponent<RectTransform>();
        entryPanel_text.text = id;
    }

    public virtual void Update()
    {

    }

    public virtual void Delete() {
        GameObject.Destroy(gameObject);
    }

    public virtual void Select() {
        is_selected = true;
        last_selected = this;
        entryPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
    }

    public virtual void DeSelect() {
        if(!entryPanel) entryPanel = GetComponent<RectTransform>();
        is_selected = false;
        entryPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0.0f);
    }

    public virtual void ToggleSelect() {
        if(is_selected) DeSelect();
        else Select();
    }
}
