using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyScript : MonoBehaviour
{

    public GameObject bullet;
    public GameObject[] bullets;

    public GameObject player;

    public Vector3 playerPos;
    public Vector3 enemypos;
    public int a = 3;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(this.transform.parent);
        player = GameObject.FindWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(playerPos);
        enemypos = transform.position;
        if (Input.GetKeyDown(KeyCode.X))
        {
            Shoot();
        }


    }

    [ContextMenu("Shoot")]
    public void Shoot()
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
    
}
