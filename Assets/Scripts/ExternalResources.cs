using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDHK_Tool.Component;
using SDHK_Tool.Static;


public class ExternalResources : MonoBehaviour
{

    static public ExternalResources instance;

    private void Awake()
    {
        instance = this;

        path = Application.streamingAssetsPath;


        Refresh();

    }

    public List<Texture2D>[] Textures = new List<Texture2D>[4];

    public List<string>[] TexturePaths = new List<string>[4];

    public Dictionary<int, GroupBoxData>[] GroupBoxDatas = new Dictionary<int, GroupBoxData>[4];


    private bool isUpdate = false;


    private string path;

    private int[] Num = new int[] { 0, 0, 0 };

    private int index;



    // Use this for initialization

    public void Refresh()
    {
        TexturePaths[3] = new List<string>();
        Textures[3] = new List<Texture2D>();
        GroupBoxDatas[0] = new Dictionary<int, GroupBoxData>();
        GroupBoxDatas[1] = new Dictionary<int, GroupBoxData>();
        GroupBoxDatas[2] = new Dictionary<int, GroupBoxData>();
        GroupBoxDatas[3] = new Dictionary<int, GroupBoxData>();

        Get_BookData();
        Get_MusicData();
        Get_VideoData();
        isUpdate = true;
    }

    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        Get_BG_Data();

    }


    public void Get_BookData()
    {
        Textures[0] = SS_File.GetFile_Texture2D(path + "/阅读", new string[] { "png", "jpg" });
        // TexturePaths[0] = SS_File.GetPaths_File(path + "/阅读", new string[] { "png", "jpg" });
    }

    public void Get_MusicData()
    {
        Textures[1] = SS_File.GetFile_Texture2D(path + "/音乐", new string[] { "png", "jpg" });
        // TexturePaths[1] = SS_File.GetPaths_File(path + "/音乐", new string[] { "png", "jpg" });
    }

    public void Get_VideoData()
    {
        Textures[2] = SS_File.GetFile_Texture2D(path + "/视频", new string[] { "png", "jpg" });
        // TexturePaths[2] = SS_File.GetPaths_File(path + "/视频", new string[] { "png", "jpg" });

        // SS_File.SetFile_JsonObject(TexturePaths,"1");
    }

    public void Get_BG_Data()
    {

        print(TexturePaths[3].Count);

    }




    public void FinishedLoading()
    {
        // GameManager.instance.GroupBG.BoxCount = TexturePaths[3].Count;
        GameManager.instance.GroupBG.BoxCount = Textures[3].Count;
        GameManager.instance.GroupBG.Refresh();
    }



    // Update is called once per frame
    void Update()
    {
        if (isUpdate)
        {


            index = Random.Range(0, 3);
            if (Num[0] < Textures[0].Count || Num[1] < Textures[1].Count || Num[2] < Textures[2].Count)
            {
                if (Num[index] < Textures[index].Count) Textures[3].Add(Textures[index][Num[index]++]);
                print("载入中");
            }
            else
            {
                isUpdate = false;
                print("载入完成");
                FinishedLoading();

            }
        }
    }
}

public class GroupBoxData //图片数据
{

    public int index = 0;

    public Texture2D Texture;

    public Vector2 localPosition;

    public Vector2 Size;
    public Vector2 BoxSize;


}

