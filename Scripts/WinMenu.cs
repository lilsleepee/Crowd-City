using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField] float _textAnimationSpeed = 0.1f;

    void OnEnable()
    {
        IndicatorPool.Instance.DeactivateAllPooledObjects();
        StartCoroutine(CoinAmountTextAnimation());
        PlayerMovement.StartGame.Invoke(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator CoinAmountTextAnimation()
    {
        int step = 1;
        if (ScorePanel.PlayerScore > 10)
        {
            step = ScorePanel.PlayerScore / 10;
        }
        for (int i = 0; i <= ScorePanel.PlayerScore; i += step)
        {
            _text.text = i.ToString();
            yield return new WaitForSeconds(_textAnimationSpeed);
        }
        _text.text = ScorePanel.PlayerScore.ToString();
    }
}
