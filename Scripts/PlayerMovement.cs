using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private const string animatorRun = "Run";

    [SerializeField] private float _speedRotation = 1;
    [SerializeField] private float _speedMove = 1;
    [SerializeField] private Animator _animator;

    private Vector3 _mousePressDownPos;
    private Vector3 _mousePositionOnDrag;
    private Rigidbody _rigidbody;
    private Touch _touch;
    private bool _enableDrag;
    private bool _move;
    

    [HideInInspector] public static Action<bool> StartGame;

    public float SpeedMove
    {
        get
        {
            return _speedMove;
        }
    }

    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        StartGame += SetDrag;
    }

    void FixedUpdate()
    {
        Rotate();
        Move();
    }

#if UNITY_EDITOR
    private void OnMouseDown()
    {
        if (!_enableDrag) return;

        _move = true;
        _mousePressDownPos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        if (!_enableDrag) return;

        _mousePositionOnDrag = _mousePressDownPos - Input.mousePosition;
        float forceX = 100 * _mousePositionOnDrag.x / Screen.width;
        float forceY = 100 * _mousePositionOnDrag.y / Screen.height;
        Vector3 direction = new Vector3(forceX, 0, forceY).normalized;
        Quaternion rotation = Quaternion.AngleAxis(transform.rotation.y + Vector3.SignedAngle(Vector3.forward, -direction + Vector3.forward * 0.001f, Vector3.up), Vector3.up);
        _rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _speedRotation));
    }
#endif


    private void Rotate()
    {
        if (Input.touchCount > 0 && _enableDrag)
        {
            _move = true;

            _touch = Input.GetTouch(0);

            if (Input.touchCount > 0)
            {
                if (_touch.phase == TouchPhase.Began)
                {
                    _mousePressDownPos = Input.mousePosition;
                }
                if (_touch.phase == TouchPhase.Moved)
                {
                    _mousePositionOnDrag = _mousePressDownPos - Input.mousePosition;
                    float forceX = 100 * _mousePositionOnDrag.x / Screen.width;
                    float forceY = 100 * _mousePositionOnDrag.y / Screen.height;
                    Vector3 direction = new Vector3(forceX, 0, forceY).normalized;
                    Quaternion rotation = Quaternion.AngleAxis(transform.rotation.y + Vector3.SignedAngle(Vector3.forward, -direction + Vector3.forward * 0.001f, Vector3.up), Vector3.up);

                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _speedRotation);
                }
            }
        }
    }
    
    private void Move()
    {
        if (_move)
        {
            if (!_animator.GetBool(animatorRun))
            {
                _animator.SetBool(animatorRun, true);
            }
            _rigidbody.MovePosition(_rigidbody.position + transform.forward * _speedMove * Time.deltaTime);
        }
    }

    public void SetDrag(bool drag)
    {
        _enableDrag = drag;
    }
}
