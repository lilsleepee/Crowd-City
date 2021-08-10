using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public static FollowPlayer Instance;

    [SerializeField] private Vector3 _offset;
    [SerializeField] private Transform _target;
    [SerializeField] private int _indexBlockedMesh;
    [SerializeField] private float _offsetMultiplier = 0.2f;

    MeshRenderer blockedMesh;

    private void Awake()
    {
        Instance = this;
    }

    public void SetTarget(Transform t)
    {
        _target = t;
        _target.gameObject.GetComponent<Leader>().onPlayerAddFollower += AddOffset;
    }

    private void Update()
    {
        FollowTarget();
        ReleaseRay();
    }

    private void FollowTarget()
    {
        if (_target == null) return;

        transform.position = _target.position + _offset;
    }

    Vector3 Ray_start_position = new Vector3(Screen.width / 2, Screen.height / 2, 0);

    private void ReleaseRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Ray_start_position);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if(hit.collider.gameObject.layer == _indexBlockedMesh)
        {
            if (hit.collider.gameObject.GetComponent<MeshRenderer>() != blockedMesh)
            {
                if (blockedMesh != null)
                {
                    for (int i = 0; i < blockedMesh.materials.Length; i++)
                    {
                        blockedMesh.materials[i].color = new Color(blockedMesh.materials[i].color.r, blockedMesh.materials[i].color.g, blockedMesh.materials[i].color.b, 1f);
                    }
                    blockedMesh = null;
                }
                blockedMesh = hit.collider.gameObject.GetComponent<MeshRenderer>();
                for (int i = 0; i < blockedMesh.materials.Length; i++)
                {
                    blockedMesh.materials[i].color = new Color(blockedMesh.materials[i].color.r, blockedMesh.materials[i].color.g, blockedMesh.materials[i].color.b, 0f);
                }
            }
        }
        else
        {
            if (blockedMesh != null)
            {
                for (int i = 0; i < blockedMesh.materials.Length; i++)
                {
                    blockedMesh.materials[i].color = new Color(blockedMesh.materials[i].color.r, blockedMesh.materials[i].color.g, blockedMesh.materials[i].color.b, 1f);
                }
                blockedMesh = null;
            }
        }
    }

    public void AddOffset()
    {
        _offset = new Vector3(_offset.x, _offset.y + _offset.y * _offsetMultiplier, _offset.z + _offset.z * _offsetMultiplier);
        transform.rotation *= Quaternion.Euler(transform.rotation.x * _offsetMultiplier, transform.rotation.y, transform.rotation.z);
    }
}
