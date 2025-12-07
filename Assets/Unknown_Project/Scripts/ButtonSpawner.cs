using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpawner : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private Transform canvasTrans;

    List<bool>isSpawnSelectButton;


    private void Start()
    {
        isSpawnSelectButton = new List<bool>();
        for (int i = 0; i < LevelData.SELECTION_COUNT; i++)
        {
            isSpawnSelectButton.Add(false);
        }

    }

    public List<GameObject> SpawnSampleButton(int generateCount)
    {
        List<GameObject> buttons = new List<GameObject>();
        int buttonCount = levelData.ButtonPrefabs.Count;
        for(int i=0; i<generateCount; i++)
        {
            int randomIndex = Random.Range(0,buttonCount);
            GameObject obj = Instantiate(levelData.ButtonPrefabs[randomIndex],canvasTrans);
            
            Button buttonCompo = obj.GetComponent<Button>();
            buttonCompo.interactable = false;

            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(10000, 0); 
            buttons.Add(obj);
        }
        return buttons;
    }

    public List<GameObject> SpawnSelectButtonInit(int generateCount)
    {
        List<GameObject> buttons = new List<GameObject>();
        int buttonCount = levelData.ButtonPrefabs.Count;
        for(int i=0; i<generateCount; i++)
        {
            int randomIndex = Random.Range(0,buttonCount);
            int cnt = 0;
            while (isSpawnSelectButton[randomIndex])
            {
                randomIndex = Random.Range(0,buttonCount);
                cnt++;
                if(cnt > 1000)
                {
                    Debug.Log("Infinite loop");
                    break;
                }
            }
            isSpawnSelectButton[randomIndex] = true;
            GameObject obj = Instantiate(levelData.ButtonPrefabs[randomIndex],canvasTrans);

            Button buttonCompo = obj.GetComponent<Button>();
            buttonCompo.interactable = false;

            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(10000, 0); 

            buttons.Add(obj);
        }
        return buttons;
    }

    public GameObject SpawnSelectButton(GameObject genObj)
    {
        string genObjTag = genObj.tag;
        GameObject obj = null;
        foreach(var prefab in levelData.ButtonPrefabs)
        {
            if(prefab.tag == genObjTag)
            {
                obj = Instantiate(prefab,canvasTrans);
            }
        }

        Button buttonCompo = obj.GetComponent<Button>();
        buttonCompo.interactable = false;

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(10000, 0);
        return obj;
    }

}
