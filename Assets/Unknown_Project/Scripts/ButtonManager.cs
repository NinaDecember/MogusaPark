using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    private GameManager manager;
    private ButtonController ctrlB;
    private ButtonSpawner spawner;
    private Queue<List<GameObject>>sampleGenList;
    private List<GameObject> selectableButtons;
    private GameObject[] setButtons;


    private void Start()
    {
        manager = FindFirstObjectByType<GameManager>();
        spawner = FindFirstObjectByType<ButtonSpawner>();
        ctrlB = FindFirstObjectByType<ButtonController>();

        sampleGenList = new Queue<List<GameObject>>();
        selectableButtons = new List<GameObject>();
        setButtons = new GameObject[levelData.samplePerRow];

    }










/// <summary>
/// Sample Button
/// </summary>


    public void GenerateSampleButtonInit()
    {
        for(int i=0; i<LevelData.SAMPLE_STEP_COUNT; i++)
        {
            List<GameObject> nowStepButtonPrefabs = spawner.SpawnSampleButton(levelData.samplePerRow);
            sampleGenList.Enqueue(nowStepButtonPrefabs);
        }
        ctrlB.ReDrawSampleButtons(sampleGenList);

        manager.loadClear[0] = true;
    }

    public void GenerateSampleButton()
    {
        List<GameObject> nowStepButtonPrefabs = spawner.SpawnSampleButton(levelData.samplePerRow);
        sampleGenList.Enqueue(nowStepButtonPrefabs);
        ctrlB.ReDrawSampleButtons(sampleGenList);
    }




    public void UpdateSampleButtons()
    {
        sampleGenList.Dequeue();
        GenerateSampleButton();
    }








/// <summary>
/// Selection Button
/// </summary>

    public void GenerateSelectionButtonInit()
    {
        
        selectableButtons.AddRange(spawner.SpawnSelectButtonInit(LevelData.SELECTION_COUNT));
        
        ctrlB.ReDrawSelectionButtons(selectableButtons);

        manager.loadClear[1] = true;
    }

    public void UpdateSelectButtons(int delIndex, GameObject genObj)
    {
        Debug.Log("delIndex:"+delIndex);
    
        selectableButtons[delIndex] = spawner.SpawnSelectButton(genObj);
        ctrlB.ReDrawSelectionButtons(selectableButtons);
    }




/// <summary>
/// 外部呼出し関数
/// </summary>

    public bool TrySet(GameObject selectObj, out GameObject targetObj, out int targetIndex)
    {
        bool isPlaceable = IsPlaceableRange(selectObj, out targetObj, out targetIndex);
        Debug.Log("Range:"+isPlaceable);
        if (!isPlaceable)
        {
            return false;
        }

        isPlaceable = IsPlaceableType(selectObj,targetObj);
        Debug.Log("Type:"+isPlaceable);
        if (!isPlaceable)
        {
            return false;
        }

        isPlaceable = IsNotDuplicated(targetIndex);
        Debug.Log("Duplicated:"+isPlaceable);
        if (!isPlaceable)
        {
            return false;
        }

        return true;

    }

    const int RECT_CORNER_COUNT = 4;
    public bool IsPlaceableRange(GameObject selectedObj, out GameObject targetObject, out int index)
    {
        targetObject = null;
        index = 0;

        List<GameObject> targetObjects = sampleGenList.Peek();

        Vector3[] selectCorners = new Vector3[4];
        RectTransform rtSelect = selectedObj.GetComponent<RectTransform>();
        rtSelect.GetWorldCorners(selectCorners);


        //重複部分の面積を求める
        float maxOverlapArea = 0;
        int cnt = 0;
        foreach(var targetObj in targetObjects)
        {
            float currentOverlapArea = 0;

            Vector3[] targetCorners = new Vector3[4];
            RectTransform rtTarget = targetObj.GetComponent<RectTransform>();
            rtTarget.GetWorldCorners(targetCorners);

            
            //0->3 : 左下->左上->右上->右下
            for(int rectCornerNum=0; rectCornerNum < RECT_CORNER_COUNT; rectCornerNum++)
            {
                int selectObjSidePosIndex = (rectCornerNum+2)%4;
                Vector3 selectObjSidePos = selectCorners[selectObjSidePosIndex];
                Vector3 targetObjSidePos = targetCorners[rectCornerNum];

                if (IsCornerInside(rtTarget,selectObjSidePos))
                {
                    Vector3[] corners = {targetObjSidePos,selectObjSidePos};
                    Vector3 leftTop = Vector3.zero;
                    Vector3 rightButtom = Vector3.zero;

                    int leftTopIndex = (rectCornerNum%RECT_CORNER_COUNT) / (RECT_CORNER_COUNT/2);
                    leftTop.x = corners[leftTopIndex].x;
                    rightButtom.x = corners[1-leftTopIndex].x;
                    int rectCornerNumTemp = (rectCornerNum == 0 ? RECT_CORNER_COUNT-1 : rectCornerNum);
                    int rightButtomIndex = ((rectCornerNumTemp % RECT_CORNER_COUNT) - 1) / (RECT_CORNER_COUNT/2);
                    leftTop.y = corners[rightButtomIndex].y;
                    rightButtom.y = corners[1-rightButtomIndex].y;
                    
                    //width,height
                    float[] rectSize = {rightButtom.x-leftTop.x, leftTop.y-rightButtom.y};


                    currentOverlapArea = rectSize[0] * rectSize[1];
                    
                    break;
                }


            }

            maxOverlapArea = Math.Max(maxOverlapArea,currentOverlapArea);
            if(maxOverlapArea == currentOverlapArea){
                targetObject = targetObj;
                index = cnt;
            }

            cnt++;
        }


        Vector2 size = rtSelect.rect.size;
        Vector3 scale = rtSelect.lossyScale;

        float worldWidth  = size.x * scale.x;
        float worldHeight = size.y * scale.y;

        float buttonArea = worldWidth * worldHeight;

        // Debug.Log("buttonArea:"+buttonArea);
        // Debug.Log("maxOverlapArea:"+maxOverlapArea);
        return maxOverlapArea > buttonArea * (1f-levelData.hitRate);
    }

    private bool IsCornerInside(RectTransform a, Vector3 corners)
    {
        // A のローカル座標に変換
        Vector2 localPos = a.InverseTransformPoint(corners);

        // Rect の中にあるか判定
        return a.rect.Contains(localPos);
    }


    private bool IsPlaceableType(GameObject selectObj, GameObject targetObj)
    {
        if(selectObj.tag == targetObj.tag)return true;
        return false;
    }


    private bool IsNotDuplicated(int targetIndex)
    {
        return !setButtons[targetIndex];
    }



    public bool IsStepClear()
    {
        return true;
    }






    public void AddSetButtonsList(int target, GameObject selectedButton)
    {
        setButtons[target] = selectedButton;

        int cnt = 0;
        foreach(var button in selectableButtons)
        {
            if(button == selectedButton)break;
            cnt++;
        }

        UpdateSelectButtons(cnt,selectedButton);
    }
    public void ResetSetButtonsList()
    {
        setButtons = new GameObject[levelData.samplePerRow];
    }


}
