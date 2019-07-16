using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour//abstract抽象类无法被实例化（无法new出来，或者Instance出来）
{
    /// <summary>
    /// 这些都是处理信号模块，封装在抽象类中，以后添加新的设备只需要改处理硬件的模块
    /// </summary>
    [Header("===== Output signals =====")]
    public float Dup;   //Dup(signal)控制信号
    public float Dright;    //Dright(signal)控制信号
    public float Dmag;  //中间参数
    public Vector3 Dvec;    //坐标变量，赋予给forward向量
    public float Jup;
    public float Jright;

    //1.pressing signal 按压信号
    public bool run;    //控制跑步的信号，按压型信号
    //2.trigger once signal 一次性触发信号
    public bool jump;       //控制跳跃的信号，按一次触发一次的
    public bool lastJump;
    public bool attack;
    public bool lastAttack;

    //3.double trigger  多次触发信号

    [Header("===== Others =====")]
    public bool inputEnabled = true;    //控制是否行走的开关（软开关）

    //在父类里要把private改成protected，不然子类无法调用
    protected float targetDup;        //目标值Dup
    protected float targetDright;     //目标值Dright
    protected float velocityDup;      //过渡值Dup（SmoothDamp方法中的转换速度,SmoothDamp方法会根据最后的时间参数自行计算）
    protected float velocityDright;   //过渡值Dright（SmoothDamp方法中的转换速度,SmoothDamp方法会根据最后的时间参数自行计算）

    //键盘和手柄都有的部分可以放到IUserInput，精简代码
    protected Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }

}
