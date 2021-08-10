using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorPool : MonoBehaviour
{
    public static IndicatorPool Instance;

    [SerializeField] private Indicator _pooledObject;
    [SerializeField] private int _pooledAmount = 1;
    [SerializeField] private bool _willGrow = true;

    List<Indicator> pooledObjects;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pooledObjects = new List<Indicator>();

        for (int i = 0; i < _pooledAmount; i++)
        {
            Indicator arrow = Instantiate(_pooledObject);
            arrow.transform.SetParent(transform, false);
            arrow.Activate(false);
            pooledObjects.Add(arrow);
        }
    }

    public Indicator GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].Active)
            {
                return pooledObjects[i];
            }
        }
        if (_willGrow)
        {
            Indicator arrow = Instantiate(_pooledObject);
            arrow.transform.SetParent(transform, false);
            arrow.Activate(false);
            pooledObjects.Add(arrow);
            return arrow;
        }
        return null;
    }

    public void DeactivateAllPooledObjects()
    {
        foreach (Indicator arrow in pooledObjects)
        {
            arrow.Activate(false);
        }
    }
}
