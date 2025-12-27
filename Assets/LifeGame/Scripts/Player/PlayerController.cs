using LifeGame.Jobs;
using LifeGame.Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace LifeGame.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Current State")]
        // 직업은 SO를 참조만 합니다. (메모리 절약)
        [SerializeField] private JobSO currentJob;
        [SerializeField] private List<ItemSO> inventory = new List<ItemSO>();


        [Header("Events")]
        // 돈이 바뀌면 UI 업데이트 (매니저 이벤트를 쓰거나 여기서 연결)
        public UnityEvent<long> onMoneyChanged;
        public UnityEvent<JobSO> onJobChanged;

        public void Initialize(JobSO startingJob)
        {
            
            ChangeJob(startingJob);
            inventory.Clear();
            UpdateUI();
        }

        // "일하기" 버튼을 눌렀을 때
        public void Work()
        {
            // 1. [수정] JobSO의 변수 대신, 매니저에 저장된 내 스탯(능력치)을 가져옴
            // 예: "채광력"이 높으면 돈을 더 많이 범
            float miningPower = GameManager.Instance.GetStat(StatType.MiningPower);

            // 기본 수입 10 + 내 능력치만큼 범
            long income = 10 + (long)miningPower;

            // 2. [수정] 아이템 보너스 (아이템SO 구조도 나중에 스탯 기반으로 바꾸는 게 좋음)
            // 임시로 기존 로직 유지 (ItemSO에 bonusIncome이 있다는 가정 하에)
            /* foreach (var item in inventory)
            {
                income += item.bonusIncome;
            }
            */

            // 3. [핵심] 돈 더하기는 GameManager에게 시킨다
            GameManager.Instance.AddMoney(income);

            // 4. UI 알림
            onMoneyChanged?.Invoke(GameManager.Instance.money);
            Debug.Log($"업무 완료! {income}원 획득.");
        }
        public void ChangeJob(JobSO newJob)
        {
            currentJob = newJob;

            // 매니저에게 직업 변경 알림 (스탯 적용 등은 매니저가 알아서 함)
            GameManager.Instance.OnJobSelected(newJob);

            onJobChanged?.Invoke(currentJob);
        }

        public void BuyItem(ItemSO item)
        {
            // 1. [수정] 돈이 충분한지 매니저의 돈을 확인
            if (GameManager.Instance.money >= item.price)
            {
                // 2. 매니저에게 돈 차감 요청 (AddMoney에 음수를 넣음)
                GameManager.Instance.AddMoney(-item.price);

                inventory.Add(item);

                // UI 갱신
                onMoneyChanged?.Invoke(GameManager.Instance.money);
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
            // 현재 매니저의 돈을 가져와서 UI 갱신
            onMoneyChanged?.Invoke(GameManager.Instance.money);
            onJobChanged?.Invoke(currentJob);
        }
    }
}
