using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    public static ScorePanel Instance;

    [SerializeField] private Leaderboard[] _leaderboards;

    private static int _playerFollowers;
    private Leader[] _leaders;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateLeaderBoards();
    }

    public void UpdateLeaderBoards()
    {
        _leaders = FindObjectsOfType<Leader>();

        if(!_leaders.Any())
        {
            return;
        }

        var sort = from s in _leaders orderby s.AgentCount descending select s;

        if (sort.Count() <= 1)
        {
            StartAgain.Instance.OpenPanel();
        }

        for (int i = 0; i < _leaderboards.Length; i++)
        {
            if (sort.Count() - 1 < i)
            {
                if (_leaderboards[i] != null)
                {
                    _leaderboards[i].Activate(false);
                }
                continue;
            }
            else
            {
                if (_leaderboards[i] != null)
                {
                    _leaderboards[i].Activate(true);
                }
            }
            if(sort.ElementAt(i).gameObject.tag == "Player")
            {
                _playerFollowers = sort.ElementAt(i).AgentCount;
            }
            _leaderboards[i].SetFollowers(i+1, sort.ElementAt(i).AgentCount);
            _leaderboards[i].SetImageColor(sort.ElementAt(i).LeaderColor);
        }
    }

    public static int PlayerScore
    {
        get
        {
            return _playerFollowers;
        }
    }

}
