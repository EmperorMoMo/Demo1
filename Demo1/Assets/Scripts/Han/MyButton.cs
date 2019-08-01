using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton
{
    public bool IsPressing = false;//正在被按住
    public bool OnPressed = false;//刚刚被按住
    public bool OnReleased = false;//刚刚被释放
    public bool IsExtending = false;//判断是否延伸的信号
    public bool IsDelaying = false;//判断长按信号
    
    public float extendingDuration = 0.15f;
    public float delayingDuration = 1.0f;

    private bool curState = false;//目前状态
    private bool lastState = false;//前一次的状态

    private MyTimer exitTimer=new MyTimer();//定义退出时间
    private MyTimer delayTimer = new MyTimer();

    public void Tick(bool input)
    {
        //下面的StartTimer方法是对这几句话的一个封装
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    exitTimer.duration = 3.0f;
        //    exitTimer.Go();
        //}
        
        exitTimer.Tick();
        delayTimer.Tick();

        curState = input;//keyboardInput中有按键按下的时候，传入一个input的bool值给curState

        IsPressing = curState;//把curState值再传给IsPressing，表示目前为正在被按住      注：长按会一直有信号

        //下面这几段话表示，刚刚被按住的时候有一个信号传给OnPressed,释放的时候传一个信号给OnReleased
        //注：按一下按键或者释放一次才会返回一个值，长按没用
        OnPressed = false;
        OnReleased = false;
        if (curState != lastState)
        {
            if (curState == true)
            {
                OnPressed = true;
                StartTimer(delayTimer,delayingDuration);
            }
            else
            {
                OnReleased = true;
                StartTimer(exitTimer, extendingDuration);
            }
        }
        lastState = curState;

        if (exitTimer.state == MyTimer.STATE.RUN)//判断计时器是否在运行，若在运行，则把延伸信号设置为true
        {
            IsExtending = true;
        }
        else
        {
            IsExtending = false;
        }

        if (delayTimer.state == MyTimer.STATE.RUN)//判断计时器是否在运行，若在运行，则把长按信号设置为true
        {
            IsDelaying = true;
        }
        else
        {
            IsDelaying = false;
        }

    }

    private void StartTimer(MyTimer timer,float duration)
    {
        timer.duration = duration;
        timer.Go();
    }
}
