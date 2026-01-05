namespace HabScene
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Unknown_Project;

    public class Hub_GameManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> cameraPositions = new List<GameObject>();
        [SerializeField] private List<int> backIndext = new List<int>();
        private int cameraPositionIndex = 0;
        [SerializeField] private CameraController cameraController;

        [SerializeField] private TextMeshProUGUI projectNameText;
        [SerializeField] private string[] projectNames;
        [SerializeField] private SceneSettingActivetor sceneSet;
        [SerializeField] private RuntimeDisplaySetting displaySetting;
        [SerializeField] private SceneSetting sceneSetting;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            InitCameraPos();
            HubSceneScreenSetting();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                cameraPositionIndex--;
                Debug.Log("index" + cameraPositionIndex);
                cameraPositionIndex = Mathf.Clamp(cameraPositionIndex, 0, cameraPositions.Count-1);
                cameraController.ChangePosition(cameraPositions[cameraPositionIndex].transform.position);

                projectNameText.text = $"ProjectName:{projectNames[cameraPositionIndex]}";
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                cameraPositionIndex++;
                Debug.Log("index" + cameraPositionIndex);
                cameraPositionIndex = Mathf.Clamp(cameraPositionIndex, 0, cameraPositions.Count-1);
                cameraController.ChangePosition(cameraPositions[cameraPositionIndex].transform.position);

                projectNameText.text = $"ProjectName:{projectNames[cameraPositionIndex]}";
            }
        }
        public void OnPushLeftArrow()
        {
            cameraPositionIndex--;
            cameraPositionIndex = Mathf.Clamp(cameraPositionIndex, 0, cameraPositions.Count-1);
            cameraController.ChangePosition(cameraPositions[cameraPositionIndex].transform.position);

            projectNameText.text = $"ProjectName:{projectNames[cameraPositionIndex]}";
        }
        public void OnPushRightArrow()
        {
            cameraPositionIndex++;
            cameraPositionIndex = Mathf.Clamp(cameraPositionIndex, 0, cameraPositions.Count-1);
            cameraController.ChangePosition(cameraPositions[cameraPositionIndex].transform.position);

            projectNameText.text = $"ProjectName:{projectNames[cameraPositionIndex]}";
        }

        public void ChangeScene(string sceneName)
        {
            Debug.Log("ÉVÅ[Éìà⁄ìÆÅF" + sceneName);
            PlayerPrefs.SetInt("LastCameraIndex", backIndext[cameraPositionIndex]);
            StartCoroutine(ChangeSceneCoroutine(sceneName));
        }
        private IEnumerator ChangeSceneCoroutine(string sceneName)
        {
            // ???????????????
            RuntimeDisplaySettingData screenSettings = displaySetting.GetScreenSettings(sceneName);

            // ???????
            sceneSetting.screenAngle = screenSettings.screenAngle;
            sceneSetting.fps = screenSettings.fps;
            yield return StartCoroutine(sceneSet.ScreenSetting());
            
            SceneManager.LoadScene(sceneName);
        }

        public void HubSceneScreenSetting()
        {
            RuntimeDisplaySettingData screenSettings = displaySetting.GetScreenSettings("hubScene");

            // ???????
            sceneSetting.screenAngle = screenSettings.screenAngle;
            sceneSetting.fps = screenSettings.fps;
            StartCoroutine(sceneSet.ScreenSetting());

        }

        private void InitCameraPos()
        {
            int n = PlayerPrefs.GetInt("LastCameraIndex",0);
            cameraPositionIndex = n;
            cameraController.ChangePosition(cameraPositions[cameraPositionIndex].transform.position);
            projectNameText.text = $"ProjectName:{projectNames[cameraPositionIndex]}";
        }
    }
}