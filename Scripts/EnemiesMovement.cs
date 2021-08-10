using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemiesMovement : MonoBehaviour
{
    private const string _playerTag = "Player";
    private const string _enemyTag = "Enemy";
    private const string _animationRun = "Run";

    [SerializeField] private Animator _animator;
    
    private bool _move;
    private Vector3 _targetToRun;
    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _targetToRun = new Vector3(Random.Range(-Plane.Width, Plane.Width), 1, Random.Range(-Plane.Height, Plane.Height));
        _animator.SetBool(_animationRun, true);
        _move = true;
        if (_agent.isOnNavMesh)
        {
            _agent.SetDestination(_targetToRun);
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (_move)
        {
            if (_agent.pathStatus == NavMeshPathStatus.PathPartial || _agent.pathStatus == NavMeshPathStatus.PathInvalid || !_agent.hasPath)
            {
                _targetToRun = new Vector3(Random.Range(-Plane.Width, Plane.Width), 1, Random.Range(-Plane.Height, Plane.Height));
                if (_agent.isOnNavMesh)
                {
                    _agent.SetDestination(_targetToRun);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != _playerTag || other.tag != _enemyTag)
        {
            return;
        }
        
        Leader otherLeader = other.gameObject.GetComponentInParent<Leader>();
        Leader leader = GetComponentInParent<Leader>();
        
        if (otherLeader.AgentCount > leader.AgentCount)
        {
            _targetToRun = transform.position - other.transform.position;
            if (_agent.isOnNavMesh)
            {
                _targetToRun = new Vector3(_targetToRun.x + transform.position.x, 1, _targetToRun.z + transform.position.z);
                if (_agent.isOnNavMesh)
                {
                    _agent.SetDestination(_targetToRun);
                }
            }
        }
        else if (otherLeader.AgentCount < leader.AgentCount)
        {
            if (_agent.isOnNavMesh)
            {
                _targetToRun = other.transform.position;
                if (_agent.isOnNavMesh)
                {
                    _agent.SetDestination(_targetToRun);
                }
            }
        }
    }
}
