using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private Text _text;
    private Image _image;

    public bool Active
    {
        get
        {
            return transform.gameObject.activeInHierarchy;
        }
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetImageColor(Color color)
    {
        _image.color = color;
    }

    public void SetFollowers(int number, int followers)
    {
        _text.text = number.ToString() + "# " + followers.ToString();
    }
    public void Activate(bool value)
    {
        gameObject.SetActive(value);
    }
}
