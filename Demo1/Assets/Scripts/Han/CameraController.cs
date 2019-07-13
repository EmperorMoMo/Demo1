using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerInput pi;
    public float horizontalSpeed = 20.0f;//相机水平旋转速度
    public float verticalSpeed = 20.0f;//相机上下旋转速度
    public float cameraDampValue = 0.5f;

    private GameObject playerHandle;//用于获取PlayerHandle物件
    private GameObject cameraHandle;//用于获取cameraHandle物件
    private float tempEulerx;//用做欧拉角的变量
    private GameObject model;//用于获取model中的Kazuko模型
    private GameObject camera;//用于获取MainCamera物件

    private Vector3 cameraDampVelocity;

    // Start is called before the first frame update
    void Awake()
    {
        cameraHandle = transform.parent.gameObject;//取得MainCamera的父物体cameraHandle组件
        playerHandle = cameraHandle.transform.parent.gameObject;//取得MainCamera父亲的父亲也就是PlayerHandle物体
        tempEulerx = 0;
        model = playerHandle.GetComponent<ActorController>().model;//获取PlayerHandle的脚本中的model
        camera = Camera.main.gameObject;//获取MainCamera物件
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 tempModelEuler = model.transform.eulerAngles;//获取此时model的欧拉角

        playerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed * Time.fixedDeltaTime);//控制相机水平旋转

        //控制相机上下
        //cameraHandle.transform.Rotate(Vector3.right, pi.Jup * -verticalSpeed * Time.deltaTime);
        //tempEulerx = cameraHandle.transform.eulerAngles.x;
        tempEulerx -= pi.Jup * verticalSpeed * Time.deltaTime;//上下旋转的速度
        tempEulerx = Mathf.Clamp(tempEulerx, -20, 30);//Mathf.Clamp方法可以限制最小，最大值
        cameraHandle.transform.localEulerAngles =
            new Vector3(tempEulerx, 0, 0);

        //把上面获取的当时欧拉角传给model，这么做可以避免相机的左右移动的时候控制人物左右移动
        model.transform.eulerAngles = tempModelEuler;

        //把cameraPos的位置通过camera传给MainCamera,并用Lerp方法使相机追人物的效果平滑明显些（也可用SmoothDamp方法实现，下面有）
        //camera.transform.position = Vector3.Lerp(camera.transform.position, transform.position, 0.2f);
        //camera.transform.eulerAngles = transform.eulerAngles;//把cameraPos的欧拉角通过camera传给MainCamera

        camera.transform.position =
            Vector3.SmoothDamp(camera.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue);
        camera.transform.eulerAngles = transform.eulerAngles;
    }
}
