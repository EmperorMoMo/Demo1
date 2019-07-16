using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimFix : MonoBehaviour
{

    private Animator anim;

    public Vector3 a;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    void OnAnimatorIK()
    {
        if (anim.GetBool("defense")==false)
        {
            Transform leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);//取得模型上的左下臂的骨头
            leftLowerArm.localEulerAngles += 0.75f * a;//设置手臂旋转角度
            //通过SetBoneLocalRotation(要旋转的骨头，旋转角度的四元素)方法让手臂旋转        
            anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftLowerArm.localEulerAngles));
        }
    }

}
