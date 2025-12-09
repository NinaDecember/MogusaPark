namespace HabScene
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.SocialPlatforms.Impl;

    public class Hub_GameManager : MonoBehaviour
    {
        [SerializeField] private Vector3[] cameraPositions = new Vector3[5];
        private int cameraPositionIndex = 0;
        [SerializeField] private CameraController cameraController;
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
                cameraPositionIndex = Mathf.Clamp(cameraPositionIndex, 0, cameraPositions.Length);
                cameraController.ChangePosition(cameraPositions[cameraPositionIndex]);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                cameraPositionIndex++;
                cameraPositionIndex = Mathf.Clamp(cameraPositionIndex, 0, cameraPositions.Length);
                cameraController.ChangePosition(cameraPositions[cameraPositionIndex]);
            }
        }
        public void ChangeScene(string sceneName)
        {
            Debug.Log("ÉVÅ[Éìà⁄ìÆÅF" + sceneName);
            //SceneManager.LoadScene(sceneName);
        }

    }
}