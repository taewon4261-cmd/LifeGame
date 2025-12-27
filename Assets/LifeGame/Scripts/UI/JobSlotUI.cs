using LifeGame.Jobs;
using LifeGame.Manager;
using System.Text;
using TMPro; // TextMeshPro 사용 권장 (가독성 및 성능 우수)
using UnityEngine;
using UnityEngine.UI;

namespace LifeGame.UI
{
    public class JobSlotUI : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private TextMeshProUGUI incomeText; // 변수 이름은 그대로 두되, 내용은 스탯을 표시
        [SerializeField] private Button selectButton;

        private JobSO _linkedJob;

        public void Setup(JobSO jobData)
        {
            _linkedJob = jobData;

            // 1. 이름과 설명 설정
            nameText.text = jobData.jobName;
            descText.text = jobData.description;

            // 2. [수정됨] 수입(incomePerAction) 대신 스탯 목록을 예쁘게 만들어서 표시
            StringBuilder statsText = new StringBuilder();

            // JobSO에 들어있는 모든 스탯을 한 줄씩 추가
            foreach (var stat in jobData.baseStats)
            {
                // 예: "MiningPower +10"
                statsText.AppendLine($"{stat.statType} +{stat.value}");
            }

            // 3. 텍스트 UI에 적용
            // 만약 스탯이 하나도 없으면 "기본 능력치 없음" 표시
            if (statsText.Length > 0)
                incomeText.text = statsText.ToString();
            else
                incomeText.text = "보너스 스탯 없음";
        }

        private void OnClicked()
        {
            // GameManager 싱글톤을 통해 선택된 직업 전달
            GameManager.Instance.OnJobSelected(_linkedJob);
        }
    }
}