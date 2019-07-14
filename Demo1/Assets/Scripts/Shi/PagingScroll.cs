using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PagingScroll : MonoBehaviour , IBeginDragHandler, IEndDragHandler {
    public Toggle[] toggleArray;
    private ScrollRect scrollRect;

    public float smoothing = 5;
    private float targetVerticalPosition = 1;
    private float[] pageArray = new float[] { 1, 0.67f, 0.34f, 0};
    private bool isDraging = false;

    // Start is called before the first frame update
    void Start() {
        scrollRect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update() {
        if (isDraging == false) {
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, targetVerticalPosition, Time.deltaTime * smoothing);
        }
    }

    public void OnBeginDrag(PointerEventData eventDate) {
        isDraging = true;
    }

    public void OnEndDrag(PointerEventData eventDate) {
        isDraging = false;
        float posX = scrollRect.verticalNormalizedPosition;
        int index = 0;
        float offset = Mathf.Abs(pageArray[index] - posX);
        for (int i = 1; i < pageArray.Length; i++) {
            float offsetTemp = Mathf.Abs(pageArray[i] - posX);
            if (offsetTemp < offset) {
                offset = offsetTemp;
                index = i;
            }
        }
        targetVerticalPosition = pageArray[index];
        toggleArray[index].isOn = true;
    }

    public void TurnToPage1(bool isON) {
        if (isON) {
            targetVerticalPosition = pageArray[0];
        }
    }

    public void TurnToPage2(bool isON) {
        if (isON) {
            targetVerticalPosition = pageArray[1];
        }
    }

    public void TurnToPage3(bool isON) {
        if (isON) {
            targetVerticalPosition = pageArray[2];
        }
    }

    public void TurnToPage4(bool isON) {
        if (isON) {
            targetVerticalPosition = pageArray[3];
        }
    }
}
