using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton
{
    public bool IsPressing = false;//正在被按住
    public bool OnPressed = false;//刚刚被按住
    public bool OnReleased = false;//刚刚被释放

    private bool curState = false;//目前状态
    private bool lastState = false;//前一次的状态

    public void Tick(bool input)
    {
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
            }
            else
            {
                OnReleased = true;
            }
        }
        lastState = curState;
    }

}
