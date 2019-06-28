using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CP_PanelController : MonoBehaviour
{
    protected Transform reference;

    public virtual void set_reference(Transform o) {
        reference = o;
    }
}
