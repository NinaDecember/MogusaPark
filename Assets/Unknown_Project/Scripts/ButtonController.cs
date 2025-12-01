using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class ButtonController : MonoBehaviour
{
    private ButtonManager managerB;
    [SerializeField] private LevelData levelData;

    [SerializeField] private GameObject centorSampleBack;
    [SerializeField] private GameObject sampleSpacePrefab;
    [SerializeField] private Transform canvasTransform;
    private RectTransform rt;

    private double baseWidth;
    private double baseHeight;
    private Vector2 baseCenter;
    private struct SampleSpaceData
    {
        public double width;
        public double height;
        public Vector2 center;
    }

    private List<SampleSpaceData> sampleSpaceList;
    private List<List<Vector2>>samplePositions;
    private List<GameObject>sampleBack;
    private List<Vector2> selectButtonPositions;


    private void Start()
    {
        managerB = FindFirstObjectByType<ButtonManager>();

        rt = centorSampleBack.GetComponent<RectTransform>();
        baseCenter = rt.anchoredPosition;
        baseHeight = rt.rect.height;
        baseWidth = rt.rect.width;
        sampleSpaceList = CulcSampleSpaceData();
        samplePositions = CulcSamplePositions();
        selectButtonPositions = CulcSelectButtonPositions();
        DrawSampleSpace();
    }





    private int SAMPLE_STEP_COUNT = LevelData.SAMPLE_STEP_COUNT;
    const double BASE_SCALE = 0.68;
    private List<SampleSpaceData> CulcSampleSpaceData()
    {
        List<SampleSpaceData> data = new List<SampleSpaceData>();

        //1行目だけ例外
        SampleSpaceData ssd1 = new SampleSpaceData();
        ssd1.width = baseWidth;
        ssd1.height = baseHeight;
        ssd1.center = baseCenter;
        data.Add(ssd1);
        for(int i=1; i<SAMPLE_STEP_COUNT; i++)
        {
            SampleSpaceData ssd = new SampleSpaceData();
            // ssd.width = data[i-1].width * Math.Pow(BASE_SCALE,i);
            // ssd.height = data[i-1].height * Math.Pow(BASE_SCALE,i);
            ssd.width = data[i-1].width * BASE_SCALE;
            ssd.height = data[i-1].height * BASE_SCALE;
            
            double centerX = baseCenter.x;
            
            double lastTopY;{
                double lastCenterY = data[i-1].center.y;
                double lastHeight = data[i-1].height;
                lastTopY = lastCenterY + lastHeight/2;
            }
            Debug.Log(lastTopY);
            double currentHeightHalf = ssd.height/2;
            Debug.Log(currentHeightHalf);
            double currentCentorY = lastTopY + currentHeightHalf;
            Debug.Log(currentCentorY);
            Debug.Log("--------------------");

            ssd.center = new Vector2((float)centerX,(float)currentCentorY);



            data.Add(ssd);
            
        }
        return data;
    }
    private List<List<Vector2>> CulcSamplePositions()
    {
        List<List<Vector2>> posList = new List<List<Vector2>>();

        for(int i=0; i<SAMPLE_STEP_COUNT; i++)
        {
            List<Vector2>rowPosList = new List<Vector2>();

            double x = 0;
            double y = sampleSpaceList[i].center.y;
            double interval = sampleSpaceList[i].width/levelData.samplePerRow;
            double leftSpace = interval/2;
            double leftX = -sampleSpaceList[i].width/2;
            for(int j=0; j<levelData.samplePerRow; j++)
            {
                x = leftX + leftSpace + interval*j;
                rowPosList.Add(new Vector2((float)x,(float)y));
            }

            posList.Add(rowPosList);
        }
        return posList;
    }

    private List<Vector2> CulcSelectButtonPositions()
    {
        int selectCount = LevelData.SELECTION_COUNT;
        const double RIGHT_X = 880;
        SampleSpaceData ssd = new SampleSpaceData();
        ssd.center = new Vector2(0,-300);//この位置固定
        ssd.width = RIGHT_X*2;
        ssd.height = ssd.center.y;


        List<Vector2> pos = new List<Vector2>();
        double interval = ssd.width/selectCount;
        double leftSpace = interval/2;
        double leftX = -ssd.width/2;

        double x = 0;
        double y = ssd.height;
        for(int i=0; i<selectCount; i++)
        {
            x = leftX + leftSpace + interval*i;
            pos.Add(new Vector2((float)x,(float)y));
        }

        return pos;
    }

    private void DrawSampleSpace()
    {
        sampleBack = new List<GameObject>();
        for(int i=LevelData.SAMPLE_STEP_COUNT-1; i>0; i--)
        {
            Debug.Log(i);
            GameObject back = Instantiate(sampleSpacePrefab,canvasTransform);
            RectTransform rt = back.GetComponent<RectTransform>();
            rt.anchoredPosition = sampleSpaceList[i].center;    
            rt.localScale = Vector3.one *(float) Math.Pow(BASE_SCALE,i);
            sampleBack.Add(back);
        }

    }








    public void BCUpdate()//ButtonController:BC
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            managerB.UpdateButtons();
        }

    }

    public Vector2 GetButtonPos(int step, int col)
    {
        return samplePositions[step][col];
    }

    public void ReDrawSample(Queue<List<GameObject>> buttons)
    {
        int rowCnt = 0;
        foreach(var rowButtons in buttons)
        {
            int colCnt = 0;
            foreach(var button in rowButtons)
            {
                RectTransform rt = button.GetComponent<RectTransform>();
                
                Vector2 pos = rt.anchoredPosition;
                pos = samplePositions[rowCnt][colCnt];
                rt.anchoredPosition = pos;

                rt.localScale = Vector3.one *(float) Math.Pow(BASE_SCALE,rowCnt);

                colCnt++;
            }
            rowCnt++;
        }
    }


    public void ReDrawSelection(List<GameObject> selectButtons)
    {
        int cnt = 0;
        Debug.Log("selectButtonSum:" + selectButtonPositions.Count);
        foreach(var button in selectButtons)
        {
            RectTransform rt = button.GetComponent<RectTransform>();
            rt.anchoredPosition = selectButtonPositions[cnt];
            cnt++;
        }
    }

}
