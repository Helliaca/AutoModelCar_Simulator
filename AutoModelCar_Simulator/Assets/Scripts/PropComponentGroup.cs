using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PropComponent {
    [System.Serializable]
    public enum ComponentType { TRANSFORM, CAMERA, LASERSCANNER, PROPAXLE, STEERAXLE, GPS };
    public GameObject reference;
    public ComponentType type;
}

public class PropComponentGroup : MonoBehaviour
{
    public List<PropComponent> components = new List<PropComponent>();
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
