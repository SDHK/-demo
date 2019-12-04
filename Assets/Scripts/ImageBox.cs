using System.Collections;
using System.Collections.Generic;
using SDHK_Tool.Component;
using SDHK_Tool.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBox : MonoBehaviour
{
    public float CenterRadius = 0.1f;

    public Transform Btn;//要控制的物体
    public GroupBoxData groupBoxData;//图片数据

    public SD_Motor_Vector3 Motor_Vector3;//移动电机

    public SD_Motor_Vector1 Motor_Vector1;//缩放电机

    private SC_Overlap_Box overlap_Box;//箱型碰撞检测器

    private RawImage rawImage;

    // private Collider NowCollider;


    private bool isStay = false;



    private void Awake()
    {
        Motor_Vector3 = new SD_Motor_Vector3();
        Motor_Vector3.Set_MotorSpeed(0.4f);

        Motor_Vector1 = new SD_Motor_Vector1();
        Motor_Vector1.Set_MotorValue(1);
        Motor_Vector1.Set_MotorSpeed(0.3f);

        if (Btn.GetComponent<RawImage>() != null) rawImage = Btn.GetComponent<RawImage>();

        if (GetComponent<SC_Overlap_Box>() != null) overlap_Box = GetComponent<SC_Overlap_Box>();

    }

    private void Start()
    {

    }

    private void Update()
    {
        if (isStay)
        {
            Motor_Vector3.Set_MotorSave(Btn.position).Run_SmoothDamp();
            Btn.position = Motor_Vector3.Get_MotorSave();
        }
        else
        {
            Motor_Vector3.Set_MotorSave(Btn.localPosition).Run_SmoothDamp();
            Btn.localPosition = Motor_Vector3.Get_MotorSave();
        }

        Motor_Vector1.Set_MotorSave(Btn.localScale.x).Run_SmoothDamp();
        Btn.localScale = Vector3.one * Motor_Vector1.Get_MotorSave();


    }


    public void OverlapStay(List<Collider> colliders)
    {
        // NowCollider = colliders.Find(a => a.tag == "a");

        isStay = true;

        Vector3 vector = (transform.position - colliders[0].transform.position);

        //!!!! ui坐标和世界坐标是不一样的 
        //!!!! 这里判断应改为判断到中心碰撞体(球形中心加个方形碰撞盒)而不是判断距离
        if (vector.magnitude < ((RectTransform)transform).sizeDelta.magnitude * CenterRadius)
        //colliders[0].transform.lossyScale.x * 0.5f * 0.2f
        {//检测进入中心
            Motor_Vector3.SetTarget_Vector(colliders[0].transform.position);
            Motor_Vector3.Set_MotorSpeed(0.3f);

            Motor_Vector1.SetTarget_Vector(6);//中心放大的大小
        }
        else
        {//检测外推
            Motor_Vector3.SetTarget_Vector((colliders[0].transform.lossyScale.x * 0.5f) * vector.normalized + colliders[0].transform.position);
            Motor_Vector3.Set_MotorSpeed(0.2f);

            Motor_Vector1.SetTarget_Vector(0.5f);//外推缩小
        }
    }


    public void OverlapExit(List<Collider> colliders)
    {//退出碰撞
        isStay = false;

        Motor_Vector3.SetTarget_Vector(groupBoxData.localPosition);
        Motor_Vector3.Set_MotorSpeed(1.5f);

        Motor_Vector1.SetTarget_Vector(1);//恢复尺寸

    }

    public void Initialize()//初始化
    {
        // rawImage.texture = null;
        // Resources.UnloadUnusedAssets();//释放无用对象

        rawImage.texture = groupBoxData.Texture;

        ((RectTransform)Btn).localPosition = groupBoxData.localPosition;

        ((RectTransform)Btn).sizeDelta = groupBoxData.Size;//设置内容盒子高宽

        Motor_Vector3.Set_MotorValue(Btn.localPosition);//位置电机重置

        if (overlap_Box != null)
        {
            overlap_Box.Center = groupBoxData.localPosition;

            overlap_Box.OnOverlapStay = OverlapStay;

            overlap_Box.OnOverlapExit = OverlapExit;
        }

    }
}
