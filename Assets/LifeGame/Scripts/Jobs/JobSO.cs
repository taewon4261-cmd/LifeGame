using LifeGame.Manager;
using System.Collections.Generic;
using UnityEngine;


namespace LifeGame.Jobs
{
    // 1. 직업 데이터 (변경되지 않는 정적 데이터)
    [CreateAssetMenu(fileName = "NewJob", menuName = "LifeGame/Job Data")]
    public class JobSO : ScriptableObject
    {
        public string jobName;
        public string description;
        public Sprite icon;

        // ★ 핵심: 이 직업이 가질 초기 스탯 목록
        [System.Serializable]
        public struct StatBonus
        {
            public StatType statType;
            public int value;
        }

        public List<StatBonus> baseStats; // 예: 광부는 MiningPower: 10, Stamina: 50
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
