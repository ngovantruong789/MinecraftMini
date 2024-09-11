using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemInfor : TruongMonoBehaviour
{
    [Header("Link Data")]
    [SerializeField] UIItemInforSO uIItemInforSO;
    public UIItemInforSO ItemInforSO => uIItemInforSO;

    [Header("Infor")]
    [SerializeField] int itemID;
    public int ItemID => itemID;

    [SerializeField] string itemName;
    public string ItemName => itemName;

    [SerializeField] bool isSelected;
    public bool IsSelected => isSelected;

    [Header("UI")]
    [SerializeField] Image blockImage;
    [SerializeField] TextMeshProUGUI textQuantity;
    [SerializeField] Color itemDefaultColor;
    [SerializeField] Color itemSelectedColor;

    #region LoadComponents
    //Load dữ liệu
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadUIItemInforSO();
        blockImage = transform.GetChild(0).Find("Block_Image").GetComponent<Image>();
        textQuantity = transform.GetComponentInChildren<TextMeshProUGUI>();

        itemID = uIItemInforSO.itemID;
        itemName = uIItemInforSO.itemName;
        itemDefaultColor = uIItemInforSO.itemDefaultColor;
        itemSelectedColor = uIItemInforSO.itemSelectedColor;
    }

    //Tìm scriptable tương ứng
    protected void LoadUIItemInforSO()
    {
        if (uIItemInforSO != null) return;
        string respath = "ScriptableObject/UIItemInfor/" + transform.GetChild(0).name;
        uIItemInforSO = Resources.Load<UIItemInforSO>(respath);
    }

    #endregion LoadComponents

    //Đổi màu block, nếu đang chọn thì màu selected, không thì default
    public void SetColorBlockImage()
    {
        if(isSelected) blockImage.color = itemSelectedColor;
        else blockImage.color = itemDefaultColor;
    }

    public void SetIsSelected(bool select)
    {
        isSelected = select;
        SetColorBlockImage();
    }

    //Hiển thị số lượng hiện tại
    public void SetTextQuantity(int quantity)
    {
        textQuantity.text = quantity.ToString();
    }
}
