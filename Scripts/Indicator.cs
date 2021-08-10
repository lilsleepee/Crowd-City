using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    [SerializeField] private Image _arrow;
    [SerializeField] private Text _text;
    private Image _indicatorImage;

    public bool Active
    {
        get
        {
            return transform.gameObject.activeInHierarchy;
        }
    }

    void Awake()
    {
        _indicatorImage = transform.GetComponent<Image>();
    }

    public void SetImageColor(Color color)
    {
        _indicatorImage.color = color;
    }

    public void SetArrowColor(Color color)
    {
        _arrow.color = color;
    }

    public void SetArrowRotation(Quaternion rotation)
    {
        _arrow.rectTransform.rotation = rotation;
    }

    public void SetFollowers(int followers)
    {
        _text.text = followers.ToString();
    }

    public void Activate(bool value)
    {
        gameObject.SetActive(value);
    }
}
