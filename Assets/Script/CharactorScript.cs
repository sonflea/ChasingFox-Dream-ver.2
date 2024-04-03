using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Timers;
using TMPro;

public partial class ControllerScript : MonoBehaviour
{
    [SerializeField] GameObject attackPoint;//공격지점
     public GameObject bullet;//총알 오브젝트 연결
    public GameObject meleeAttack;
    public Vector3 worldPosition;
    public class Charactor
    {
        public float speed;
        public bool isHuman = true;
        public Vector3 hidePos;




        public virtual void Die(Charactor charactor)
        {

        }

        public virtual void Setspeed() { }

        public virtual void Attack() { }
        public void Crouch(GameObject guard)//ref bool 로 접근
        {

            Debug.Log("크라우치");
            guard.SetActive(true);


        }//늑대와 인간 둘다 웅크림이 같다고 가정하기에 그냥 일반함수로 구현

        public virtual void Reload() { }
    }
    public class WereWolf : Charactor
    {
        public int life = 2;
        private static WereWolf instance;
        //IEnumerator tTimer;
        public bool isAttacking = false;

        private bool showAttack = false;

        float t = 0;

        public static WereWolf Instance()
        {
            if (instance == null)
            {
                //Debug.Log("비었음 생성");
                instance = new WereWolf();
                ControllerScript.instance.StartCoroutine(instance.timer());//코루틴 호출 부분이 조금 수정 할 필요있을것 같음 전지전능한 잼미니에게 물어보면 StartCorutine을 그냥 가상함수로 만들라고 하긴하더라

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
        public override void Attack()//다시 공격하는것의 기준은 바닥에 발이 닿았을때로? 그러면 isground
        {
            Debug.Log("늑대 공격");
            //float startAttack = Time.time;
            float startAttack =0;
            t = 0;
            ControllerScript.instance.meleeAttack.transform.position = ControllerScript.instance.attackPoint.transform.position;


            //ControllerScript.instance.meleeAttack.SetActive(true);

            //if (!showAttack)
            //{
                if (!isAttacking)
                {
                    ControllerScript.instance.meleeAttack.SetActive(true);
                    Vector2 temp = (ControllerScript.instance.ClickPos() - ControllerScript.instance.transform.position).normalized;
                    temp = temp.normalized;//짧은 거리도 1로 맞춰주기 귀함
                    Debug.Log(temp);
                    ControllerScript.instance.rg2d.AddForce(temp * 4, ForceMode2D.Impulse);
                    isAttacking = true;
                }
                //else
                //{
                //    temp = new Vector2(temp.x, 0);
                //    ControllerScript.instance.rg2d.AddForce(temp * 4, ForceMode2D.Impulse);
                //}
            //}
            //isAttacking= true;
            //showAttack = true;



            
            //ControllerScript.instance.StartCoroutine(tTimer);

            //Debug.Log("끝");
            //invoke(eraseMelee,1);//만약 invoke로 만든다면

        }
        IEnumerator timer()
        {
            while (true)
            {
                //만약 연속 공격하는게 어색하다면 아래 공격 지속시간을 짧게 함으로서 이상함을 줄여보고 그래도 이상하다면 수정 필요
                if (isAttacking)//if를 지워도 되지 않을까 어차피 클릭자체가 이벤트이자 if임 , if를 지우면 1.x초에서 클릭하면 잠깐 보이고 없어짐
                {
                    
                    //if (t < 1.0f)
                    //{
                    //    ControllerScript.instance.meleeAttack.SetActive(true);
                    //    t += Time.deltaTime;
                    //
                    //}
                    //else
                    //{
                    //    ControllerScript.instance.meleeAttack.SetActive(false);
                    //    showAttack = false;
                    //    t = 0;
                    //}
                    //만약 공격이 연속해서 나갈 수 없다면 아래 부분 그래도 아마 위에 부분 응용해서 만드는게 자연스러울듯
                    Debug.Log("보임");
                    yield return new WaitForSeconds(2);
                    
                    ControllerScript.instance.meleeAttack.SetActive(false);
                    //showAttack = false;
                    isAttacking = false;

                }

                yield return null;
            }

        }
        public void SetFalse()
        {
            ControllerScript.instance.meleeAttack.SetActive(false);
        }

        float Getspeed()
        {
            return this.speed;
        }

    }

    public class Human : Charactor
    {
        public int ammo = 2;
        public int spare_ammo = 0;
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
            if (ammo <= 0)
            {
                Debug.Log("장탄수 부족");
                return;
            }
            Debug.Log("사람 공격 빵야");
            //GameObject _bullet = Instantiate(ControllerScript.instance.bullet, ControllerScript.instance.attackPoint.transform.position, ControllerScript.instance.transform.rotation);
            //_bullet.transform.SetParent(ControllerScript.instance.gameObject.transform);
            Vector3 temp = ControllerScript.instance.ClickPos();
            GameObject _bullet = Instantiate(ControllerScript.instance.bullet, ControllerScript.instance.attackPoint.transform.position, ControllerScript.instance.transform.rotation);

            //var screenPoint = Input.mousePosition;//마우스 위치 가져옴
            //screenPoint.z = Camera.main.transform.position.z;
            //ControllerScript.instance.worldPosition = Camera.main.ScreenToWorldPoint(screenPoint);
            _bullet.GetComponent<BulletScript>().targetPos = temp;
            _bullet.GetComponent<BulletScript>().shootPos = ControllerScript.instance.transform.position;

            //Debug.Log(ControllerScript.instance.worldPosition);
            ammo--;
            Debug.Log($"남은 장탄수{ammo}");

        }
        public override void Setspeed()
        {
            Debug.Log("사람 속도");
            this.speed = 3.0f;
        }
        public void GetAmmo()
        {
            if (ammo < 2)
            {
                spare_ammo++;
            }
        }
        public override void Reload()
        {
            if (isHuman && spare_ammo > 0)
            {
                if (ControllerScript.Instance._DrawReload(ref ControllerScript.instance.b_reload))
                {
                    this.spare_ammo--;
                    this.ammo++;
                }
                //if (!UIController.Instance.startCor)
                //{
                //    UIController.Instance.StartCoroutine(UIController.Instance.DrawReload());
                //}
                //
                ////Debug.Log(UIController.Instance.reloadDoon);
                //if (UIController.Instance.reloadDoon)//1안
                //{
                //    UIController.Instance.StopCoroutine(UIController.Instance.DrawReload());
                //    //ControllerScript.Instance._DrawReload();
                //    //UIController.Instance._DrawReload();
                //    Debug.Log("인간 상태 재장전");
                //    spare_ammo--;
                //    ammo++;
                //    Debug.Log($"남은 장탄수{ammo}");//재장전 쿨타임 필요
                //}
                //Debug.Log("인간 상태 재장전");//2안
                //spare_ammo--;
                //ammo++;
                //Debug.Log($"남은 장탄수{ammo}");//재장전 쿨타임 필요
                //UIController.Instance._DrawReload();//재장전
                //UIController.Calltest();
            }
            //ammo++;

        }
    }
}
