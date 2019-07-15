using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionControl : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRM", (object) anim.deltaPosition);//把(object) anim.deltaPosition（模型自身动画移动量）传给OnUpdateRM
    }
}
