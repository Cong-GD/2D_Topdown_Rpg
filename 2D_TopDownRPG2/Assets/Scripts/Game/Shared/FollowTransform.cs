using System.Collections;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{

    [SerializeField]
    private Transform followTransfrom;

    [SerializeField]
    private bool autoOffset;

    [SerializeField]
    private Vector3 offset;

    private void OnEnable()
    {
        StartCoroutine(FollowCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator FollowCoroutine()
    {
        if (transform != null && autoOffset)
        {
            offset = transform.position - followTransfrom.position;
        }

        while (followTransfrom != null)
        {
            transform.position = followTransfrom.position + offset;
            yield return CoroutineHelper.endOfFrameWait;
        }
    }
}
