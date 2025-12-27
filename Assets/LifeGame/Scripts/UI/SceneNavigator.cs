using UnityEngine;
using UnityEngine.SceneManagement;

namespace LifeGame.UI
{
    public class SceneNavigator : MonoBehaviour
    {
        public static SceneNavigator Instance { get; private set; }

        private void Awake()
        {
            // ★ 2. 싱글톤 초기화
            if (Instance == null)
            {
                Instance = this;
                // ControlPanel(부모)이 이미 DontDestroyOnLoad이므로 
                // 여기서는 별도로 DontDestroyOnLoad를 호출하지 않아도 됩니다.
            }
            else
            {
                // 중복 생성 방지
                Destroy(gameObject);
            }
        }

        [Header("Scene Configuration")]
        [SerializeField] private string MineSceneName = "Mine";
        [SerializeField] private string homeSceneName = "Home";
        [SerializeField] private string shopSceneName = "Shop";
        [SerializeField] private string townSceneName = "Town";

        public void GoToMine() => LoadSceneSafe(MineSceneName);
        public void GoHome() => LoadSceneSafe(homeSceneName);
        public void GoToShop() => LoadSceneSafe(shopSceneName);

        public void GoToTown() => LoadSceneSafe(townSceneName);

        public void OnNotImplementedClick()
        {
            Debug.Log("미구현 기능입니다.");
            // 만약 나중에 알림창을 띄운다면, 
          
        }

        private void LoadSceneSafe(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return;
            SceneManager.LoadScene(sceneName);
        }
    }
}