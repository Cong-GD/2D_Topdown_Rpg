using System;
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private static readonly Quaternion leftRotation = Quaternion.Euler(0, 180, 0);
    private static readonly Quaternion rightRotation = Quaternion.Euler(0, 0, 0);

    [Min(0)]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private Transform rotateTransform;

    public event Action OnStartMoving;
    public event Action OnStopMoving;

    private Vector2 _moveDirect = Vector2.zero;
    private int _blockElement;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set 
        { 
            _moveSpeed = Mathf.Max(0, value); 
            CheckForMovingEvent();
        }
    }
    public Vector2 MoveDirect 
    {
        get => _moveDirect;
        set
        {   
            if(value.x != 0)
            {
                IsFacingRight = value.x > 0;
                rotateTransform.rotation = IsFacingRight ? rightRotation : leftRotation;
            }
            _moveDirect = value.normalized;
            CheckForMovingEvent();
        }
    }

    public bool BlockMovement 
    {
        get => _blockElement > 0;
        set
        {
            if (value)
            {
                _blockElement++;
            }
            else if(_blockElement > 0)
            {
                _blockElement--;
            }
            CheckForMovingEvent();
        }
    }

    public bool IsMoving { get; private set; }

    public bool IsFacingRight { get; private set; }

    private void OnEnable()
    {
        CheckForMovingEvent();
    }

    private void FixedUpdate()
    {
        if (IsMoving)
        {     
            rigidbody2d.velocity = MoveDirect * MoveSpeed;
        }
        else
        {
            rigidbody2d.velocity = Vector2.zero;
        }
    }

    public void ClearState()
    {
        _blockElement = 0;
        _moveDirect = Vector2.zero;
        StopAllCoroutines();
        CheckForMovingEvent();
    }

    public void Block(float blockTime)
    {
        StartCoroutine(BlockMovementCoroutine(blockTime));
    }    

    private void CheckForMovingEvent()
    {
        var isMoving = CheckForMoving();

        if (IsMoving == isMoving)
            return;

        IsMoving = isMoving;
        if (isMoving)
        {
            OnStartMoving?.Invoke();
        }
        else
        {
            OnStopMoving?.Invoke();
        }
    }
    private bool CheckForMoving()
    {
        return !BlockMovement && MoveSpeed > Mathf.Epsilon && !MoveDirect.Equals(Vector2.zero);
    }

    private IEnumerator BlockMovementCoroutine(float blockTime)
    {
        BlockMovement = true;
        yield return blockTime.Wait();
        BlockMovement = false;
    }
}
