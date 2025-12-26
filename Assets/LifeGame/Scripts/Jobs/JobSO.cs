using UnityEngine;

namespace LifeGame.Jobs
{
    // 1. 직업 데이터 (변경되지 않는 정적 데이터)
    [CreateAssetMenu(fileName = "NewJob", menuName = "LifeGame/Job Data")]
    public class JobSO : ScriptableObject
    {
        [Header("Basic Info")]
        public string jobName;        // 직업 이름 (예: 광부)
        [TextArea] public string description;

        [Header("Economy")]
        public int incomePerAction;   // 업무 1회당 수입
        public float workCooldown;    // 업무 쿨타임

        [Header("Requirements")]
        // 이 직업을 얻기 위한 조건 (예: 0이면 조건 없음)
        public int requiredStr;
        public int unlockCost;        // 전직 비용
    }

    // 2. 아이템 데이터
    [CreateAssetMenu(fileName = "NewItem", menuName = "LifeGame/Item Data")]
    public class ItemSO : ScriptableObject
    {
        public string itemName;
        public int price;

        [Header("Bonus Stats")]
        public int bonusIncome;       // 아이템 보유 시 추가 수입
        public int bonusStr;          // 아이템 보유 시 힘 증가
    }
}
