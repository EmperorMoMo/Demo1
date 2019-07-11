using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //Variable  变数变量区
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    public float Dup;   //Dup(signal)控制信号
    public float Dright;    //Dright(signal)控制信号
    public float Dmag;  //中间参数
    public Vector3 Dvec;    //坐标变量，赋予给forward向量

    public bool inputEnabled = true;    //控制是否行走的开关（软开关）

    private float targetDup;        //目标值Dup
    private float targetDright;     //目标值Dright
    private float velocityDup;      //过渡值Dup（SmoothDamp方法中的转换速度,SmoothDamp方法会根据最后的时间参数自行计算）
    private float velocityDright;   //过渡值Dright（SmoothDamp方法中的转换速度,SmoothDamp方法会根据最后的时间参数自行计算）

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        //三元表达式()?():()     通过Dup值的变化(1,0,-1)来控制前后行走
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);
        //通过Dright值的变化(1,0,-1)来控制左右行走,两个控制信号形成一个二维坐标轴

        if (inputEnabled == false)
        {
            targetDup = 0;
            targetDright = 0;
        }

        //SmoothDamp方法可以让控制信号能平滑的从0到1或者-1而不是直接从0到1或-1
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);//ref的意思是传参不传值
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        //控制人物转向
        Dmag = Mathf.Sqrt((Dup * Dup) + (Dright * Dright));//勾股定理
        //if (Dmag > 1)
        //{
        //    Dmag = 1;
        //}
        Dvec = Dright * transform.right + Dup * transform.forward;      //这段代码需要好好理解！！精华所在
    }
}
