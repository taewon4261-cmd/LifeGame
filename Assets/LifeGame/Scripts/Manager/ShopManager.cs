using UnityEngine;
using LifeGame.UI; // SceneNavigator 사용을 위해 필요

namespace LifeGame.Manager
{
    public class ShopManager : MonoBehaviour
    {
        // 나중에 물건 구매 기능이 여기에 추가될 예정입니다.
        // public void BuyItem(int itemId) { ... }

        // [기능] 마을로 돌아가기
        public void GoBackTown()
        {
            if (SceneNavigator.Instance != null)
            {
                SceneNavigator.Instance.GoToTown();
            }
            else
            {
                Debug.LogError("SceneNavigator가 없습니다. Town 씬에서 게임을 시작했나요?");
            }
        }
    }
}