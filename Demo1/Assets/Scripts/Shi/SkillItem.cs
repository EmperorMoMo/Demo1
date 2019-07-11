using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour {
    public float coldTime = 2; //技能冷却时间
    private float timer = 0; //计时器初始值
    private Image filledImage;
    private bool isStartTimer; //是否开始计算时间
    

    // Start is called before the first frame update
    void Start() {
        filledImage = transform.Find("FilledSkill").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {
        if (isStartTimer) {
            timer = Time.deltaTime;
            filledImage.fillAmount = (coldTime - timer) / coldTime;
        }
        if (timer >= coldTime) {
            filledImage.fillAmount = 0;
        }
    }

    public void onclick() {
        isStartTimer = true;
    }
}
