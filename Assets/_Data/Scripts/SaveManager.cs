using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : TruongMonoBehaviour
{
    private static SaveManager instance;
    public static SaveManager Instance => instance;

    DataToSave dataToSave;
    string data;
    protected override void Awake()
    {
        base.Awake();
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        LoadData();
    }

    //Tải dữ liệu rồi đưa vào dataToSave
    public void LoadData()
    {
        //string data = File.ReadAllText(Application.dataPath + "/_Data/SaveData.json");
        if (PlayerPrefs.GetString("Data") == "") return;
        
        dataToSave = JsonConvert.DeserializeObject<DataToSave>(PlayerPrefs.GetString("Data"));
    }

    //Lưu dữ liệu
    //dataBlocksFromWorld chứa thông tin các block đã được đặt ra vì không convert được vector thành json nên phải dùng list
    //dataBlocks chứa thông tin về số lượng block hiện tại
    //vì không thể JsonConvert 1 Dictionary nên đành dùng list
    //Dùng PlayerPrefs thay vì tạo file vì lâu
    public void SaveData()
    {
        List<DataBlockFromWorld> dataBlocksFromWorld = new List<DataBlockFromWorld>();
        List<DataBlock> dataBlocks = new List<DataBlock>();
        BlockInteraction blockInteraction = FindObjectOfType<BlockInteraction>();
        if (blockInteraction == null) return;

        foreach (Transform transform in blockInteraction.Holder)
        {
            BlockInfor blockInfor = transform.GetComponent<BlockInfor>();
            DataBlockFromWorld dataBlockFromWorld = new DataBlockFromWorld();
            dataBlockFromWorld.blockID = blockInfor.BlockID;
            dataBlockFromWorld.blockName = blockInfor.BlockName;
            dataBlockFromWorld.blockPos.Add(blockInfor.transform.position.x);
            dataBlockFromWorld.blockPos.Add(blockInfor.transform.position.y);
            dataBlockFromWorld.blockPos.Add(blockInfor.transform.position.z);

            dataBlocksFromWorld.Add(dataBlockFromWorld);
        }

        foreach(Transform transform in blockInteraction.Blocks)
        {
            BlockInfor blockInfor = transform.GetComponent<BlockInfor>();
            DataBlock dataBlock = new DataBlock();
            dataBlock.blockName = blockInfor.BlockName;
            dataBlock.quantity = blockInfor.Quantity;
            dataBlocks.Add(dataBlock);
        }

        DataToSave dataToSave = new DataToSave(dataBlocksFromWorld, dataBlocks);
        PlayerPrefs.SetString("Data", JsonConvert.SerializeObject(dataToSave));
        //File.WriteAllText(Application.dataPath + "/_Data/SaveData.json", data);
    }

    public DataToSave GetDataToSave()
    {
        return dataToSave;
    }

    private void OnApplicationQuit()
    {
        //PlayerPrefs.DeleteKey("Data");
        SaveData();
    }
}

public class DataToSave
{
    public List<DataBlockFromWorld> dataBlocksFromWorld = new List<DataBlockFromWorld>();
    public List<DataBlock> dataBlocks = new List<DataBlock>();


    public DataToSave(List<DataBlockFromWorld> dataBlocksFromWorld, List<DataBlock> dataBlocks)
    {
        this.dataBlocksFromWorld = dataBlocksFromWorld;
        this.dataBlocks = dataBlocks;
    }
}

public class DataBlockFromWorld
{
    public int blockID;
    public string blockName;
    public List<float> blockPos = new List<float>();
}

public class DataBlock
{
    public string blockName;
    public int quantity;
}
