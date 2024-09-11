using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenu : TruongMonoBehaviour
{
    private static UIMenu instance;
    public static UIMenu Instance => instance;

    protected override void Awake()
    {
        base.Awake();
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }

    protected override void Start()
    {
        base.Start();
        UIItemInfor uIItemInfor = transform.GetChild(0).GetComponent<UIItemInfor>();
        SetItemSelected(uIItemInfor.name);
    }

    //Set item đã chọn, nếu người dùng bấm vào item nào thì nó isSelected của nó = true, còn lại false
    public void SetItemSelected(string itemName)
    {
        foreach(Transform transform in transform)
        {
            UIItemInfor uIItemInfor = transform.GetComponent<UIItemInfor>();
            if (transform.name == itemName) uIItemInfor.SetIsSelected(true);
            else uIItemInfor.SetIsSelected(false);
        }
    }

    //Lấy tên mà item đang được chọn qua IsSelected
    public string GetItemSelected()
    {
        foreach(Transform transform in transform)
        {
            UIItemInfor uIItemInfor = transform.GetComponent<UIItemInfor>();
            if (uIItemInfor.IsSelected) return uIItemInfor.ItemName;
        }

        return null;
    }

    //Lấy thông tin item thông qua tên
    public UIItemInfor GetUIItemInfor(string itemName)
    {
        foreach (Transform transform in transform)
        {
            UIItemInfor uIItemInfor = transform.GetComponent<UIItemInfor>();
            if (uIItemInfor.ItemName == itemName) return uIItemInfor;
        }

        return null;
    }
}
