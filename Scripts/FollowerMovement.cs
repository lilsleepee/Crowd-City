using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerMovement : MonoBehaviour
{
    private const string animatorWalk = "Walking";
    private const string animatorRun = "Run";

    [SerializeField] private float _speedRun = 1;
    [SerializeField] private float _speedWalking = 1;

    private Vector3 _randomTarget;
    private Transform _target;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private int _walkingAnimId = 0;
    private NavMeshAgent _agent;
    private Follower _follower;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _randomTarget = new Vector3(Random.Range(-Plane.Width, Plane.Width), 1, Random.Range(-Plane.Height, Plane.Height));
        _walkingAnimId = Random.Range(0, 2);
        _agent.speed = _speedWalking;
        _animator.SetInteger(animatorWalk, _walkingAnimId);
        _follower = GetComponent<Follower>();
        _follower.onSetLeader += EnableRun;
        if (_agent.isOnNavMesh)
        {
            _agent.SetDestination(_randomTarget);
        }
    }

    void LateUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_target != null)
        {
             if (_agent.isOnNavMesh)
             {
                _agent.SetDestination(_target.position);
             }
        }
        else
        {
            if (_agent.pathStatus != NavMeshPathStatus.PathComplete || _rigidbody.IsSleeping() || !_agent.hasPath)
            {
                _randomTarget = new Vector3(Random.Range(-Plane.Width, Plane.Width), 1, Random.Range(-Plane.Height, Plane.Height));
                if (_agent.isOnNavMesh)
                {
                    _agent.SetDestination(_randomTarget);
                }
            }
        }
    }

    private void EnableRun(Leader leader)
    {
        _animator.SetInteger(animatorWalk, -1);
        _animator.SetBool(animatorRun, true);
        _agent.speed = _speedRun;

        _target = leader.transform;
    }
}
