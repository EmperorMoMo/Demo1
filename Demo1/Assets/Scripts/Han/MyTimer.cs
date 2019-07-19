using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer//计时器
{
    public enum STATE//枚举类型
    {
        IDLE,//计时器未开始运行
        RUN,//计时器开始运作
        FINISHED//计时器结束运作
    }
    public STATE state;

    public float duration = 1.0f;//计时器结束时间

    private float elapsedTime = 0;//流逝的时间

    public void Tick()
    {
        if (state == STATE.IDLE)
        {

        }
        else if (state == STATE.RUN)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration)//判断是否到时间了
            {
                state = STATE.FINISHED;
            }
        }
        else if (state == STATE.FINISHED)
        {

        }
        else
        {
            Debug.Log("MyTimer error");
        }
    }

    public void Go()//计时器开启
    {
        elapsedTime = 0;
        state = STATE.RUN;
    }

}
