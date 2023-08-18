using UnityEngine;

public class BackgroundMoving : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private Collider2D commonCollider;

    private float endPositionX;
    private float startPositionX;

    private void Start()
    {
        endPositionX = commonCollider.bounds.center.x - commonCollider.bounds.size.x;
        startPositionX = commonCollider.bounds.center.x + commonCollider.bounds.size.x;
    }

    private void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector2.left);
        if (transform.position.x < endPositionX)
        {
            transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
        }
    }
}
