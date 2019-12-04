using System.Collections;
using System.Collections.Generic;
using SDHK_Tool.Component;
using SDHK_Tool.Static;
using UnityEngine;

public class BoxProcessor_01 : SB_ScrollGroup_BoxProcessor
{
    public BoxNum boxNum;

    public Vector2 Limit_Size = Vector2.one;

    public Dictionary<GameObject, ImageBox> GroupBtnBoxPool = new Dictionary<GameObject, ImageBox>();

    public Dictionary<int, GroupBoxData> GroupBoxDatas = new Dictionary<int, GroupBoxData>();

    private SC_ScrollGroup scrollGroup;

    private void Awake()
    {

    }

    public override void RefreshGroup()
    {
        scrollGroup = GetComponent<SC_ScrollGroup>();
        scrollGroup.BoxCount = ExternalResources.instance.Textures[boxNum.Num[0]].Count;
        GroupBtnBoxPool.Clear();
        GroupBoxDatas = ExternalResources.instance.GroupBoxDatas[boxNum.Num[0]];
    }

    public override void GroupBox_New(GameObject GroupBox, int Index)
    {
        GroupBtnBoxPool.Add(GroupBox, GroupBox.GetComponent<ImageBox>());
        // print("组件池：" + GroupBtnBoxPool.Count + " 编号:" + Index);
    }

    public override void GroupBox_Del(GameObject GroupBox, int Index)
    {
        GroupBtnBoxPool.Remove(GroupBox);
    }

    public override void GroupBox_Work(GameObject GroupBox, int Index)
    {

        if (!GroupBoxDatas.ContainsKey(Index))
        {
            GroupBoxData groupBoxData = new GroupBoxData();

            groupBoxData.Texture = (ExternalResources.instance.Textures[boxNum.Num[0]])[Index];//先存到本地加载出来读取

            float RandomSize = Random.Range(Limit_Size.x, Limit_Size.y);//获取随机大小缩减

            Vector2 SpriteSize = SS_Mathf.Rect_ProperFormat(new Vector2(groupBoxData.Texture.width, groupBoxData.Texture.height), scrollGroup.BoxSize) * RandomSize;//随机大小
            Vector2 Box_localPosition = (scrollGroup.BoxSize - SpriteSize) * 0.5f;//获取移动位置最大值
            Vector2 RandomVector = SS_Mathf.Random_Vector2(-Box_localPosition, Box_localPosition);//随机位置

            groupBoxData.index = Index;
            groupBoxData.Size = SpriteSize;
            groupBoxData.BoxSize = scrollGroup.BoxSize;
            groupBoxData.localPosition = RandomVector;

            GroupBoxDatas.Add(Index, groupBoxData);

            // print("数据池：" + GroupBtnBoxPool.Count + " 编号:" + Index);
        }

        GroupBtnBoxPool[GroupBox].groupBoxData = GroupBoxDatas[Index];//数据替换
        GroupBtnBoxPool[GroupBox].Initialize();//刷新数据
    }

    public override void GroupBox_Idle(GameObject GroupBox, int Index)
    {

    }

}
