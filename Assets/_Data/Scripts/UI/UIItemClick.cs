using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIItemClick : TruongMonoBehaviour, IPointerClickHandler
{
    [SerializeField] UIMenu uIMenu;
    public UIMenu UIMenu => uIMenu;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        uIMenu = transform.parent.GetComponent<UIMenu>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        uIMenu.SetItemSelected(transform.name);
    }
}
