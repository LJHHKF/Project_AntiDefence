using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropMoneyBar : MonoBehaviour
{
    private Camera uiCamera;
    private Canvas canvas;
    private RectTransform rectParent;
    private RectTransform rectMoney;

    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Vector3 TargetTr;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("UI_Canvas").GetComponent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectMoney = this.gameObject.GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(TargetTr + offset);
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }
        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
        rectMoney.localPosition = localPos;
    }
}
