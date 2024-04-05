using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlatformScript:MonoBehaviour//해당 스크립트에서 MonoBehaviour가 들어간것은 인스펙터에서 오브젝트 설정을 위함
{

    // Start is called before the first frame update
    public enum downJumpObject { STRAIGHT, DIAGONAL};//대각선 바닥 일자 바닥 설정
    public downJumpObject dObject;
}
