using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    //싱글톤 구현? 다른 ui시스템 스크립트를 넣어두고 여기서 관리 혹은 static으로 불러옴 근데 static으로 불러오는것보다는 싱글톤이 좋아보임
    [SerializeField] private UnityEngine.UI.Image image;
    public bool reloadDoon = true;
    [SerializeField] private float duration = 1f;
    public bool startCor = false;
    // Start is called before the first frame update

    private static UIController instance=null;
    public static UIController Instance 
    { 
        get 
        {
            if(instance == null) { return null; }
            return instance; 
        } 
    }

    private void Awake()
    {
        if(instance!= null)
        {
            Destroy(this.gameObject);
            return; 
        }instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ControllerScript.Instance.b_reload)
        {
            _DrawReload();
            Debug.Log("그리는중");
        }
        else
        {
            Debug.Log("그림완");
        }

        
        
    }

    public IEnumerator DrawReload()
    {
        Debug.Log("코루틴 시작");
        float currentTime = 0.0f;
        float startFillAmout = 0f;
        float endFillAmout = 1.0f;
        startCor = true;

        while (currentTime <= duration)//재장전 확인값 던져주는지 체크 아마 currentTime<duration&&reloadStart
        {
            reloadDoon = false;
            float t = currentTime / duration;
            //float currentValue = Mathf.Lerp(startFillAmout,endFillAmout, t);
            //image.fillAmount = t;

            currentTime += Time.deltaTime;
            //Debug.Log("재장전중" + currentTime);
            yield return null;
        }

        if (currentTime >= duration)//완료상태
        {
            //Debug.Log("완료");
            reloadDoon = true;
            //currentTime = 0.0f; 
            Debug.Log("1초 후");
            startCor = false;
            //yield return new WaitForSecondsRealtime(1);
            
        }
        //yield return true;
        //yield return null;//이게 밖에있어야함 무한 루프기준
        
    }

    public void _DrawReload()//ControllerScript 의 temp를 받아서 가능함
    {
        //재장전 애니메이션 및 내용들
        image.fillAmount = ControllerScript.Instance.currentTime / ControllerScript.Instance.duration;
        
    }


}
