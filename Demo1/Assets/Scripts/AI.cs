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
        
    }
}
