using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDropMoneyBar : MonoBehaviour
{
    private Camera uiCamera;
    private Canvas canvas;
    private RectTransform rectParent;
    private RectTransform rectMoney;

    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Vector3 TargetTr;

    private Text txt_money;
    private int dropMoneyValue;
    private bool is_text_Updated;
    private float time_liv = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("UI_Canvas").GetComponent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectMoney = this.gameObject.GetComponent<RectTransform>();

        txt_money = gameObject.transform.Find("Text").GetComponent<Text>();
        
    }

    private void OnEnable()
    {
        is_text_Updated = false;
    }

    void LateUpdate()
    {
        if (is_text_Updated == false)
        {
            txt_money.text = dropMoneyValue.ToString();
            StartCoroutine(Off());
            is_text_Updated = true;
        }

        var screenPos = Camera.main.WorldToScreenPoint(TargetTr + offset);
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out Vector2 localPos);
        rectMoney.localPosition = localPos;

        TargetTr.y += 0.01f;
    }

    public void SetDropMoney(int value)
    {
        dropMoneyValue = value;
    }

    public void SetTime(float sec)
    {
        time_liv = sec;
    }

    IEnumerator Off()
    {
        yield return new WaitForSeconds(time_liv);
        gameObject.SetActive(false);
        yield break;
    }
}
