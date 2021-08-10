using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public static float Height;
    public static float Width;
    
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        Height = _collider.bounds.size.z / 2;
        Width = _collider.bounds.size.x / 2;
    }
}
