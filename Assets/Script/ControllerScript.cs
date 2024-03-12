using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;



public class Charactor
{
    public float speed;

    public virtual void Die(Charactor charactor)
    {

    }
}
public class WereWolf:Charactor
{

    void Setspeed()
    {
        this.speed = 5.0f;
    }
    float Getspeed()
    {
        return this.speed;
    }
}

public class Human: Charactor
{
    int ammo = 0;
    void Setspeed()
    {
        this.speed = 3.0f;
    }
}

public class JumpState//굳이 클래스여야할까
{
    public float jumpPower = 0;//점프 힘
    public bool isJump = false;//아래랑 같음
    public float jumpStartTime = 0;
    public float jumpHight;
    public State jumptype;

    public enum State { IDLE, NORMAL_JUMP, LONG_JUMP };
}

public class ControllerScript : MonoBehaviour
{
    Rigidbody2D rg2d;//이번 프로젝트에서는 rigdbody를 이용해서 움직일것임

    JumpState jumpState = new JumpState();



    public float jumpForce = 5f;//점프 힘
    public float jumpDuration = 3f;//긴 점프 할 수 있는 시간
    public bool isJumping = false;//다른곳에서 사용 될 수도 있지만 핵심은 오래누르는 긴 점프를위함 점프 클래스가 있어서 굳이 싶기도 함

    private bool isMoving = false;//update에서 fixedUpdate로 움직임 전해주기 위함

    private float InxPos,InyPos;

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

        if (isMoving)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 v3 = new Vector3(InxPos, rg2d.velocity.y, 0);
        
        rg2d.velocity = v3;
        Debug.Log(v3+"나는 움직임~~");
    }

    private void Update()
    {

        Debug.Log(jumpState.jumptype);

        if (Input.GetKeyDown(KeyCode.W)){//"W"가 점프라고 생각했을때 구현내용
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
        }
        else
        {
            Debug.Log("else");
            jumpState.jumptype = JumpState.State.IDLE;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            Debug.Log("UPPPPP");
            jumpState.isJump = false;
            jumpState.jumptype = JumpState.State.IDLE;
        }
        InxPos = Input.GetAxis("Horizontal") * 10;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            isMoving = true;
            
            Debug.Log("좌우");
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        { 
            isMoving= false;
            Debug.Log("좌우 입력값 " + InxPos);
        }
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