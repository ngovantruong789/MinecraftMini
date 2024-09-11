using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIItem", menuName = "ScriptableObject/UIItem")]
public class UIItemInforSO : ScriptableObject
{
    public int itemID;

    public string itemName;

    public Color itemDefaultColor;
    public Color itemSelectedColor;
}
