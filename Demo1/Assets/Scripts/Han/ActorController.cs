using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    //public PlayerInput pi;
    public IUserInput pi;//新增的抽象类，让PlayerInput继承IUserInput，这个抽象类可以方便以后拓展其他设备如手柄，AR眼镜等
    public float walkSpeed = 2.4f;//控制行走的速度，让Rigidbody和动画播放的移动速度契合，不滑步
    public float runMultiplier = 2.0f;//控制跑步的速度
    public float jumpVelocity = 5.0f;//跳的高度
    public float rollVelocity = 1.0f;//翻滚距离

    [Space(10)]
    [Header("===== Friction Settings =====")]
    //控制摩擦力
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;//用于存储跟玩家相关的移动控制信息
    private Vector3 thrustVec;//冲量向量
    private bool canAttack;//限定是否可以进行attack，尝试解决跳跃过程中能攻击的问题
    private bool lockPlanar = false;//是否锁死平面移动速度
    private CapsuleCollider col;//用于获取CapsuleCollider组件
    private float lerpTarget;//充当状态机层级的权重（1.0f)

    private Vector3 deltaPos;//??
    
    void Awake()//Awake比Start好一些
    {
        anim = model.GetComponent<Animator>();
        //pi = GetComponent<PlayerInput>();
        pi = GetComponent<IUserInput>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
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

        //播放defense举盾动画
        anim.SetBool("defense",pi.defense);

        //翻滚动画播放
        if (pi.jump && rigid.velocity.magnitude > 0f)
        {
            anim.SetTrigger("roll");
        }

        //跳跃动画播放
        if (pi.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;//跳跃的时候，canAttack为false的无法进行攻击
        }

        //攻击动画播放        CheckState目前可有可无？？？
        if (pi.attack && CheckState("ground") && canAttack)//添加限定条件，判断是否在ground状态机，并且判断canAttack是否为true
        {
            anim.SetTrigger("attack");
        }

        //让人物不再在玩家没输入的时候回到正面
        if (pi.Dmag > 0.1f)
        {
            //model.transform.forward = pi.Dvec;//控制玩家转向,但是这么做的话，转向非常迅速，看起来很怪，所以用下面代码

            //Vector3 targetForward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);//Slerp差值法，让数值平滑
            //model.transform.forward = targetForward;//上下两句可以合并成下面这一句，不过拆开容易理解
            model.transform.forward= Vector3.Slerp(model.transform.forward, pi.Dvec, 0.25f);
        }

        if (lockPlanar==false)//当lockPlanar为false时，可以自由移动
        {
            //玩家操作的变量*当下模型的正面，这样就可以得到玩家的移动意图
            planarVec = pi.Dmag * model.transform.forward * walkSpeed *
                        ((pi.run) ? runMultiplier : 1.0f); //walkspeed后面乘的数是跑步速度
        }

        //print(CheckState("idle", "attack"));//检验CheckState是否成功
    }

    //rigid的调用最好放在FixedUpdate里面,FixedUpdate中两帧间隔是Time.fixedDeltaTime(1/50)
    void FixedUpdate()
    {
        rigid.position += deltaPos;//给刚体加上从OnUpdateRM（在讯息里）上累加的deltaPos（模型自身动画的位移量）

        //rigid.position += planarVec * Time.fixedDeltaTime;//直接改位置，需要速度*时间
        //rigid.velocity = planarVec;//直接指派速度，就不需要乘时间了,但这样的话从斜坡下来的时候会飘在空中然后慢慢下落，
        //可以按下面写法
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;

        deltaPos = Vector3.zero;//播放完一次后清零
    }

    //检查状态机是在哪个层级（stateName为层级里面的状态机，layerName判断是否在Base Layer层级）
    private bool CheckState(string stateName,string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);//获取状态机层级的索引
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);//获取层级索引里面状态机的名字
        return result;

        //上面三句可以合并为一句(不过会看不懂)
        //return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }

    ///
    ///下面放Message讯息
    ///
    public void OnJumpEnter()
    {
        //print("On Jump Enter");
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, jumpVelocity, 0);
    }

    //public void OnJumpExit()
    //{
    //    //print("On Jump Exit");
    //    pi.inputEnabled = true;
    //    lockPlanar = false;
    //}

    public void IsGround()
    {
        //print("Is On Ground!!!!!!!!!!");
        anim.SetBool("isGround",true);
    }

    public void IsNotGround()
    {
        //print("Is Not On Ground!!!!!!!!!");
        anim.SetBool("isGround", false);
    }

    public void OnGroundEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
        canAttack = true;//落地的时候canAttack改为true
        //将PlayerHandle的CapsuleCollider组件中Material(摩擦力材质)切换为frictionOne（自己制作的）,在地上的时候静动摩擦力都为1（最大）
        col.material = frictionOne;
    }

    public void OnGroundExit()
    {
        //将PlayerHandle的CapsuleCollider组件中Material(摩擦力材质)切换为frictionZero（自己制作的），
        //在跳起来的时候设置静动摩擦力为0，解决黏在墙上的bug
        col.material = frictionZero;
    }

    public void OnFallEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }

    public void OnRollEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, rollVelocity, 0);
    }

    public void OnJabEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }

    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }

    public void OnAttack1hAEnter()
    {
        pi.inputEnabled = false;
        //lockPlanar = true;
        lerpTarget = 1.0f;
    }

    public void OnAttack1hAUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity");//通过动画曲线(Curves)来给模型攻击时增加位移冲量

        float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("attack"));//使用currentWeight来获取权重值
        currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.25f);//通过Mathf.lerp来使currentWeight值过渡平滑
        anim.SetLayerWeight(anim.GetLayerIndex("attack"), currentWeight);//改attack层级的权重
        //上面三句可以合并为一句
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"),
        //    Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.1f));
    }

    //public void OnAttack1hCEnter()
    //{
    //    pi.inputEnabled = false;
    //}

    //public void OnAttack1hCUpdate()
    //{
    //    thrustVec = model.transform.forward * anim.GetFloat("attack1hCVelocity");
    //    anim.SetLayerWeight(anim.GetLayerIndex("attack"),
    //        Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.25f));
    //}

    public void OnAttackIdleEnter()
    {
        pi.inputEnabled = true;
        //lockPlanar = false;
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), 0f);//改attack层级的权重
        lerpTarget = 0f;
    }

    public void OnAttackIdleUpdate()
    {
        //float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("attack"));//使用currentWeight来获取权重值
        //currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.25f);//通过Mathf.lerp来使currentWeight值过渡平滑
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), currentWeight);//改attack层级的权重
        anim.SetLayerWeight(anim.GetLayerIndex("attack"),
            Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.25f));
    }

    public void OnUpdateRM(object _deltaPos)
    {
        //print(_deltaPos);
        if (CheckState("attack1hC", "attack"))//检查是否在attack层级中的第三段攻击状态
        {
            //deltaPos += (Vector3)_deltaPos;//将从RootMotionControl传来的_deltaPos累加到deltaPos，在下面改一下第三段攻击抖动问题
            deltaPos += (deltaPos + (Vector3) _deltaPos) / 2.0f;

        }
    }
}
