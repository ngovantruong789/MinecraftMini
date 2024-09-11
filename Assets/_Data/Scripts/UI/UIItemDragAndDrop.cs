using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIItemDragAndDrop : TruongMonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] UIMenu uIMenu;
    [SerializeField] bool isDrag;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        uIMenu = GetComponentInParent<UIMenu>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        UIItemInfor uIItemInfor = GetComponent<UIItemInfor>();
        if (!uIItemInfor.IsSelected) return;
        isDrag = true;
        Debug.Log(uIItemInfor.ItemName + ": drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDrag) return;

        UIItemInfor uIItemInfor = GetComponent<UIItemInfor>();
        if (!uIItemInfor.IsSelected) return;
        isDrag = false;

        SpawnBlock();
        Debug.Log(uIItemInfor.ItemName + ": drop");
    }

    void SpawnBlock()
    {
        BlockInteraction blockInteraction = FindObjectOfType<BlockInteraction>();
        if (blockInteraction == null) return;

        blockInteraction.CheckTouchBlock(true);
    }
}
