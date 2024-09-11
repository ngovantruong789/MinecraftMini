using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInteraction : TruongMonoBehaviour
{

    [SerializeField] protected List<Transform> blocks = new List<Transform>();
    public List<Transform> Blocks => blocks;

    [SerializeField] protected Transform holder;
    public Transform Holder => holder;

    protected override void Start()
    {
        SpawnBlockWhenBegin();
        SetBlockQuantity();
    }

    private void Update()
    {
        CheckTouchBlock(false);
    }

    #region LoadComponents
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadBlocks();
        holder = transform.Find("Holder");
    }

    protected void LoadBlocks()
    {
        Transform prefabs = transform.Find("Prefabs");
        if (prefabs == null) return;
        foreach (Transform transform in prefabs)
        {
            blocks.Add(transform);
            transform.gameObject.SetActive(false);
        }
    }
    #endregion LoadComponents

    //Dùng raycast để kiểm tra việc chạm vào
    //Nếu không chạm vào gì thì return
    //Nếu chạm vào không thuộc block thì return
    public void CheckTouchBlock(bool exception)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit)) return;
        if (!CheckBlock(hit)) return;

        SpawnBlock(hit, exception);
        RemoveBlock(hit);
    }

    //Nếu đây là block thì return true
    bool CheckBlock(RaycastHit hit)
    {
        BlockInfor blockInfor = GetBlockInforFromObject(hit);
        if (blockInfor == null) return false;
        return true;
    }

    //Nếu bấm chuột trái thì spawm
    //Lấy tên item đã chọn trong menu, sau đó mình sẽ lấy blockInfor từ list thông qua tên item đã chọn để thao tác
    //Nếu block từ list đó đã hết số lượng thì return
    //Lấy Transform block đang giữ blockInfor
    //Lấy toạ độ của block mà mình đã chạm vào
    //Tính toán là dùng toạ độ block đã chạm rồi + hoặc - tùy vào người chơi chạm vào cạnh nào của block
    //Spawn ra rồi đưa vào holder
    void SpawnBlock(RaycastHit hit, bool exception)
    {
        if (!Input.GetMouseButtonDown(0) && !exception) return;

        string blockNameSelected = UIMenu.Instance.GetItemSelected();

        BlockInfor blockInfor = GetBlockInforFromList(blockNameSelected);
        if (!blockInfor.CheckQuantity()) return;

        blockInfor.DetuctQuantity();
        
        Transform transBlock = blockInfor.transform;
        Vector3 pos = hit.collider.transform.parent.parent.position;
        string hitColliderName = hit.collider.name;

        if (hitColliderName == "Top")
            pos = new Vector3(pos.x, pos.y + 1f, pos.z);
        if (hitColliderName == "Down")
            pos = new Vector3(pos.x, pos.y - 1f, pos.z);
        if (hitColliderName == "Left")
            pos = new Vector3(pos.x - 1f, pos.y, pos.z);
        if (hitColliderName == "Right")
            pos = new Vector3(pos.x + 1f, pos.y, pos.z);
        if (hitColliderName == "Front")
            pos = new Vector3(pos.x, pos.y, pos.z - 1f);
        if (hitColliderName == "Behind")
            pos = new Vector3(pos.x, pos.y, pos.z + 1f);

        GameObject obj = Instantiate(transBlock.gameObject, pos, Quaternion.identity);
        obj.transform.SetParent(holder);
        obj.SetActive(true);

        SetBlockID(obj.GetComponent<BlockInfor>());
    }

    //Spawn các block đã có trước đó khi mở game, nếu không có block nào spawn ra thì khi mở game mặt định tạo 1 block sẵn trong scene để tương tác
    void SpawnBlockWhenBegin()
    {
        if (SaveManager.Instance.GetDataToSave() == null) return;

        List<DataBlockFromWorld> dataBlockFromWorlds = SaveManager.Instance.GetDataToSave().dataBlocksFromWorld;
        if(dataBlockFromWorlds.Count > 0) 
            foreach(Transform child in holder)
                Destroy(child.gameObject);

        foreach(DataBlockFromWorld data in dataBlockFromWorlds)
        {
            BlockInfor blockInforFromList = GetBlockInforFromList(data.blockName);
            if (blockInforFromList == null) continue;

            Vector3 pos = new Vector3(data.blockPos[0], data.blockPos[1], data.blockPos[2]);
            GameObject obj = Instantiate(blockInforFromList.gameObject, pos, Quaternion.identity);

            BlockInfor blockInfor = obj.GetComponent<BlockInfor>();
            blockInfor.SetBlockID(data.blockID);

            obj.transform.SetParent(holder);
            obj.SetActive(true);
            SetBlockID (obj.GetComponent<BlockInfor>());
        }
    }

    //Nếu không bấm chuột phải thì return
    //Lấy BlockInfor của block mà mình chạm vào
    //Lấy BlockInfor từ list dựa vào tên mà block mình chạm vào sau đó + vào số lượng rồi destroy block đã chạm
    void RemoveBlock(RaycastHit hit)
    {
        if (!Input.GetMouseButtonDown(1)) return;

        BlockInfor blockInforFromHit = GetBlockInforFromObject(hit);
        BlockInfor blockInforFromList = GetBlockInforFromList(blockInforFromHit.BlockName);
        if (blockInforFromList == null) return;
        blockInforFromList.AddQuantity(1);
        Destroy(hit.collider.transform.parent.parent.gameObject);
    }

    //Lấy BlockInfor từ list blocks
    BlockInfor GetBlockInforFromList(string blockName)
    {
        foreach (Transform transform in blocks)
        {
            BlockInfor blockInfor = transform.GetComponent<BlockInfor>();
            if (blockInfor == null) continue;
            if (blockInfor.BlockName != blockName) continue;

            return blockInfor;
        }

        return null;
    }

    //Lấy BlockInfor từ block mình chạm vào
    BlockInfor GetBlockInforFromObject(RaycastHit hit)
    {
        BlockInfor blockInfor = hit.collider.GetComponentInParent<BlockInfor>();
        return blockInfor;
    }

    public int GetQuantity(string blockName)
    {
        BlockInfor blockInfor = GetBlockInforFromList(blockName);
        return blockInfor.Quantity;
    }

    //Set số lượng các block hiện tại khi mở game
    public void SetBlockQuantity()
    {
        DataToSave dataToSave = SaveManager.Instance.GetDataToSave();
        if (dataToSave == null) return;
        foreach(DataBlock dataBlock in dataToSave.dataBlocks)
        {
            foreach(Transform transform in blocks)
            {
                BlockInfor blockInfor = transform.GetComponent<BlockInfor>();
                if (blockInfor.BlockName == dataBlock.blockName)
                    blockInfor.SetQuantity(dataBlock.quantity);
            }
        }
    }

    void SetBlockID(BlockInfor blockInfor)
    {
        for(int i = 0; i < holder.childCount; i++)
        {
            if (CheckID(i)) continue;

            blockInfor.SetBlockID(i);
            break;
        }
    }

    bool CheckID(int id)
    {
        foreach(Transform transform in holder)
        {
            BlockInfor blockInfor = transform.GetComponent<BlockInfor>();
            if (blockInfor == null) continue;
            if(blockInfor.BlockID == id) return true;
        }

        return false;
    }
}
