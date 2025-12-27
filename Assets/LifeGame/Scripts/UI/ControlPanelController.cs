using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Events;

namespace LifeGame.UI
{
    public class ControlPanelController : MonoBehaviour
    {
        public static ControlPanelController Instance { get; private set; }

        [Header("UI Components")]
        [SerializeField] private List<Button> buttons;
        [SerializeField] private List<TextMeshProUGUI> buttonTexts;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // ★ SceneConfig 클래스 안에 있는 ButtonInfo 구조체를 사용한다고 명시
        public void SetSceneButtons(List<SceneConfig.ButtonInfo> newButtons)
        {
            // 1. 기존 버튼 초기화
            foreach (var btn in buttons)
            {
                btn.gameObject.SetActive(false);
                btn.onClick.RemoveAllListeners();
            }

            // 2. 새 버튼 등록
            for (int i = 0; i < newButtons.Count; i++)
            {
                if (i >= buttons.Count) break;

                int index = i;
                var info = newButtons[index];

                buttons[index].gameObject.SetActive(true);
                buttonTexts[index].text = info.buttonText;

                buttons[index].onClick.AddListener(() =>
                {
                    info.onClickAction?.Invoke();
                });
            }
        }
    }
}