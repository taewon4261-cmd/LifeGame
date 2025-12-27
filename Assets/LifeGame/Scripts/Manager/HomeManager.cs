using LifeGame.Manager;
using LifeGame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LifeGame.Manager
{
    public class HomeManager : MonoBehaviour
    {
        // 1. [휴식] 체력 100% 회복
        public void Rest()
        {
            // 체력을 최대치로 설정
            GameManager.Instance.currentHp = GameManager.Instance.maxHp;

            // ★ UI 갱신 (중요!)
            // 데이터만 바꾸면 눈에 안 보이니까, UI한테도 "바꿔!"라고 말해줘야 합니다.
            if (GameManager.Instance.topUI != null)
            {
                GameManager.Instance.topUI.UpdateHpUI(
                    GameManager.Instance.currentHp,
                    GameManager.Instance.maxHp
                );
            }

            // 저장
            GameManager.Instance.SaveGame();

            Debug.Log("편안하게 휴식했습니다. 체력이 모두 회복되었습니다.");
        }

        // 2. [나가기] 마을로 이동
        // 이 기능은 SceneNavigator를 직접 연결해도 되지만,
        // 나중에 "나갈 때 문 소리 재생" 같은 걸 넣으려면 함수로 감싸는 게 좋습니다.
        public void GoOutside()
        {
            if (SceneNavigator.Instance != null)
            {
                SceneNavigator.Instance.GoToTown();
            }
            else
            {
                Debug.LogError("SceneNavigator를 찾을 수 없습니다! 타운 씬에서 게임을 시작했나요?");
            }
        }
    }

}
