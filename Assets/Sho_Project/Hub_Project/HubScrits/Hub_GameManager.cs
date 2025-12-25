namespace HabScene
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class Hub_GameManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> cameraPositions = new List<GameObject>();
        private int cameraPositionIndex = 0;
        [SerializeField] private CameraController cameraController;

        [SerializeField] private TextMeshProUGUI projectNameText;
        [SerializeField] private string[] projectNames;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                cameraPositionIndex--;
                cameraPositionIndex = Mathf.Clamp(cameraPositionIndex, 0, cameraPositions.Count);
                cameraController.ChangePosition(cameraPositions[cameraPositionIndex].transform.position);

                projectNameText.text = $"ProjectName:{projectNames[cameraPositionIndex]}";
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                cameraPositionIndex++;
                cameraPositionIndex = Mathf.Clamp(cameraPositionIndex, 0, cameraPositions.Count);
                cameraController.ChangePosition(cameraPositions[cameraPositionIndex].transform.position);

                projectNameText.text = $"ProjectName:{projectNames[cameraPositionIndex]}";
            }
        }
        public void ChangeScene(string sceneName)
        {
            Debug.Log("ÉVÅ[Éìà⁄ìÆÅF" + sceneName);
            SceneManager.LoadScene(sceneName);
        }

    }
}