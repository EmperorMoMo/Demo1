using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//状态
public enum EnemyState
{
    idle,
    run,
    attack
}
public class AI : MonoBehaviour
{
    //状态
    public EnemyState CurrentState = EnemyState.idle;
    //动画控制器
    private Animation ani;
    //玩家
    private Transform player;
    //导航代理
    private NavMeshAgent agent; 
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animation>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        switch (CurrentState)
        {
            case EnemyState.idle:
                //站立状态，判断和玩家的距离如果在1到3米内，变为跑步
                if(distance>1&&distance<=3)
                {
                    CurrentState = EnemyState.run;
                }
                //播放站立动画
                ani.Play("IdleF");
                //导航停止
                agent.isStopped = true;
                break;
            case EnemyState.run:
                //追踪状态，判断如果离玩家大于3米，则变回到站立
                if(distance >3)
                {
                    CurrentState = EnemyState.idle;
                }
                //播放跑步动画
                ani.Play("Run");
                //导航开始
                agent.isStopped = false;
                agent.SetDestination(player.position);//导航目标
                break;
            case EnemyState.attack:
                break;
        }
    }
}
