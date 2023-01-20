using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

//N.B.: "BJP" stands for "Big Jump Press"; When you press jump key longer to jump higher, just like in 2D Mario games
public class PlayerController : MonoBehaviour
{
    #region Parameters

    private Rigidbody2D _Rb2d;

    [Header("Move parameters")]
    [SerializeField] private float _MoveSpeed = 10f;


    [Header("Jump parameters")]
    [SerializeField] private KeyCode _JumpKey = KeyCode.Space;
    [SerializeField] private bool _CanJump = false;
    [SerializeField] private float _JumpForce = 9.81f;
    [SerializeField] private float _MaxJumpHold = 0.0f;
    [SerializeField] private float _BaseGravityForce = 9.81f;
    [SerializeField] private float _LowGravityForce = 3.81f;

    private float _CurrJumpHold = 0.0f;
    private bool _CanHoldCurrentJump = true;

    [SerializeField] private int _MaxJumpCount = 2;
    private int _CurrJumpCount = 0;


    [SerializeField] private bool _AwaitJumpReset = false;


    #endregion

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

    //Handle Movement inputs and player translation between -1 and 1 multiplied by speed
    void HandleMove()
    {
        float dir = Input.GetAxis("Horizontal");
        transform.Translate(new Vector2(dir, 0) * _MoveSpeed * Time.fixedDeltaTime);
    }

    void HandleJump()
    {
        //Lock jump when touching ground but jump key still pressed
        if (_AwaitJumpReset && !Input.GetKey(_JumpKey)) ResetJump();

        //Reset "BJP" when releasing jump key
        if (_CanJump && !_CanHoldCurrentJump && !Input.GetKey(_JumpKey))
            _CanHoldCurrentJump = true;

        if (Input.GetKey(_JumpKey) && _CanHoldCurrentJump)
        {
            _Rb2d.velocity = Vector2.up * _JumpForce;
            _CurrJumpHold += Time.deltaTime;
            
            if (_CurrJumpHold >= _MaxJumpHold)
            {
                _CurrJumpCount++;
                _CanHoldCurrentJump = false;
                _CurrJumpHold = 0.0f;
            }
            
        }

        if (Input.GetKeyUp(_JumpKey) && _CanJump)
        {
            _CurrJumpCount++;
            _CanHoldCurrentJump = true;
            _CurrJumpHold = 0.0f;
        }

        if (_Rb2d.velocity.y >= 0 && _CanJump)
        {
            _Rb2d.velocity -= Vector2.up * _LowGravityForce * Time.fixedDeltaTime;

        }
        else if (_Rb2d.velocity.y < 0 || !_CanJump)
        {
            _Rb2d.velocity -= Vector2.up * _BaseGravityForce * Time.deltaTime;
        }
    }

    void ResetJump()
    {
        _CanJump = true;
        _CanHoldCurrentJump = true;
        _CurrJumpHold = 0.0f;
        _AwaitJumpReset = false;
        _CurrJumpCount = 0;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!Input.GetKey(_JumpKey)) ResetJump();
        else _AwaitJumpReset = true;
    }
}
