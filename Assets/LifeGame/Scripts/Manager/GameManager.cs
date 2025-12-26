using UnityEngine;
using LifeGame.Jobs;
using LifeGame.Player; // PlayerController가 있다고 가정
using LifeGame.UI;     // UI 네임스페이스 사용

namespace LifeGame.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("References")]
        public PlayerController player;

        // GameObject 대신 구체적인 스크립트 타입을 참조하여 접근성을 높입니다.
        public JobSelectionPanel jobSelectionPanel;
        public GameObject gameHUD;

        [Header("Starting Data")]
        public JobSO[] starterJobs;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            ShowJobSelection();
        }

        private void ShowJobSelection()
        {
            // 패널 활성화
            jobSelectionPanel.gameObject.SetActive(true);
            gameHUD.SetActive(false);

            // *핵심: UI 패널에게 데이터 목록을 넘겨주며 생성을 요청합니다.*
            jobSelectionPanel.InitJobMenu(starterJobs);
        }

        public void OnJobSelected(JobSO selectedJob)
        {
            Debug.Log($"[GameManager] 직업 선택됨: {selectedJob.jobName}");

            // 플레이어 초기화 (PlayerController에 Initialize 메서드가 있다고 가정)
            player.Initialize(selectedJob);

            // 화면 전환
            jobSelectionPanel.gameObject.SetActive(false);
            gameHUD.SetActive(true);
        }
    }
}