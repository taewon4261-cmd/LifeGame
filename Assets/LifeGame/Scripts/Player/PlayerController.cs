using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using LifeGame.Jobs;


namespace LifeGame.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Current State")]
        // 직업은 SO를 참조만 합니다. (메모리 절약)
        [SerializeField] private JobSO currentJob;
        [SerializeField] private List<ItemSO> inventory = new List<ItemSO>();

        private int currentMoney;

        // UI 업데이트를 위한 이벤트 (옵저버 패턴)
        // 값이 바뀔 때만 UI를 갱신하여 성능 최적화
        public UnityEvent<int> onMoneyChanged;
        public UnityEvent<JobSO> onJobChanged;

        public void Initialize(JobSO startingJob)
        {
            currentMoney = 0;
            ChangeJob(startingJob);
            inventory.Clear();
            UpdateUI();
        }

        public void Work()
        {
            if (currentJob == null) return;

            // 1. 기본 수입 계산
            int totalIncome = currentJob.incomePerAction;

            // 2. 아이템 보너스 합산 (LINQ 대신 foreach가 가비지 컬렉션(GC) 측면에서 더 좋음)
            foreach (var item in inventory)
            {
                totalIncome += item.bonusIncome;
            }

            // 3. 수입 적용
            currentMoney += totalIncome;

            // 4. UI 갱신 알림
            onMoneyChanged?.Invoke(currentMoney);
            Debug.Log($"업무 완료! {totalIncome}원 획득. 현재 잔액: {currentMoney}");
        }

        public void ChangeJob(JobSO newJob)
        {
            currentJob = newJob;
            onJobChanged?.Invoke(currentJob);
        }

        public void BuyItem(ItemSO item)
        {
            if (currentMoney >= item.price)
            {
                currentMoney -= item.price;
                inventory.Add(item);
                onMoneyChanged?.Invoke(currentMoney);
                Debug.Log($"{item.itemName} 구매 완료!");
            }
            else
            {
                Debug.Log("돈이 부족합니다.");
            }
        }

        // 초기화를 위해 강제 호출
        private void UpdateUI()
        {
            onMoneyChanged?.Invoke(currentMoney);
            onJobChanged?.Invoke(currentJob);
        }
    }
}
