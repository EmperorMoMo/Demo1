﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class KeyboardInput : IUserInput
{
    //Variable  变数变量区
    [Header("===== Key settings =====")]//标识这个区块代码是什么
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;
    public string keyE;

    public MyButton buttonA = new MyButton();
    public MyButton buttonB = new MyButton();
    public MyButton buttonC = new MyButton();
    public MyButton buttonD = new MyButton();
    public MyButton buttonE = new MyButton();

    public string keyJRight;//控制相机移动
    public string keyJLeft; //控制相机移动
    public string keyJUp;   //控制相机移动
    public string keyJDown; //控制相机移动
    

    /*这些都放到IUserInput里了*/
    //[Header("===== Output signals =====")]
    //public float Dup;   //Dup(signal)控制信号
    //public float Dright;    //Dright(signal)控制信号
    //public float Dmag;  //中间参数
    //public Vector3 Dvec;    //坐标变量，赋予给forward向量
    //public float Jup;
    //public float Jright;

    ////1.pressing signal 按压信号
    //public bool run;    //控制跑步的信号，按压型信号
    ////2.trigger once signal 一次性触发信号
    //public bool jump;       //控制跳跃的信号，按一次触发一次的
    //public bool lastJump;
    //public bool attack;
    //public bool lastAttack;

    ////3.double trigger  多次触发信号

    //[Header("===== Others =====")]
    //public bool inputEnabled = true;    //控制是否行走的开关（软开关）

    //private float targetDup;        //目标值Dup
    //private float targetDright;     //目标值Dright
    //private float velocityDup;      //过渡值Dup（SmoothDamp方法中的转换速度,SmoothDamp方法会根据最后的时间参数自行计算）
    //private float velocityDright;   //过渡值Dright（SmoothDamp方法中的转换速度,SmoothDamp方法会根据最后的时间参数自行计算）

    [Header("===== Mouse settings =====")]
    public bool mouseEnable = false;

    public float mouseSensitivityX = 1.0f;//用于改变鼠标X轴移动速度
    public float mouseSensitivityY = 1.0f;//用于改变鼠标Y轴移动速度

    // Update is called once per frame
    void Update()
    {

        //MyButton写了信号
        buttonA.Tick(Input.GetKey(keyA));
        buttonB.Tick(Input.GetKey(keyB));
        buttonC.Tick(Input.GetKey(keyC));
        buttonD.Tick(Input.GetKey(keyD));
        buttonE.Tick(Input.GetKey(keyE));

        //当mouseEnable为true时，使用鼠标控制相机
        if (mouseEnable == true)
        {
            //也可以用鼠标控制摄像机
            Jup = Input.GetAxis("Mouse Y") * 2.5f * mouseSensitivityY;//2.5f是预设值，后面的mouseSensitivityY方便玩家修改
            Jright = Input.GetAxis("Mouse X") * 3f * mouseSensitivityX;//3f是预设值，后面的mouseSensitivityX方便玩家修改
        }
        else//当mouseEnable为false则使用键盘控制
        {
            //Jup控制相机上下移动，值为（1，0，-1）。Jright控制相机左右值也为（1,0,-1)
            Jup = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0);
            Jright = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0);
        }
        
        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        //三元表达式()?():()     通过Dup值的变化(1,0,-1)来控制前后行走
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);
        //通过Dright值的变化(1,0,-1)来控制左右行走,两个控制信号形成一个二维坐标轴

        //玩家操作的开关，关了则无法操作
        if (inputEnabled == false)
        {
            targetDup = 0;
            targetDright = 0;
        }

        //SmoothDamp方法可以让控制信号能平滑的从0到1或者-1而不是直接从0到1或-1（应该也可以用Vector3.Lerp实现）
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);//ref的意思是传参不传值
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        //这段代码修复斜着跑的时候速度会变快的BUG
        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;

        //控制人物转向
        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));//勾股定理
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;      //这段代码需要好好理解！！精华所在

        //defense控制角色举盾
        //defense = Input.GetKey(keyD);
        defense = buttonD.IsPressing;

        //人物跑步
        //run = Input.GetKey(keyA);
        run = buttonA.IsPressing;

        //人物跳跃
        //bool newJump = Input.GetKey(keyB);
        ////jump = tempJump;
        //if (newJump != lastJump && newJump==true )
        //{
        //    jump = true;
        //}
        //else
        //{
        //    jump = false;
        //}
        //lastJump = newJump;
        jump = buttonB.OnPressed;

        //人物攻击
        //bool newAttack = Input.GetKey(keyC);
        //if (newAttack != lastAttack && newAttack == true)
        //{
        //    attack = true;
        //}
        //else
        //{
        //    attack = false;
        //}
        //lastAttack = newAttack;
        attack = buttonC.OnPressed;

        hide = buttonE.IsPressing;
        
        
    }

    ////修复斜着跑速度变快的BUG（放到IUserInput里了，与手柄共同的代码都可以放进去）
    //private Vector2 SquareToCircle(Vector2 input)
    //{
    //    Vector2 output = Vector2.zero;

    //    output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
    //    output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

    //    return output;
    //}
}
