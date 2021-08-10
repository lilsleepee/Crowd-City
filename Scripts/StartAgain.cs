using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartAgain : MonoBehaviour
{
    public static StartAgain Instance;

    [SerializeField] GameObject _panel;

    private void Awake()
    {
        Instance = this;
        Timer.onEndTime += OpenPanel;
    }

    public void OpenPanel()
    {
        _panel.SetActive(true);
    }
}
