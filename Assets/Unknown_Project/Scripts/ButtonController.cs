using System;
using System.Collections.Generic;
using UnityEngine;




public class ButtonController : MonoBehaviour
{
    private LevelData levelData;

    [SerializeField] private GameObject centorSampleBack;
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

    private List<List<Vector2>>SamplePositions;


    private void Start()
    {
        levelData = FindFirstObjectByType<LevelData>();

        rt = centorSampleBack.GetComponent<RectTransform>();
        baseCenter = rt.anchoredPosition;
        baseHeight = rt.rect.height;
        baseWidth = rt.rect.width;
        SamplePositions = CulcSamplePositions();
    }

    const int SAMPLE_STEP_COUNT = 4;
    const double BASE_SCALE = 0.8;
    private List<List<Vector2>> CulcSamplePositions()
    {
        List<SampleSpaceData> sampleSpaceData = CulcSampleSpaceData();

        List<List<Vector2>> posList = new List<List<Vector2>>();

        for(int i=0; i<SAMPLE_STEP_COUNT; i++)
        {
            List<Vector2>rowPosList = new List<Vector2>();

            double x = 0;
            double y = sampleSpaceData[i].center.y;
            double interval = sampleSpaceData[i].width/levelData.samplePerRow;
            double leftSpace = interval/2;
            for(int j=0; j<levelData.samplePerRow; j++)
            {
                x = leftSpace + interval*j;
                rowPosList.Add(new Vector2((float)x,(float)y));
            }

            posList.Add(rowPosList);
        }
        return posList;
    }
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
            ssd.width = data[i-1].width * Math.Pow(BASE_SCALE,i);
            ssd.height = data[i-1].height * Math.Pow(BASE_SCALE,i);
            
            double centerX = baseCenter.x;
            
            double lastTopY;{
                double lastCenterY = data[i-1].center.y;
                double lastHeight = data[i-1].height;
                lastTopY = lastCenterY + lastHeight/2;
            }
            double currentHeightHalf = ssd.height/2;
            double currentCentorY = lastTopY + currentHeightHalf;

            ssd.center = new Vector2((float)centerX,(float)currentCentorY);



            data.Add(ssd);
            
        }
        return data;
    }

    private void Update()
    {
        
    }

    public Vector2 GetButtonPos(int step, int col)
    {
        return SamplePositions[step][col];
    }

}
