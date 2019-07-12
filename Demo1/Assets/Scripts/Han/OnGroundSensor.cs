using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider capcol;//可以获取PlayerHandle的CapsuleCollider
    public float offset = 0.1f;

    private Vector3 point1;
    private Vector3 point2;
    private float radius;

    // Start is called before the first frame update
    void Awake()
    {
        radius = capcol.radius - 0.05f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        point1 = transform.position + transform.up * (radius - offset);//人物脚底往上0.3的点
        point2 = transform.position + transform.up * (capcol.height - offset) - transform.up * radius;//人物头顶往下0.3的点

        //调用方法把point1,point2编织成和PlayerHandle的胶囊体一样大小线框
        Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("Ground"));     
        if (outputCols.Length!=0 )
        {
            //foreach (var col in outputCols)
            //{
            //    print("collision:" + col.name);
            //}

            SendMessageUpwards("IsGround");

        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }
        
    }
}
