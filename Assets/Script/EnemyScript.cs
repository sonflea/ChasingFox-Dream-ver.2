using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyScript : MonoBehaviour
{

    public GameObject bullet;//총알 연결
    public GameObject[] bullets;//현재 사용은 안 하며 혹시나 필요할까 만들어 둔 부분 사용은 안 하는중 

    public GameObject player;//플레이어를 타겟팅 하기위해서 플레이어를 연결하기위한 부분

    public Vector3 playerPos;//플레이어의 위치를 담는 변수
    public Vector3 enemypos;//본인의 위치를 담는 변수
    public int a = 3;//테스트로 만든 변수같은데 안 쓰는듯 지워도 아마도 무방함
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(this.transform.parent);
        player = GameObject.FindWithTag("Player");//플레이어를 찾아서 담음, 이렇게 한 이유는 처음부터 다 연결해두고 적군을 설정하면 좋지만 만약 동적으로 적군을 생성해야할경우를 위해 넣은 부분
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(playerPos);
        enemypos = transform.position;//본인의 위치를 계속 초기화
        if (Input.GetKeyDown(KeyCode.X))//해당 부분은 정말 테스트를 위해만든 부분이었음 그냥 내가 x를 누르면 적군이 공격하는걸 하기위함
        {
            Shoot();
        }


    }

    public void Shoot()//사격 코드 해당 코드는 나중에 수정해서 계속 사용해도 될것음
    {
        //Instantiate(bullet,new Vector3(0,0,0),Quaternion.identity);
        GameObject _bullet = Instantiate(bullet, enemypos, transform.rotation);
        //_bullet.transform.SetParent(this.transform);
        playerPos = player.gameObject.transform.position;
        _bullet.GetComponent<BulletScript>().targetPos = playerPos;
        _bullet.GetComponent<BulletScript>().shootPos = enemypos;
        //enemypos = transform.position;
        Debug.Log("shoot"+playerPos+"enemypos"+enemypos);
        //_bullet.transform.position = Vector2.left;
    }


    private void OnTriggerEnter2D(Collider2D collision)//만약 적군이 근접공격을 맞을경우 만든건데 아마 공격관련 trigger들을 나중에 플레이어한테 넣을지  적군 개개인한테 넣을지 고민이긴함 근데 플레이어쪽에게 넣는게 좋아보이긴함
    {
        if (collision.gameObject.name == "MeleeAttack")
        {
            Debug.Log("쌈@뽕한 근접 공격 뒤졌죠?");
        }
    }

}
