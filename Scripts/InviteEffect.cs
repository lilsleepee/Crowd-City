using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InviteEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _circle;
    [SerializeField] private ParticleSystemRenderer _particle;

    public void SetColor(Color color)
    {
        for (int i = 0; i < _circle.Length; i++)
        {
            _circle[i].color = color;
        }
        _particle.material.color = color;
    }

    public void DestroyEffect()
    {
        gameObject.SetActive(false);
    }
}
