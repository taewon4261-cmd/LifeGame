using UnityEngine;

namespace LifeGame.UI
{
    public class GlobalCanvas : MonoBehaviour
    {
        public static GlobalCanvas Instance;

        private void Awake()
        {
            // 1. 싱글톤 패턴 적용: 나랑 똑같은 애가 이미 있으면, 나는 죽는다.
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // 2. 내가 최초라면, 나를 전역으로 등록하고 파괴되지 않게 한다.
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
