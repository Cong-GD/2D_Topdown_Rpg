using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class CameraManager : GlobalReference<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera virtureCamera;
    [SerializeField] private CinemachineConfiner2D confiner;
    [SerializeField] private Camera mainCam;

    private CinemachineFramingTransposer _transposer;
    private Tweener _tweener;

    public static Camera MainCam => Instance.mainCam;

    protected override void Awake()
    {
        base.Awake();
        _transposer = virtureCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
    public void SetConfiner(PolygonCollider2D boundary)
    {
        confiner.InvalidateCache();
        confiner.m_BoundingShape2D = boundary;
        //StartCoroutine(SmoothComeBackToPlayer());
    }

    public IEnumerator SmoothComeBackToPlayer()
    {
        const float comebackTime = 2f;
        var followtranform = virtureCamera.Follow;
        var currentPosition = followtranform.position;
        var endTime = Time.time + comebackTime;
        while(true)
        {
            float remainTime = endTime - Time.time;
            if(remainTime <= Mathf.Epsilon)
            {
                yield break;
            }
            float distanceToTarget = Vector3.Distance(currentPosition, followtranform.position);
            if(distanceToTarget <= Mathf.Epsilon)
            {
                yield break;
            }
            float speedNeedToReach = distanceToTarget / remainTime;
            currentPosition = Vector3.MoveTowards(currentPosition, followtranform.position, speedNeedToReach);
            virtureCamera.ForceCameraPosition(currentPosition, Quaternion.identity);
            yield return null;
        }
    }

    public void MoveOffet(Vector3 offset, float speed, Action onComplete = null, Action onKill = null)
    {
        _tweener?.Kill();
        _tweener = DOVirtual.Vector3(_transposer.m_TrackedObjectOffset, offset, speed, (value) => _transposer.m_TrackedObjectOffset = value)
            .SetEase(Ease.Linear)
            .OnComplete(() => onComplete?.Invoke())
            .OnKill(() => onKill?.Invoke());
    }
}