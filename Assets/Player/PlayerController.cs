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
    [SerializeField] private bool _CanJump = true;
    [SerializeField] private float _JumpForce = 9.81f;
    [SerializeField] private float _MaxJumpHold = 0.0f;
    [SerializeField] private float _BaseGravityForce = 9.81f;
    [SerializeField] private float _LowGravityForce = 3.81f;

    private float _CurrJumpHold = 0.0f;
    [SerializeField] private bool _CanHoldCurrentJump = true;
    private bool _IsHoldingJump = false;
    private bool _IsReleasingJump = false;

    [SerializeField] private int _MaxJumpCount = 2;
    private int _CurrJumpCount = 0;


    [SerializeField] private bool _AwaitJumpReset = false;

    [SerializeField] private float _CoyoteMaxTime = 0.3f;
    private float _CoyoteCurrTime = 0.0f;



    #endregion

    void Awake()
    {
        _Rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        HandleJumpInput();
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

    void HandleJumpInput()
    {
        if (Input.GetKeyDown(_JumpKey) && _CanJump)
        {
            _CurrJumpCount++;
            if (_CurrJumpCount >= _MaxJumpCount)
            {
                _CanJump = false;
            }
        }

        _IsHoldingJump = Input.GetKey(_JumpKey);
        _IsReleasingJump = Input.GetKeyUp(_JumpKey);
    }

    void HandleJump()
    {
        //Lock jump when touching ground but jump key still pressed
        if (_AwaitJumpReset && !_IsHoldingJump) ResetJump();

        //Reset "BJP" when releasing jump key
        if (_CanJump && !_CanHoldCurrentJump && !_IsHoldingJump)
        {
            _CanHoldCurrentJump = true;
            _CurrJumpHold = 0.0f;
        }

        //Handling "BJP"
        if (_IsHoldingJump && _CanHoldCurrentJump)
        {
            _Rb2d.velocity = Vector2.up * _JumpForce;

            _CurrJumpHold += Time.deltaTime;
            if (_CurrJumpHold >= _MaxJumpHold)
            {
                _CanHoldCurrentJump = false;
                _CurrJumpHold = 0.0f;
            }
            
        }
        
        
        if (_IsReleasingJump && _CanJump)
        {
            _CanHoldCurrentJump = true;
            _CurrJumpHold = 0.0f;
        }
        else if (!_IsHoldingJump && !_CanJump)
        {
            _CanHoldCurrentJump = false;
        }

        //Gravity force applied on jump based on "BJP"
        if (_Rb2d.velocity.y >= 0)
        {
            if (!_IsHoldingJump || !_CanHoldCurrentJump)
                _Rb2d.velocity -= Vector2.up * _LowGravityForce * Time.fixedDeltaTime;
            else
                _Rb2d.velocity -= Vector2.up * _BaseGravityForce * Time.fixedDeltaTime;

        }

        else if (_Rb2d.velocity.y < 0)
        {
            //Handling Coyot time: Allowing player to jump a short time after leaving plateform, avoiding to force jump precision
            if (_CurrJumpCount == 0)
            {

                _CoyoteCurrTime += Time.deltaTime;
                if (_CoyoteCurrTime >= _CoyoteMaxTime)
                {
                    _CanJump = false;
                    _CanHoldCurrentJump = false;
                }

            }

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
        _CoyoteCurrTime = 0.0f;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!Input.GetKey(_JumpKey)) ResetJump();
        else _AwaitJumpReset = true;
    }
}
