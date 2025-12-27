using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using LifeGame.UI; // ControlPanelController를 찾기 위해

namespace LifeGame.UI
{
    // ★ 클래스 선언이 반드시 있어야 합니다!
    public class SceneConfig : MonoBehaviour
    {
        // 1. 구조체 정의 (클래스 안에 존재해야 함)
        [System.Serializable]
        public struct ButtonInfo
        {
            public string buttonText;
            public UnityEvent onClickAction;
        }

        // 2. 리스트 변수
        public List<ButtonInfo> sceneButtons;

        private void Start()
        {
            // 3. 매니저에게 버튼 교체 요청
            if (ControlPanelController.Instance != null)
            {
                ControlPanelController.Instance.SetSceneButtons(sceneButtons);
            }
        }
    }
}