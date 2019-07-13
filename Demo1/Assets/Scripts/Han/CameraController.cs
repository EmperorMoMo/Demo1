using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerInput pi;
    public float horizontalSpeed = 20.0f;//相机水平旋转速度
    public float verticalSpeed = 20.0f;//相机上下旋转速度

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerx;

    // Start is called before the first frame update
    void Awake()
    {
        cameraHandle = transform.parent.gameObject;//取得MainCamera的父物体cameraHandle组件
        playerHandle = cameraHandle.transform.parent.gameObject;//取得MainCamera父亲的父亲也就是PlayerHandle物体
        tempEulerx = 20;
    }

    // Update is called once per frame
    void Update()
    {
        playerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed * Time.deltaTime);//控制相机水平旋转

        //cameraHandle.transform.Rotate(Vector3.right, pi.Jup * -verticalSpeed * Time.deltaTime);//控制相机上下
        //tempEulerx = cameraHandle.transform.eulerAngles.x;
        tempEulerx -= pi.Jup * verticalSpeed * Time.deltaTime;
        tempEulerx = Mathf.Clamp(tempEulerx, -20, 30);
        cameraHandle.transform.localEulerAngles =
            new Vector3(tempEulerx, 0, 0);
    }
}
