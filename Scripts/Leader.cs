using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour
{
    private const string playerTag = "Player";

    public event OnSetFollower onSetFollowers;
    public event OnPlayerAddFollower onPlayerAddFollower;
    public event OnDestroyLeader onDestroy;

    public delegate void OnSetFollower(int followers);
    public delegate void OnPlayerAddFollower();
    public delegate void OnDestroyLeader();

    [HideInInspector] public Indicator Indicator;

    [SerializeField] private SkinnedMeshRenderer _leaderSkin;

    private int _followers = 1;

    public void Init(Color color)
    {
        _leaderSkin.material.color = color;
        CaptureCharacter capture = GetComponent<CaptureCharacter>();
        capture.SetLeader(this);
    }

    public void AddAgent()
    {
        _followers++;
        onSetFollowers?.Invoke(_followers);
        if(gameObject.tag == playerTag)
        {
            onPlayerAddFollower?.Invoke();
        }
        if(ScorePanel.Instance != null) ScorePanel.Instance.UpdateLeaderBoards();
    }

    public void MinusAgent()
    {
        _followers--;
        onSetFollowers?.Invoke(_followers);
        if (ScorePanel.Instance != null) ScorePanel.Instance.UpdateLeaderBoards();
    }

    public int AgentCount
    {
        get
        {
            return _followers;
        }
    }

    public Color LeaderColor
    {
        get
        {
            return _leaderSkin.material.color;
        }
    }

    private void OnDestroy()
    {
        if (gameObject.tag == playerTag)
        {
               StartAgain.Instance.OpenPanel();
        }
        OffScreenIndicator.TargetStateChanged?.Invoke(this, false);
        ScorePanel.Instance.UpdateLeaderBoards();
    }

    private void OnEnable()
    {
        OffScreenIndicator.TargetStateChanged?.Invoke(this, true);
    }
}
