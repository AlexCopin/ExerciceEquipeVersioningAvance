using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D _Rb2d;

    [Header("Move parameters")]
    [SerializeField] private float _MoveSpeed = 10f;


    [Header("Jump parameters")]
    [SerializeField] private bool _IsJumping = false;
    [SerializeField] private float _JumpForce = 9.81f;
    [SerializeField] private float _MaxJumpHold = 0.0f;
    [SerializeField] private float _BaseGravityForce = 9.81f;
    [SerializeField] private float _LowGravityForce = 3.81f;

    private float _CurrJumpHold = 0.0f;
    private bool _CanHoldJump = true;

    [SerializeField] private bool _AwaitJumpReset = false;
    

    void Awake()
    {
        _Rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }


    void FixedUpdate()
    {
        HandleMove();
        HandleJump();
    }

    void HandleMove()
    {
        float dir = Input.GetAxis("Horizontal");
        transform.Translate(new Vector2(dir, 0) * _MoveSpeed * Time.fixedDeltaTime);
    }

    void HandleJump()
    {
        if (_AwaitJumpReset && !Input.GetKey(KeyCode.Space)) ResetJump();

        if (!Input.GetKey(KeyCode.Space) && _Rb2d.velocity.y != 0 && _CanHoldJump)
        {
            _IsJumping = true;
            _CanHoldJump = false;
        }

        if (Input.GetKey(KeyCode.Space) && _CanHoldJump)
        {
            _Rb2d.velocity = Vector2.up * _JumpForce;
            _CurrJumpHold += Time.deltaTime;
            if (_CurrJumpHold >= _MaxJumpHold)
            {
                _IsJumping = true;
                _CanHoldJump = false;
            }
        }

        if (_Rb2d.velocity.y >= 0 && _IsJumping)
        {
            _Rb2d.velocity -= Vector2.up * _LowGravityForce * Time.fixedDeltaTime;

        }
        else if (_Rb2d.velocity.y < 0)
        {
            _Rb2d.velocity -= Vector2.up * _BaseGravityForce * Time.deltaTime;
        }
    }

    void ResetJump()
    {
        _IsJumping = false;
        _CanHoldJump = true;
        _CurrJumpHold = 0.0f;
        _AwaitJumpReset = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!Input.GetKey(KeyCode.Space)) ResetJump();
        else _AwaitJumpReset = true;
    }
}
