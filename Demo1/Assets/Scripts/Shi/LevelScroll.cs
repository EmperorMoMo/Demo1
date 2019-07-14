using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelScroll : MonoBehaviour , IBeginDragHandler, IEndDragHandler {

    private ScrollRect scrollRect;
    public float smoothing = 5;
    public Toggle[] toggleArray;
    private float targetHorizontalPosition = 0;
    private bool isDraging = false;
    private float[] pageArray = new float[] { 0.00f, 0.33f, 0.66f, 1.00f };

    // Start is called before the first frame update
    void Start() {
        scrollRect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update() {
        if (isDraging == false) { 
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetHorizontalPosition, Time.deltaTime * smoothing);
        }
    }

    public void OnBeginDrag(PointerEventData eventDate) {
        isDraging = true;
    }

    public void OnEndDrag(PointerEventData evenDate) {
        isDraging = false;
        float posX = scrollRect.horizontalNormalizedPosition; //取得拖动结尾的水平坐标，EndDrag的点
        int index = 0; //预设页码
        float offset = Mathf.Abs(pageArray[index] - posX); //差值运算
        for (int i = 1; i < pageArray.Length; i++) {
            float offsetTemp = Mathf.Abs(pageArray[i] - posX);
            if (offsetTemp < offset) {
                index = i;
                offset = offsetTemp; //实现跳页(离Array点最近的位置)
            }
        }
        targetHorizontalPosition = pageArray[index];
        toggleArray[index].isOn = true;
        //scrollRect.horizontalNormalizedPosition = pageArray[index];
    }

    public void TurnTopage1(bool isOn) {
        if (isOn) {
            targetHorizontalPosition = pageArray[0]; //跳转到第1页
        }
    }
    public void TurnTopage2(bool isOn) {
        if (isOn) {
            targetHorizontalPosition = pageArray[1]; //跳转到第2页
        }
    }
    public void TurnTopage3(bool isOn) {
        if (isOn) {
            targetHorizontalPosition = pageArray[2]; //跳转到第3页
        }
    }
    public void TurnTopage4(bool isOn) {
        if (isOn) {
            targetHorizontalPosition = pageArray[3]; //跳转到第4页
        }
    }
}
