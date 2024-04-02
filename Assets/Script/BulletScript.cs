using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bullet_speed = 1f;

    public GameObject player;

    public GameObject temp;

    public Vector3 targetPos;
    public Vector3 shootPos;
    public Vector2 nowpos;
    public Vector2 destination;

    public float startTime;

    public Rigidbody2D bullet_rg;

    public bool block;
    // Start is called before the first frame update
    
    private void Awake()
    {
        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("bullet"), LayerMask.NameToLayer("OneWayPlatform"), true);
        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("bullet"), LayerMask.NameToLayer("Ground"), true);
    }
    void Start()
    {
        startTime = 0;
        if (this.gameObject.tag != "bullet")//적군총알이라면
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyBullet"), LayerMask.NameToLayer("OneWayPlatform"), true);//총알 레이어 설정
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyBullet"), LayerMask.NameToLayer("Ground"), true);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyBullet"), LayerMask.NameToLayer("Enemy"), true);
            destination = targetPos - transform.position;//타겟 목적지 설정
        }
        else//플레이어 총알이라면
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("OneWayPlatform"), true);//총알 레이어 설정
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Ground"), true);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Player"), true);
            //this.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
            destination = targetPos - transform.position;//타겟 목적지 설정
        }


        bullet_rg = GetComponent<Rigidbody2D>();    
        //transform.position = Vector3.MoveTowards(transform.position, temp.GetComponent<EnemyScript>().playerPos, Time.deltaTime * 10);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("player pos"+ targetPos + "enemy pos" + shootPos);
        nowpos = transform.position;//목적지 도착시 오브젝트 파괴를 위함
        bullet_rg.velocity = new Vector2(destination.x, destination.y).normalized*bullet_speed;
        //nowpos = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * 10);
        
        if (nowpos == destination)
        {
            Debug.Log("도착");
            //Destroy(this.gameObject) ;
        }



    }


    private void Update()
    {
        startTime+= Time.deltaTime;
        if (startTime > 4)
        {
            Destroy(this.gameObject);
            
        }

        if (!ControllerScript.Instance.isCrouching)
        {
            this.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "guard")
        {
            this.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
        }
        if (collision.gameObject.tag == "Player")//레이어 설정한 것 때문에 적군 총알만 플레이어 에게 충돌일어남
        {

        }
        if(collision.gameObject.tag == "Enemy")//플레이어 총알이 적군에게 충돌시
        {
            Debug.Log("적 충돌");
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "guard")
        {
            
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "guard")
        {
            this.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
        }
        if (collision.gameObject.tag == "Player")//레이어 설정한 것 때문에 적군 총알만 플레이어 에게 충돌일어남
        {
            Debug.Log("플레이어 충돌");
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Enemy")//플레이어 총알이 적군에게 충돌시 적이 trigger로 설정되어있어서 안 쓸것 같음
        {
            //Debug.Log("적 충돌");
            //Destroy(this.gameObject);
        }


    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "guard")
        {
            Debug.Log("벗어남");
            this.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
        }
    }
}
