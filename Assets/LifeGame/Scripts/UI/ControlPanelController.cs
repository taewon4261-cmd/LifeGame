using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections.Generic;

namespace LifeGame.UI
{
    
    [System.Serializable] 
    public struct ButtonData
    {
        public string buttonText;
        public UnityEvent onClickAction;
    }

  
    public class ControlPanelController : MonoBehaviour
    {
        public static ControlPanelController Instance { get; private set; }

        [SerializeField] private Button[] buttons;
        [SerializeField] private TextMeshProUGUI[] buttonTexts;
        private void Awake()
        {
            // 게임 시작 시 "내가 바로 그 담당자다"라고 등록
            if (Instance == null)
            {
                Instance = this;
                // 씬이 넘어가도 파괴되지 않게 하려면 아래 줄 주석 해제
                // DontDestroyOnLoad(gameObject); 
            }
            else
            {
                // 만약 실수로 2개가 생기면 나중에 생긴 놈을 파괴 (중복 방지)
                Destroy(gameObject);
                return;
            }
        }

        public void InitializeButtons(List<ButtonData> dataList)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i < dataList.Count)
                {
                    buttons[i].gameObject.SetActive(true);
                    buttonTexts[i].text = dataList[i].buttonText;

                    // 기존 연결 끊고 새로 연결
                    buttons[i].onClick.RemoveAllListeners();
                    buttons[i].onClick.AddListener(() => dataList[i].onClickAction.Invoke());
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}