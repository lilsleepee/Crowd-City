using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Follower : MonoBehaviour
{
    public event OnSetLeader onSetLeader;
    public delegate void OnSetLeader(Leader leader);

    [SerializeField] private InviteEffect _circleEffect;
    [SerializeField] private SkinnedMeshRenderer _mesh;

    private CaptureCharacter _captureCharacter;
    private Leader _leader;
    
    private void Awake()
    {
        _captureCharacter = GetComponent<CaptureCharacter>();
    }

    public void ActiveCharacter(Leader target)
    {
        if (_leader == target) return;
        if (HasLeader()) _leader.MinusAgent();

        _captureCharacter.SetLeader(target);

        _circleEffect.gameObject.SetActive(true);
        _circleEffect.SetColor(target.LeaderColor);
        _mesh.material.color = target.LeaderColor;

        target.AddAgent();
        _leader = target;
        onSetLeader?.Invoke(target);
    }


    public bool HasLeader()
    {
        return true ? _leader != null : false;
    }

    public Leader GetLeader
    {
        get
        {
            return _leader;
        }
    }
}

