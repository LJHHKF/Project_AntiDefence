using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDropMoneySet : MonoBehaviour
{
    public GameObject dropMoney_Prefab;
    private Canvas uiCanvas;
    private Text txt_money;
    private GameObject dropMoney;
    private EnemyDropMoneyBar _dropMoney;

    //[HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Vector3 TargetTr;
    private int dropMoneyValue;

    // Start is called before the first frame update
    void OnEnable()
    {
        uiCanvas = GameObject.FindGameObjectWithTag("UI_Canvas").GetComponent<Canvas>();
        dropMoney = Instantiate(dropMoney_Prefab, uiCanvas.transform);
        txt_money = dropMoney.transform.Find("Text").GetComponent<Text>();

        _dropMoney = dropMoney.GetComponent<EnemyDropMoneyBar>();
        _dropMoney.TargetTr = TargetTr;
        //_dropMoney.offset = offset;
        

        Destroy(dropMoney, 1.0f);
        Destroy(gameObject, 1.0f);
    }

    public void SetDropMoney(int value)
    {
        dropMoneyValue = value;
    }

    private void Start()
    {
        txt_money.text = dropMoneyValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        TargetTr.y += 0.01f;
        _dropMoney.TargetTr = TargetTr;
    }
}
