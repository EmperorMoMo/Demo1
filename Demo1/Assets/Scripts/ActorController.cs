using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public PlayerInput pi;

    [SerializeField]//可以把私有变量显示到unity
    private Animator anim;
    
    // Start is called before the first frame update
    void Awake()//Awake比Start好一些
    {
        //anim = GetComponent<Animator>();
        pi = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        //print(pi.Dup);
        anim.SetFloat("forward", pi.Dmag);
        if (pi.Dmag > 0.1f)
        {
            model.transform.forward = pi.Dvec;
        }
    }
}
