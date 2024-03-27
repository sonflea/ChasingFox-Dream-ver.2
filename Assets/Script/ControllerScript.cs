using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static PlatformScript;




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

    public class Charactor
    {
        public float speed;
        public bool isHuman = true;
        public int hidePos = 0;


        public virtual void Die(Charactor charactor)
        {

        }

        public virtual void Setspeed() { }

        public virtual void Attack() { }
        public void Crouch(GameObject guard)
        {
            Debug.Log("크라우치");
            guard.SetActive(true);

            if (hidePos < 0)
            {

            }

        }//늑대와 인간 둘다 웅크림이 같다고 가정하기에 그냥 일반함수로 구현
    }
    public class WereWolf : Charactor
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
        public override void Attack()
        {
            Debug.Log("늑대 공격");
        }

        float Getspeed()
        {
            return this.speed;
        }
    }

    public class Human : Charactor
    {
        int ammo = 0;
        private static Human instance;

        private Human() { }
        public static Human Instance()
        {
            if (instance == null)
            {
                return instance = new Human();
            }
            else
            {
                return instance;
            }
        }
        public override void Attack()
        {
            Debug.Log("사람 공격");
        }
        public override void Setspeed()
        {
            Debug.Log("사람 속도");
            this.speed = 3.0f;
        }
    }

    private static ControllerScript instance=null;
    public static ControllerScript Instance
    {
        get
        {
            if (instance == null) return null;
            return instance;
        }
    }

    private ControllerScript() { }




    Rigidbody2D rg2d;//이번 프로젝트에서는 rigdbody를 이용해서 움직일것임

    JumpState jumpState = new JumpState();
    Charactor charactor = new Charactor();


    public float jumpForce = 5f;//점프 힘
    public float jumpDuration = 0.5f;//긴 점프 할 수 있는 시간
    [SerializeField]
    private bool isMoving = false;//update에서 fixedUpdate로 움직임 전해주기 위함
    [SerializeField]
    private bool isGround = true;
    public bool isHide = false;//크라우치 할 수 있는지 확인 용변수
    public bool isCrouching = false;//크라우치 중인지 확인뇽

    [SerializeField] private GameObject currentOneWayPlatform;
    [SerializeField] private BoxCollider2D playerCollider;
    //[SerializeField] BoxCollider2D platformCollider;//현재 안씀

    public float downTime = 0.4f;//다운 점프하면 오브젝트 충돌 무시하는 시간
    public bool canDown = false;//다운 점프 가능 구간 확인
    //private bool dJump = false;//다운 점프 현재 안씀



    public float moveSpeed = 5;
    private float InxPos;

    Vector2 vec;//레이 위한 것

    [SerializeField] private GameObject guard;

    private void Awake()
    {
        vec = Vector2.left;//캐릭터 방향키 입력에 따른 레이 변경을 위해
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        StartCoroutine(DownJump());
    }

    void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "platform")//지면 확인 점프용
        {
            isGround = true;
            if (collision.gameObject.tag == "platform")
            {
                currentOneWayPlatform = collision.gameObject;
                //platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
                Debug.Log(collision.gameObject.GetComponent<PlatformScript>().dObject);//다운 오브젝트 타입확인용 로그
                switch (collision.gameObject.GetComponent<PlatformScript>().dObject)//대각선 직선 오브젝트 마다 떨어지는 시간이 다를수도 있으니
                {
                    case downJumpObject.STRAIGHT://직선
                        downTime = 0.4f;
                        break;
                    case downJumpObject.DIAGONAL://대각선
                        downTime = 0.6f;
                        break;
                }
                //canDown = true;
            }
        }

        if (collision.gameObject.tag == "cover")//엄페물 확인용 엄페용
        {
            Debug.Log("엄페물");
        }

        if (collision.gameObject.tag == "bullet")
        {
            Debug.Log("collision hit");
            Vector2 pos = collision.GetContact(0).point;




            float posCheck = Mathf.Sign(transform.position.x - pos.x);
            string leftright = "";
            leftright = (posCheck > 0) ? "좌" : "우";
            Debug.Log($"충돌 위치 : {pos} 위치 차이 {posCheck} {leftright} 에서 피격");

        }

    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "platform")
        {
            isGround = false;
            if (collision.gameObject.tag == "platform")
            {
                currentOneWayPlatform = null;
                //canDown = false;


            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "platform")
        {
            isGround = true;
            if (collision.gameObject.tag == "platform")
            {
                currentOneWayPlatform = collision.gameObject;
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("트리거");

        if (collision.gameObject.tag == "cover")
        {
            Debug.Log($"경계 {collision.bounds.min.x}");
            charactor.hidePos= coverDir(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "cover")
        {
            charactor.hidePos = coverDir(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "cover")
        {
            Debug.Log("사라짐");
            isHide = false;
            guard.SetActive(false);
        }
    }

    private void FixedUpdate()
    {

        if (jumpState.isJump)
        {
            switch (jumpState.jumptype)
            {
                case JumpState.State.IDLE:
                    //Debug.Log("일반");
                    break;
                case JumpState.State.NORMAL_JUMP:
                    //Debug.Log("점프");
                    //Jump();
                    break;
                case JumpState.State.LONG_JUMP:
                    //Debug.Log("긴 점프");
                    //Debug.Log("슈퍼 점프");
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
        Vector2 v2;
        //if (Mathf.Sign(InxPos) != Mathf.Sign(rg2d.velocity.x))
        //{
        //    v2 = new Vector2(0, rg2d.velocity.y);
        //}
        v2 = new Vector2(InxPos, rg2d.velocity.y);

        rg2d.velocity = v2;
    }

    private void _Attack()
    {
        Debug.Log("Attack");
        charactor.Attack();
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
            _Attack();
        }
        if (Input.GetMouseButtonDown(1))//우클릭
        {
            Formchange();
        }

        if (Input.GetKeyDown(KeyCode.W) && isGround)//점프키 관련 만약 키가 변한다면 keycode만 변경하면 됨
        {//"W"가 점프라고 생각했을때 구현내용
            Jump();
        }
        else if (Input.GetKey(KeyCode.W) && jumpState.isJump && Time.time - jumpState.jumpStartTime < jumpDuration)
        {
            //Debug.Log("HOLDDDDDDDDDDDDD");
            jumpState.jumptype = JumpState.State.LONG_JUMP;
        }
        else
        {
            //Debug.Log("else");
            jumpState.isJump = false;
            jumpState.jumptype = JumpState.State.IDLE;

        }

        //if (Input.GetKeyDown(KeyCode.S))
        if (Input.GetKey(KeyCode.S))
        {
            if (isHide)
            {
                guard.transform.position = this.gameObject.transform.GetChild(0).transform.position;
                charactor.Crouch(guard);
                isCrouching = true;
            }
            if (currentOneWayPlatform != null)//밑 점프 부분
            {
                Debug.Log("hello");
                canDown = true;
                //Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
                //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("OneWayPlatform"),true);

                //StartCoroutine(DownJump());
            }

        }
        else
        {
            guard.SetActive(false);
            isCrouching = false;
        }
        //if (currentOneWayPlatform == null)
        //{
        //    BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        //    Physics2D.IgnoreCollision(playerCollider, platformCollider);
        //}
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

            //Debug.Log("좌우");
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            //InxPos = 0;
            isMoving = false;

            //Debug.Log("좌우 입력값 " + InxPos);
        }
    }

    private void Update()
    {
        InputManager();
        //_Raycast();
    }

    void _Raycast()
    {

        if (Input.GetKey(KeyCode.A))
        {
            vec = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            vec = Vector2.right;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, vec, 0.3f, LayerMask.GetMask("bullet"));
        Debug.DrawRay(transform.position, vec, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "bullet")
            {
                CapsuleCollider2D cap2D = hit.collider.GetComponent<CapsuleCollider2D>();

                cap2D.isTrigger = true;
                Debug.Log("trigger on");


            }
            Debug.Log("hit");
            Debug.Log(hit.transform.name);
        }




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
        //Debug.Log("higer");
        rg2d.AddForce(Vector3.up * jumpForce * Time.deltaTime * 2, ForceMode2D.Impulse);//추가적인 힘을 계속 주는것 time.deltatime은 아주작은 소수점이 나올것이므로 곱하게 되면 값이 아주작아짐
    }

    IEnumerator DownJump()
    {
        while (true)
        {
            if (canDown)
            {
                Debug.Log("hi");
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("OneWayPlatform"), true);
                yield return new WaitForSeconds(downTime);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("OneWayPlatform"), false);
                canDown = false;
            }
            yield return null;


        }


    }

    int coverDir(Collider2D collision)
    {
        float check = collision.gameObject.transform.position.x - this.gameObject.transform.position.x;

        Debug.Log($"트리거 체크 {Mathf.Sign(check)}");
        isHide = true;
        Debug.Log("트리거엄페물");
        if (Mathf.Sign(check) < 0)
        {
            this.gameObject.transform.GetChild(0).transform.localPosition = new Vector3(-0.76f, 0, 0);
        }
        else
        {
            this.gameObject.transform.GetChild(0).transform.localPosition = new Vector3(0.76f, 0, 0);
        }

        return (int)Mathf.Sign(check);
    }
}