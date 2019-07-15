using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerControl : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ResetTrigger(string triggerName)//与attack1hA和attack1hB里面动画的Events事件中Function的ResetTrigger方法关联
    {
        anim.ResetTrigger(triggerName);//重置Events事件中String中的attack信号
    }
}
