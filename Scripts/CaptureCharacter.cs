using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CaptureCharacter : MonoBehaviour
{
    [SerializeField] private int _agentPerLeader;
    [SerializeField] private GameObject _agentPrefab;
    [SerializeField] private float _coolDown = 0.7f;
    [SerializeField] private float _timescape = 0;

    private Leader _leader;

    private void Start()
    {
        _timescape = _coolDown;
    }

    private void Update()
    {
        if (_timescape > 0)
        {
            _timescape -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InviteFollower(other);
    }

    public void SetLeader(Leader leader)
    {
        _leader = leader;
    }

    private void InviteFollower(Collider other)
    {
        if (_leader == null || _timescape > 0)
        {
            return;
        }

        Follower captureMover = other.gameObject.GetComponent<Follower>();
        Leader enemyLeader = other.gameObject.GetComponent<Leader>();

        if (captureMover != null)
        {
            if (captureMover.GetLeader == null)
            {
                captureMover.GetComponent<Follower>().ActiveCharacter(_leader);
                _timescape = _coolDown;
            }
            else
            {
                if (captureMover.GetLeader.AgentCount < _leader.AgentCount)
                {
                    captureMover.GetComponent<Follower>().ActiveCharacter(_leader);
                    _timescape = _coolDown;
                }
            }
        }
        else if (enemyLeader != null)
        {
            if (enemyLeader.AgentCount < 2 && _leader.AgentCount > 1)
            {
                Vector3 spawnPos = other.gameObject.transform.position;
                Destroy(other.gameObject);
                for (int i = 0; i < _agentPerLeader; i++)
                {
                    GameObject newAgent = Instantiate(_agentPrefab, new Vector3 (spawnPos.x, 2.5f, spawnPos.z), Quaternion.identity);
                    newAgent.GetComponent<Follower>().ActiveCharacter(_leader);
                }
                _timescape = _coolDown + Vector3.Distance(transform.position, _leader.gameObject.transform.position) / 100;
            }
        }
    }
}
