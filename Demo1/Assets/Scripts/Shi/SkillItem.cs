using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour {
    public float coldTime = 2; //技能冷却时间
    private float timer = 0; //计时器初始值
    private Image filledImage;
    private bool isStartTimer; //是否开始计算时间
    public KeyCode keycode;
    

    // Start is called before the first frame update
    void Start() {
        filledImage = transform.Find("FilledSkill").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(keycode)) { //当按下设定键后
            isStartTimer = true; //计时器开始执行
        }
        if (isStartTimer) { //如果计时器开始执行
            timer += Time.deltaTime; //计时器的时间开始往上累加
            filledImage.fillAmount = (coldTime - timer) / coldTime; //武器的阴影图按照比例增加
            if (timer >= coldTime) { //计时器的时间超过了技能冷却时间
                filledImage.fillAmount = 0; //武器的阴影图隐藏
                timer = 0; //计时器归零
                isStartTimer = false; //是否开始计算时间
            }
        }
    }

    public void onclick() {
        isStartTimer = true;
    }
}
