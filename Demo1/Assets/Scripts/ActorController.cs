﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public PlayerInput pi;
    public float walkSpeed = 2.0f;//控制行走的速度，让Rigidbody和动画播放的移动速度契合，不滑步
    public float runMultiplier = 2.0f;//控制跑步的速度

    [SerializeField]//可以把私有变量显示到unity
    private Animator anim;

    private Rigidbody rigid;
    private Vector3 movingVec;//用于存储跟玩家相关的移动控制信息
    
    // Start is called before the first frame update
    void Awake()//Awake比Start好一些
    {
        //anim = GetComponent<Animator>();
        pi = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
    }

    //Update中两帧间隔是Time.deltaTime(1/60)
    void Update()
    {
        //pi.Dmag后面乘的数是为了混合树的正常播放,这么做从步行到跑步很突然
        //anim.SetFloat("forward", pi.Dmag * ((pi.run) ? 2.0f : 1.0f));

        //下面这种做法能改正   /*pi.Dmag好好理解一下*/
        //float targetMulti = ((pi.run) ? 2.0f : 1.0f);//创建中间变量targetMulti
        //获取状态机forward的值，Mathf.Lerp（现在的值，目标值，变化幅度）
        //anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), targetMulti, 0.25f));
        //上面两句代码可以整合为一句
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), ((pi.run) ? 2.0f : 1.0f), 0.25f));

        //跳跃动作播放
        if (pi.jump)
        {
            anim.SetTrigger("jump");
        }

        //让人物不再在玩家没输入的时候回到正面
        if (pi.Dmag > 0.1f)
        {
            //model.transform.forward = pi.Dvec;//控制玩家转向,但是这么做的话，转向非常迅速，看起来很怪，所以用下面代码

            //Vector3 targetForward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);//Slerp差值法，让数值平滑
            //model.transform.forward = targetForward;//上下两句可以合并成下面这一句，不过拆开容易理解
            model.transform.forward= Vector3.Slerp(model.transform.forward, pi.Dvec, 0.25f);
        }

        //玩家操作的变量*当下模型的正面，这样就可以得到玩家的移动意图
        movingVec = pi.Dmag * model.transform.forward * walkSpeed *
                    ((pi.run) ? runMultiplier : 1.0f); //walkspeed后面乘的数是跑步速度
    }

    //rigid的调用最好放在FixedUpdate里面,FixedUpdate中两帧间隔是Time.fixedDeltaTime(1/50)
    void FixedUpdate()
    {
        //rigid.position += movingVec * Time.fixedDeltaTime;//直接改位置，需要速度*时间
        //rigid.velocity = movingVec;//直接指派速度，就不需要乘时间了,但这样的话从斜坡下来的时候会飘在空中然后慢慢下落，
        //可以按下面写法
        rigid.velocity = new Vector3(movingVec.x, rigid.velocity.y, movingVec.z);
    }

    public void OnJump()
    {
        print("On Jump");
    }

}
