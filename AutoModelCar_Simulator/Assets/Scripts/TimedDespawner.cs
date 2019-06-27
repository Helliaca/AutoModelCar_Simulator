using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDespawner : MonoBehaviour
{
    float delay = 5.0f;
    void Start()
    {
        Destroy(gameObject, delay);
    }
}
