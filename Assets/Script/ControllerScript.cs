using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;



public class Charactor
{
    public float speed;
    public bool isHuman = true;

    public virtual void Die(Charactor charactor)
    {

    }

    public virtual void Setspeed() { }
}
public class WereWolf:Charactor
{
    public int life = 2;
    private static WereWolf instance;

    public static WereWolf Instance()
    {
            if (instance == null)
            {
                //Debug.Log("비었음 생성");
                instance = new WereWolf();
            }
            else
            {
                //Debug.Log("안 비었기때문에 기존거 전달");
            }
            return instance;
    }

    private WereWolf()
    {
        //if (instance == null) Debug.Log("늑대");
    }

    public override void Setspeed()
    {
        Debug.Log("늑대 속도");
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
    private static Human instance;

    private Human() { }
    public static Human Instance()
    {
        if(instance == null)
        {
            return instance = new Human();
        }
        else
        {
            return instance;
        }
    }
    public override void Setspeed()
    {
        Debug.Log("사람 속도");
        this.speed = 3.0f;
    }
}


public class JumpState//굳이 클래스여야할까
{
    public float jumpPower = 0;//점프 힘
    public bool isJump = false;//아래랑 같음ㄴ
    public float jumpStartTime = 0;
    public float jumpHight;
    public State jumptype;

    public enum State { IDLE, NORMAL_JUMP, LONG_JUMP };
}

public class ControllerScript : MonoBehaviour
{
    Rigidbody2D rg2d;//이번 프로젝트에서는 rigdbody를 이용해서 움직일것임

    JumpState jumpState = new JumpState();
    Charactor charactor = new Charactor();


    



    public float jumpForce = 5f;//점프 힘
    public float jumpDuration = 0.5f;//긴 점프 할 수 있는 시간
    public bool isJumping = false;//다른곳에서 사용 될 수도 있지만 핵심은 오래누르는 긴 점프를위함 점프 클래스가 있어서 굳이 싶기도 함
    [SerializeField]
    private bool isMoving = false;//update에서 fixedUpdate로 움직임 전해주기 위함

    public float moveSpeed = 1;
    private float InxPos;

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
        //else
        //{
        //    rg2d.velocity = new Vector2(0, rg2d.velocity.y);//키 입력 풀릴경우 0으로 만들기 위함
        //}


    }

    private void Move()
    {
        Vector2 v2;
        //   Vector3 v3 = new Vector3(InxPos, rg2d.velocity.y, 0);
        if (Mathf.Sign(InxPos) != Mathf.Sign(rg2d.velocity.x))
        {
            v2 = new Vector2(0, rg2d.velocity.y);
        }
         v2 = new Vector2(InxPos, rg2d.velocity.y);
        
        rg2d.velocity = v2;
        
        Debug.Log(v2+"나는 움직임~~");
    }

    private void Attack()
    {
        Debug.Log("Attack");
    }

    private void Formchange()
    {
        //if(charactor is WereWolf)//아래와 동일
        //{
        //
        //}
        //else
        //{
        //
        //}
        if (charactor.isHuman)//사람일때 변신
        {
            charactor = WereWolf.Instance();
            charactor.isHuman = false;
            Debug.Log("늑대로 변경");
        }
        else//늑대일때 변신
        {
            charactor = Human.Instance();
            charactor.isHuman = true;
            Debug.Log("사람으로 변경");
        }
        charactor.Setspeed();
        moveSpeed = charactor.speed;

    }

    private void InputManager()
    {
        if (Input.GetMouseButtonDown(0))//좌클릭
        {
            Attack();
        }
        if(Input.GetMouseButtonDown(1))//우클릭
        {
            Formchange();
        }
        if (Input.GetKeyDown(KeyCode.W))//점프키 관련 만약 키가 변한다면 keycode만 변경하면 됨
        {//"W"가 점프라고 생각했을때 구현내용
            Jump();

        }
        else if (Input.GetKey(KeyCode.W) && jumpState.isJump && Time.time - jumpState.jumpStartTime < jumpDuration)
        {
            Debug.Log("HOLDDDDDDDDDDDDD");
            jumpState.jumptype = JumpState.State.LONG_JUMP;
        }
        else
        {
            //Debug.Log("else");
            jumpState.isJump = false;
            jumpState.jumptype = JumpState.State.IDLE;

        }
        //if (Input.GetKeyUp(KeyCode.W))//큰차이 없는거 같아서 일단 주석처리함 위에 else부분과 겹치는 부분 조작 부분에서 차이가 난다고 한다면 수정 필요 아마 수정 필요없을듯
        //{
        //    jumpState.isJump = false;
        //    jumpState.jumptype = JumpState.State.IDLE;
        //}
        //Debug.Log("InxPos value" + InxPos);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            InxPos = Input.GetAxis("Horizontal") * moveSpeed;
            isMoving = true;

            Debug.Log("좌우");
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            InxPos = 0;
            isMoving = false;

            Debug.Log("좌우 입력값 " + InxPos);
        }
    }

    private void Update()
    {
        InputManager();
    }

    private void Jump()
    {
        //Debug.Log("nomal");
        //Debug.Log("jump Hight "+jumpState.jumpHight);
        jumpState.jumpHight = jumpForce;
        jumpState.jumpStartTime = Time.time;
        rg2d.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        jumpState.isJump = true;
        jumpState.jumptype = JumpState.State.NORMAL_JUMP;
    }

    private void JumpHigher()
    {
        Debug.Log("higer");
        rg2d.AddForce(Vector3.up * jumpForce * Time.deltaTime*2, ForceMode2D.Impulse);//추가적인 힘을 계속 주는것 time.deltatime은 아주작은 소수점이 나올것이므로 곱하게 되면 값이 아주작아짐
    }
}