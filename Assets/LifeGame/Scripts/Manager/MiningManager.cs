using LifeGame.Manager; // GameManager 사용을 위해 필요
using LifeGame.Mineral;
using LifeGame.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LifeGame.Manager
{
    // 광물 데이터 구조체
    [System.Serializable]
    public struct OreData
    {
        public string oreName;
        public float maxHp;
        public long rewardMoney;
        public float hpCost;     // 캘 때 드는 체력
        public Sprite oreSprite;
    }

    public class MiningManager : MonoBehaviour
    {
        [Header("UI References")]
        public Image hpBarFill;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI statusText;

        [Header("Scene Objects")]
        public List<OreInfo> oreList;

        private int currentOreIndex = 0;
        private float currentOreHp;

        void Start()
        {
            // 시작 시 모든 광물 끄기
            foreach (var ore in oreList) ore.gameObject.SetActive(false);

            SpawnOre();
            StartCoroutine(AutoMineRoutine());
        }

        void Update()
        {
            UpdateUI();
        }

        // --- [새로운 버튼 기능들] ---

        // 1. 더 쉬운 광물 찾기 (Button 1)
        public void ExploreEasier()
        {
            // 이미 제일 쉬운(0번) 광물이면 작동 안 함
            if (currentOreIndex <= 0)
            {
                Debug.Log("이곳보다 더 안전한 곳은 없습니다.");
                return;
            }

            // 이전 광물로 교체
            ChangeOreIndex(currentOreIndex - 1);
            Debug.Log("더 쉬운 광맥을 찾았습니다.");
        }

        // 2. 더 비싼 광물 찾기 (Button 2)
        public void ExploreHarder()
        {
            // 이미 제일 어려운(마지막) 광물이면 작동 안 함
            if (currentOreIndex >= oreList.Count - 1)
            {
                Debug.Log("더 깊이 들어갈 수 없습니다.");
                return;
            }

            // 다음 광물로 교체
            ChangeOreIndex(currentOreIndex + 1);
            Debug.Log("더 깊은 곳으로 이동했습니다. 위험하지만 보상이 큽니다.");
        }

        // 3. 마을로 돌아가기 (Button 3)
        public void GoBackTown()
        {
            // SceneNavigator 싱글톤 사용
            SceneNavigator.Instance.GoToTown();
        }

        // 내부적으로 광물 바꾸는 로직
        private void ChangeOreIndex(int newIndex)
        {
            // 기존 광물 끄기
            if (currentOreIndex < oreList.Count)
                oreList[currentOreIndex].gameObject.SetActive(false);

            currentOreIndex = newIndex;
            SpawnOre();
        }

        // ---------------------------

        IEnumerator AutoMineRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                // 광물이 없거나 범위 밖이면 패스
                if (currentOreIndex < 0 || currentOreIndex >= oreList.Count) continue;

                OreInfo currentOre = oreList[currentOreIndex];

                // ★ 체력 소모 & 기절 체크
                if (!TryConsumeHp(currentOre.hpCost))
                {
                    yield break; // 코루틴 종료 (기절함)
                }

                // 자동 채굴 진행
                float myPower = GameManager.Instance.GetStat(StatType.MiningPower);
                float autoDamage = Mathf.Max(1, myPower * 0.5f);
                HitOre(autoDamage);
            }
        }

        // 클릭 채광
        public void OnClickOre()
        {
            OreInfo currentOre = oreList[currentOreIndex];

            // ★ 체력 소모 & 기절 체크
            if (!TryConsumeHp(currentOre.hpCost)) return;

            float myPower = GameManager.Instance.GetStat(StatType.MiningPower);
            float damage = 1 + myPower;

            HitOre(damage);
        }

        // ★ [핵심] 체력 소모 시도 및 기절 처리 함수
        private bool TryConsumeHp(float cost)
        {
            bool success = GameManager.Instance.ConsumeHp(cost);

            if (!success)
            {
                // 체력 부족 -> 기절 발생!
                
                long currentMoney = GameManager.Instance.money;
                long penalty = currentMoney / 2;

                GameManager.Instance.AddMoney(-penalty);

                GameManager.Instance.SaveGame();

                // 집으로 강제 귀환 (Home 씬)
                // SceneNavigator에 GoHome이 없다면 GoToTown 대신 LoadScene을 써도 됨
                // 여기서는 SceneNavigator가 있다고 가정
                if (SceneNavigator.Instance != null)
                    SceneNavigator.Instance.GoHome();
                else
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Home");

                return false; // 행동 실패
            }
            return true; // 행동 성공
        }

        void SpawnOre()
        {
            OreInfo currentOre = oreList[currentOreIndex];
            currentOre.gameObject.SetActive(false);
            currentOre.gameObject.SetActive(true);

            currentOreHp = currentOre.maxHp;
            if (nameText != null) nameText.text = currentOre.oreName;
        }

        void HitOre(float damage)
        {
            currentOreHp -= damage;
            if (oreList[currentOreIndex] != null) oreList[currentOreIndex].Shake();

            if (currentOreHp <= 0)
            {
                GetReward();
                SpawnOre();
            }
        }

        void GetReward()
        {
            OreInfo currentOre = oreList[currentOreIndex];
            GameManager.Instance.AddMoney(currentOre.rewardMoney);
            GameManager.Instance.AddStat(StatType.MiningPower, 1f);
            GameManager.Instance.SaveGame();
        }

        void UpdateUI()
        {
            if (oreList.Count == 0) return;
            OreInfo currentOre = oreList[currentOreIndex];

            if (hpBarFill != null)
                hpBarFill.fillAmount = currentOreHp / currentOre.maxHp;

            if (statusText != null)
            {
                float hp = GameManager.Instance.currentHp;
                float power = GameManager.Instance.GetStat(StatType.MiningPower);
                statusText.text = $"HP: {hp:F0} / Power: {power:F0}";
            }
        }
    }
}
