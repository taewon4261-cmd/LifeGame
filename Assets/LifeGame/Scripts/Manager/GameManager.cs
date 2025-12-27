using LifeGame.Jobs;
using LifeGame.Player; // PlayerController가 있다고 가정
using LifeGame.UI;     // UI 네임스페이스 사용
using UnityEngine;
using System;
using System.Collections.Generic;

namespace LifeGame.Manager
{
    public enum StatType
    {
        None = 0,
        MiningPower,  // 채광력
        MiningSpeed,  // 채광 속도
        AttackPower,  // 전투 공격력
        TradingSkill, // 상술 (상점 할인 등)
        Stamina,      // 체력
        Luck          // 행운
    }

    public class GameManager : MonoBehaviour
    {
        // --- [싱글톤 패턴] ---
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // 씬 이동 시 파괴 안 됨

                InitializeStats(); // 스탯 통 초기화
                LoadGame();        // 저장된 데이터 불러오기
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // --- [데이터 변수들] ---
        [Header("Economy Data")]
        public long money = 0;
        public float currentHp = 100f;
        public float maxHp = 100f;

        // ★ 핵심: 모든 능력치를 담는 만능 주머니 (Dictionary)
        private Dictionary<StatType, float> playerStats = new Dictionary<StatType, float>();

        // --- [참조 변수들 (UI, Player)] ---
        [Header("References")]
        public PlayerController player;
        public JobSelectionPanel jobSelectionPanel; // 직업 선택 UI
        public TopUI topUI;

        public GameObject gameHUD;                // 게임 플레이 UI (돈, 체력바 등)

        [Header("Starting Data")]
        public JobSO[] starterJobs;

        private void Start()
        {
            // 게임 시작 시 직업 선택창 로직 (데이터가 없을 때만 띄우거나 항상 띄우거나 정책에 따라)
            // 여기서는 저장된 직업이 없다고 가정하고 선택창을 띄웁니다.
            ShowJobSelection();

            if (topUI != null)
            {
                topUI.UpdateMoneyText(money);
                topUI.UpdateHpUI(currentHp, maxHp); // ★ 시작할 때 체력바 세팅
            }
        }

        // --- [UI 및 직업 선택 로직] ---

        private void ShowJobSelection()
        {
            if (jobSelectionPanel != null)
            {
                jobSelectionPanel.gameObject.SetActive(true);
                jobSelectionPanel.InitJobMenu(starterJobs); // UI에 직업 목록 전달
            }

            if (gameHUD != null)
                gameHUD.SetActive(false); // HUD 숨김
        }

        public void OnJobSelected(JobSO selectedJob)
        {
            Debug.Log($"[GameManager] 직업 선택됨: {selectedJob.jobName}");

            // 1. 플레이어 초기화 (PlayerController에 로직이 있다면)
            if (player != null) player.Initialize(selectedJob);

            // 2. ★ 직업별 스탯 보너스 적용 로직
            ApplyJobStats(selectedJob);

            // 3. UI 전환
            if (jobSelectionPanel != null) jobSelectionPanel.gameObject.SetActive(false);
            if (gameHUD != null) gameHUD.SetActive(true);

            SaveGame(); // 직업 선택했으니 저장
        }

        private void ApplyJobStats(JobSO job)
        {
            // (선택 사항) 직업 바꿀 때 스탯 초기화가 필요하면 InitializeStats() 호출

            foreach (var bonus in job.baseStats)
            {
                AddStat(bonus.statType, bonus.value);
            }
        }

        // --- [스탯 시스템 (Dictionary 관리)] ---

        // 딕셔너리에 Enum 키들을 미리 만들어두는 함수
        private void InitializeStats()
        {
            foreach (StatType type in Enum.GetValues(typeof(StatType)))
            {
                if (!playerStats.ContainsKey(type))
                {
                    playerStats.Add(type, 0f); // 기본값 0으로 생성
                }
            }
        }

        // 스탯 가져오기 (외부에서 사용: GameManager.Instance.GetStat(StatType.MiningPower))
        public float GetStat(StatType type)
        {
            if (playerStats.ContainsKey(type)) return playerStats[type];
            return 0f;
        }

        // 스탯 증가시키기
        public void AddStat(StatType type, float amount)
        {
            if (playerStats.ContainsKey(type))
            {
                playerStats[type] += amount;
                // UI 갱신 이벤트가 있다면 여기서 호출
            }
        }

        // --- [경제 및 체력 로직] ---

        public void AddMoney(long amount)
        {
            money += amount;
            Debug.Log($"[GameManager] 돈 획득: {amount} (Total: {money})");

            if (topUI != null)
            {
                topUI.UpdateMoneyText(money);
            }
            SaveGame();
        }

        public bool ConsumeHp(float amount)
        {
            if (currentHp >= amount)
            {
                currentHp -= amount;

                // ★ [추가] 체력이 줄어들었으니 UI도 갱신해라!
                if (topUI != null) topUI.UpdateHpUI(currentHp, maxHp);

                return true; // 성공
            }

            // 실패 (체력 부족)
            Debug.Log("체력이 부족합니다!");
            return false;
        }

        // --- [저장 및 불러오기 (PlayerPrefs)] ---

        public void SaveGame()
        {
            // 1. 기본 재화 저장
            PlayerPrefs.SetString("Money", money.ToString());
            PlayerPrefs.SetFloat("CurrentHp", currentHp);

            // 2. ★ 스탯 Dictionary 저장 (Stat_MiningPower 처럼 키를 만들어서 저장)
            foreach (var kvp in playerStats)
            {
                string key = "Stat_" + kvp.Key.ToString();
                PlayerPrefs.SetFloat(key, kvp.Value);
            }

            PlayerPrefs.Save();
            Debug.Log("게임 저장 완료");
        }

        public void LoadGame()
        {
            // 1. 기본 재화 불러오기
            if (PlayerPrefs.HasKey("Money"))
            {
                long.TryParse(PlayerPrefs.GetString("Money"), out money);
                currentHp = PlayerPrefs.GetFloat("CurrentHp", 100f);
            }

            // 2. ★ 스탯 Dictionary 불러오기
            // Enum에 있는 모든 타입을 돌면서 저장된 값이 있나 확인
            foreach (StatType type in Enum.GetValues(typeof(StatType)))
            {
                string key = "Stat_" + type.ToString();
                if (PlayerPrefs.HasKey(key))
                {
                    float value = PlayerPrefs.GetFloat(key);
                    if (playerStats.ContainsKey(type))
                        playerStats[type] = value;
                    else
                        playerStats.Add(type, value);
                }
            }
            if (topUI != null)
            {
                topUI.UpdateMoneyText(money);
                topUI.UpdateHpUI(currentHp, maxHp);
            }
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }
    }
}