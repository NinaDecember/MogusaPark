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
    private List<GameObject> currentSelectButton;


    private void Start()
    {
        manager = FindFirstObjectByType<GameManager>();
        spawner = FindFirstObjectByType<ButtonSpawner>();
        ctrlB = FindFirstObjectByType<ButtonController>();

        sampleGenList = new Queue<List<GameObject>>();
        currentSelectButton = new List<GameObject>();

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
        
        currentSelectButton.AddRange(spawner.SpawnSelectButtonInit(LevelData.SELECTION_COUNT));
        
        ctrlB.ReDrawSelectionButtons(currentSelectButton);

        manager.loadClear[1] = true;
    }

    public void GenerateSelectionButton(int delIndex)
    {
        currentSelectButton.Add(spawner.SpawnSelectButton(delIndex));
        ctrlB.ReDrawSelectionButtons(currentSelectButton);
    }

    public void UpdateSelectButtons()
    {
        // currentSelectButton remove task
        int delIndex = 0;
        GenerateSelectionButton(delIndex);
    }




/// <summary>
/// 外部呼出し関数
/// </summary>

    // public bool IsOverlap(GameObject selectedButton, out List<GameObject> targetObjects)
    // {
    //     List<GameObject> sampleButtons = sampleGenList.Peek();
    //     targetObjects = new List<GameObject>();
    //     bool isExist = false;

    //     for(int buttonCol=0; buttonCol<levelData.samplePerRow; buttonCol++)
    //     {
    //         GameObject sampleButton = sampleButtons[buttonCol];
    //         if (sampleButton.GetComponent<RectTransform>().rect.Overlaps(selectedButton.GetComponent<RectTransform>().rect))
    //         {
    //             targetObjects.Add(sampleButton);
    //             isExist = true;
    //         }
    //     }

    //     return isExist;
    // }


    const int RECT_CORNER_COUNT = 4;
    public bool IsPlaceable(GameObject selectedObj, out GameObject targetObject)
    {
        targetObject = null;

        List<GameObject> targetObjects = sampleGenList.Peek();

        Vector3[] selectCorners = new Vector3[4];
        RectTransform rtSelect = selectedObj.GetComponent<RectTransform>();
        rtSelect.GetWorldCorners(selectCorners);


        //重複部分の面積を求める
        float maxOverlapArea = 0;
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
            if(maxOverlapArea == currentOverlapArea)targetObject = targetObj;
        }


        Vector2 size = rtSelect.rect.size;
        Vector3 scale = rtSelect.lossyScale;

        float worldWidth  = size.x * scale.x;
        float worldHeight = size.y * scale.y;

        float buttonArea = worldWidth * worldHeight;

        Debug.Log("buttonArea:"+buttonArea);
        Debug.Log("maxOverlapArea:"+maxOverlapArea);
        return maxOverlapArea > buttonArea * (1f-levelData.hitRate);
    }

    private bool IsCornerInside(RectTransform a, Vector3 corners)
    {
        // A のローカル座標に変換
        Vector2 localPos = a.InverseTransformPoint(corners);

        // Rect の中にあるか判定
        return a.rect.Contains(localPos);
    }


}
