using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public PlayerInput pi;
    public float walkSpeed = 2.0f;//控制行走的速度，让Rigidbody和动画播放的移动速度契合，不滑步

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
        //print(pi.Dup);
        anim.SetFloat("forward", pi.Dmag);

        //让人物不再在玩家没输入的时候回到正面
        if (pi.Dmag > 0.1f)
        {
            model.transform.forward = pi.Dvec;
        }

        movingVec = pi.Dmag * model.transform.forward * walkSpeed; //玩家操作的变量*当下模型的正面，这样就可以得到玩家的移动意图
    }

    //rigid的调用最好放在FixedUpdate里面,FixedUpdate中两帧间隔是Time.fixedDeltaTime(1/50)
    void FixedUpdate()
    {
        //rigid.position += movingVec * Time.fixedDeltaTime;//直接改位置，需要速度*时间
        //rigid.velocity = movingVec;//直接指派速度，就不需要乘时间了,但这样的话从斜坡下来的时候会飘在空中然后慢慢下落，
        //可以按下面写法
        rigid.velocity = new Vector3(movingVec.x, rigid.velocity.y, movingVec.z);
    }

}
