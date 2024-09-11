using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfor : TruongMonoBehaviour
{
    [SerializeField] private int blockID = -1;
    public int BlockID => blockID;

    [SerializeField] private string blockName;
    public string BlockName => blockName;

    [SerializeField] int quantity = 10;
    public int Quantity => quantity;

    [SerializeField] List<Transform> blockColliders = new List<Transform>();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadBlockColliders();
    }

    //Load các cạnh của block
    void LoadBlockColliders()
    {
        Transform colliders = transform.Find("Colliders");
        foreach (Transform transform in colliders)
            blockColliders.Add(transform);
    }

    public void SetBlockID(int id)
    {
        blockID = id;
    }

    public void AddQuantity(int value)
    {
        quantity += value;
        ShowQuantityFromUI();
    }

    public void DetuctQuantity()
    {
        if(quantity <= 0) quantity = 0;
        else quantity--;

        ShowQuantityFromUI();
    }

    public void SetQuantity(int value)
    {
        quantity = value;
        ShowQuantityFromUI();
    }
    public bool CheckQuantity()
    {
        if(quantity <= 0) return false;
        return true;
    }

    void ShowQuantityFromUI()
    {
        UIMenu.Instance.GetUIItemInfor(blockName).SetTextQuantity(quantity);
    }
}
