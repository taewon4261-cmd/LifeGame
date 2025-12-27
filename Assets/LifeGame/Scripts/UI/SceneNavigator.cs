using UnityEngine;
using UnityEngine.SceneManagement;

namespace LifeGame.UI
{
    public class SceneNavigator : MonoBehaviour
    {
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