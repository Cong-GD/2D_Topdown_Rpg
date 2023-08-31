using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D defaulfBoundary;

    private void Start()
    {
        SetToDefaultLevelBound();
    }

    public void SetConfinerCollider(PolygonCollider2D boundary)
    {
        CameraManager.Instance.SetConfiner(boundary);
    }

    public void SetToDefaultLevelBound()
    {
        SetConfinerCollider(defaulfBoundary);
    }

    public void SetBoundAfterMoveOffset(Vector3 offset, float moveTime, PolygonCollider2D boundary)
    {
        CameraManager.Instance.MoveOffet(offset, moveTime,
                () =>
                {
                    SetConfinerCollider(boundary);
                    CameraManager.Instance.MoveOffet(Vector3.zero, 1f);
                });
    }
}

