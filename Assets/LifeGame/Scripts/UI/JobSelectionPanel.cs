using UnityEngine;
using LifeGame.Jobs;
using System.Collections.Generic;

namespace LifeGame.UI
{
    public class JobSelectionPanel : MonoBehaviour
    {
        [Header("Template")]
        [SerializeField] private JobSlotUI slotPrefab; // 생성할 버튼 프리팹
        [SerializeField] private Transform contentArea; // 버튼이 들어갈 부모 객체 (Scroll View의 Content 등)

        // 생성된 슬롯들을 추적 관리하기 위한 리스트 (나중에 초기화할 때 유용)
        private List<JobSlotUI> _spawnedSlots = new List<JobSlotUI>();

        public void InitJobMenu(JobSO[] jobs)
        {
            // 기존에 만들어진 슬롯이 있다면 모두 제거 (재진입 시 중복 방지)
            ClearSlots();

            foreach (var job in jobs)
            {
                // 1. 프리팹 생성
                JobSlotUI newSlot = Instantiate(slotPrefab, contentArea);

                // 2. 데이터 주입
                newSlot.Setup(job);

                // 3. 관리 리스트에 추가
                _spawnedSlots.Add(newSlot);
            }
        }

        private void ClearSlots()
        {
            foreach (var slot in _spawnedSlots)
            {
                Destroy(slot.gameObject);
            }
            _spawnedSlots.Clear();
        }
    }
}