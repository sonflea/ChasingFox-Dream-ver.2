using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;


public class JumpState
{
    public float jumpPower = 0;
    public bool isJump = false;
    public float jumpStartTime = 0;
    public float jumpHight;
    public State jumptype;

    public enum State { IDLE, NORMAL_JUMP, LONG_JUMP };
}

public class ControllerScript : MonoBehaviour
{
    Rigidbody2D rg2d;

    JumpState jumpState = new JumpState();



    public float jumpForce = 5f;
    public float jumpDuration = 3f;
    public bool isJumping = false;



    float jumpStartTime;

    private void Awake()
    {

    }

    void Start()
    {
        
        rg2d = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        //Debug.Log("fixedUpdate");
        if (jumpState.isJump)
        {
            //Jump();
            switch (jumpState.jumptype)
            {
                case JumpState.State.IDLE:
                    Debug.Log("일반");
                    break;
                case JumpState.State.NORMAL_JUMP:
                    Debug.Log("점프");
                    //Jump();
                    break;
                case JumpState.State.LONG_JUMP:
                    Debug.Log("긴 점프");
                    Debug.Log("슈퍼 점프");
                    JumpHigher();
                    break;
        
            }
         
        }
    }

    private void Update()
    {

        //jumpState.jumptype=JumpState.State.LONG_JUMP;

        Debug.Log(jumpState.jumptype);
        //isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
        //Debug.Log("update");

        if (Input.GetKeyDown(KeyCode.W)){
            Debug.Log("W");
            jumpState.jumpHight= jumpForce;
            jumpState.jumpStartTime=Time.time;
            Jump();
            jumpState.isJump = true;
            jumpState.jumptype= JumpState.State.NORMAL_JUMP;
            
        }
        
        else if (Input.GetKey(KeyCode.W)&&jumpState.isJump&&Time.time- jumpState.jumpStartTime < jumpDuration) {
            Debug.Log("HOLDDDDDDDDDDDDD");
            jumpState.jumptype = JumpState.State.LONG_JUMP;

            //jumpState.jumpHight = jumpForce * Time.deltaTime;
            //JumpHigher();
        }
        else
        {
            Debug.Log("else");
            jumpState.jumptype = JumpState.State.IDLE;
        }

        //Debug.Log("jump start" + (Time.time - jumpState.jumpStartTime));
        if (Input.GetKeyUp(KeyCode.W))
        {
            Debug.Log("UPPPPP");
            jumpState.isJump = false;
            jumpState.jumptype = JumpState.State.IDLE;
            //jumpState.jumpStartTime = 0;
        }


        Debug.Log("jumpstart" + jumpState.jumpStartTime);
        Debug.Log("jumptype"+ jumpState.jumptype);

    }

    private void Jump()
    {
        Debug.Log("nomal");
        Debug.Log("jump Hight "+jumpState.jumpHight);
        rg2d.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        //isJumping = true;
        //jumpStartTime = Time.time;
    }

    private void JumpHigher()
    {
        Debug.Log("higer");
        rg2d.AddForce(Vector3.up * jumpForce * Time.deltaTime*2, ForceMode2D.Impulse);//추가적인 힘을 계속 주는것 time.deltatime은 아주작은 소수점이 나올것이므로 곱하게 되면 값이 아주작아짐
    }
}