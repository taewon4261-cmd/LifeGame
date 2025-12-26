using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용 권장 (가독성 및 성능 우수)
using LifeGame.Jobs;
using LifeGame.Manager;

namespace LifeGame.UI
{
    public class JobSlotUI : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private TextMeshProUGUI incomeText;
        [SerializeField] private Button selectButton;

        private JobSO _linkedJob;

        // 데이터를 받아서 UI를 갱신하는 메서드
        public void Setup(JobSO jobData)
        {
            _linkedJob = jobData;

            nameText.text = jobData.jobName;
            descText.text = jobData.description;
            // $ 문자열 보간을 사용하여 메모리 할당 최소화 (단, 빈번한 업데이트가 아니므로 허용)
            incomeText.text = $"수입: {jobData.incomePerAction}원";

            // 버튼 리스너 초기화 (중복 등록 방지)
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            // GameManager 싱글톤을 통해 선택된 직업 전달
            GameManager.Instance.OnJobSelected(_linkedJob);
        }
    }
}