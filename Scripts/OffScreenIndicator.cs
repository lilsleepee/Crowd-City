using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicator : MonoBehaviour
{
    [HideInInspector] public static Action<Leader, bool> TargetStateChanged;

    [Range(0.5f, 0.9f)]
    [SerializeField] private float _screenBoundOffset = 0.9f;

    [Range(0.1f, 0.9f)]
    [SerializeField] private float _targetOffset = 0.9f;

    [SerializeField] private Camera _camera;

    private List<Leader> _targets = new List<Leader>();
    private Vector3 _screenCentre;
    private Vector3 _screenBounds;

    private void Start()
    {
        _screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
        _screenBounds = _screenCentre * _screenBoundOffset;
        _camera = Camera.main;
        TargetStateChanged += HandleTargetStateChanged;
    }


    private void HandleTargetStateChanged(Leader target, bool active)
    {
        if (active)
        {
            _targets.Add(target);

        }
        else
        {
            target.Indicator?.Activate(false);
            target.Indicator = null;
            _targets.Remove(target);
        }
    }

    void LateUpdate()
    {
        DrawIndicators();
    }

    void DrawIndicators()
    {
        foreach (Leader target in _targets)
        {
            Vector3 screenPosition = GetScreenPosition(_camera, target.transform.position);
            bool isTargetVisible = IsTargetVisible(screenPosition);
            Indicator indicator = GetIndicator(target);
            float angle = float.MinValue;

            if (indicator)
            {
                if (isTargetVisible)
                {
                    screenPosition.y += Screen.height / 2 * _targetOffset;
                    indicator.SetArrowRotation(Quaternion.Euler(0, 0, 0));
                }

                else
                {
                    GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, _screenCentre, _screenBounds);
                    indicator.SetArrowRotation(Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + 90));
                }
                indicator.transform.position = screenPosition;
            }
        }
    }

    public static void GetArrowIndicatorPositionAndAngle(ref Vector3 screenPosition, ref float angle, Vector3 screenCentre, Vector3 screenBounds)
    {
        screenPosition -= screenCentre;

        if (screenPosition.z < 0)
        {
            screenPosition *= -1;
        }

        angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
        float slope = Mathf.Tan(angle);

        if (screenPosition.x > 0)
        {
            screenPosition = new Vector3(screenBounds.x, screenBounds.x * slope, 0);
        }
        else
        {
            screenPosition = new Vector3(-screenBounds.x, -screenBounds.x * slope, 0);
        }
        if (screenPosition.y > screenBounds.y)
        {
            screenPosition = new Vector3(screenBounds.y / slope, screenBounds.y, 0);
        }
        else if (screenPosition.y < -screenBounds.y)
        {
            screenPosition = new Vector3(-screenBounds.y / slope, -screenBounds.y, 0);
        }
        screenPosition += screenCentre;
    }

    public static Vector3 GetScreenPosition(Camera mainCamera, Vector3 targetPosition)
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
        return screenPosition;
    }

    public static bool IsTargetVisible(Vector3 screenPosition)
    {
        bool isTargetVisible = screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height;
        return isTargetVisible;
    }

    private Indicator GetIndicator(Leader target)
    {
        if(target.Indicator == null)
        {
            target.Indicator = IndicatorPool.Instance.GetPooledObject();
            target.Indicator.Activate(true);
            target.Indicator.SetImageColor(target.LeaderColor);
            target.Indicator.SetArrowColor(target.LeaderColor);
            target.onSetFollowers += target.Indicator.SetFollowers;
        }
        
        return target.Indicator;
    }
}
