using UnityEngine;
using TMPro;
using UnityEngine.UI; // 텍스트 매쉬 프로 필수

namespace LifeGame.UI
{
    public class TopUI : MonoBehaviour
    {
        [Header("Money UI")]
        [SerializeField] private TextMeshProUGUI moneyText; // 인스펙터에서 텍스트 연결

        [Header("HP UI")]
        [SerializeField] private Image hpBarImage;       
        [SerializeField] private TextMeshProUGUI hpText;

        // 매니저가 이 함수를 호출해서 화면을 갱신합니다.
        public void UpdateMoneyText(long currentMoney)
        {
            // N0 포맷: 1,000,000 처럼 3자리마다 콤마를 찍어줍니다.
            if (moneyText != null)
            {
                moneyText.text = $"{currentMoney:N0} won";
            }
        }
        public void UpdateHpUI(float current, float max)
        {
            if (hpBarImage != null)
            {
                // 현재 체력 / 최대 체력 = 0.0 ~ 1.0 사이의 비율값
                hpBarImage.fillAmount = current / max;
            }

            if (hpText != null)
            {
                hpText.text = $"{current:F0} / {max:F0}";
            }
        }
    }
}