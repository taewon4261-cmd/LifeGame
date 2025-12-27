using System.Collections;
using UnityEngine;



namespace LifeGame.Mineral
{
    // 이 스크립트는 광물 오브젝트 하나하나에 붙습니다.
    public class OreInfo : MonoBehaviour
    {
        [Header("광물 데이터")]
        public string oreName;      // 광물 이름
        public float maxHp;         // 최대 체력
        public long rewardMoney;    // 보상 금액
        public float hpCost;        // 채광 시 소모 체력

        // [선택] 각 광물마다 캘 때 나는 소리가 다르면 여기에 AudioClip 추가 가능
        // public AudioClip miningSound; 

        [Header("연출 설정")]
        [SerializeField] private float shakeDuration = 0.1f;  // 얼마나 오래 떨릴지 (0.1초 추천)
        [SerializeField] private float shakeStrength = 10f;   // 얼마나 세게 떨릴지 (UI 크기에 따라 조절 필요)

        private Vector3 initialPosition; // 원래 위치 기억용
        private Coroutine currentShakeRoutine;

        private void OnEnable()
        {
            // 켜질 때 자기 원래 위치를 기억해둡니다.
            initialPosition = transform.localPosition;
        }

        // 외부(매니저)에서 이 함수를 부르면 떨기 시작합니다.
        public void Shake()
        {
            // 이미 떨고 있다면 멈추고 새로 시작 (연타했을 때 자연스럽게)
            if (currentShakeRoutine != null) StopCoroutine(currentShakeRoutine);

            currentShakeRoutine = StartCoroutine(ShakeRoutine());
        }

        IEnumerator ShakeRoutine()
        {
            float elapsed = 0f;

            while (elapsed < shakeDuration)
            {
                // 원래 위치에서 랜덤하게 조금씩 비껴나간 위치를 계산
                // Random.insideUnitSphere는 반지름 1짜리 구 안의 랜덤 좌표를 줍니다.
                Vector3 randomPoint = initialPosition + (Random.insideUnitSphere * shakeStrength);

                // Z축(깊이)은 흔들리면 안 되므로 원래 값 유지 (2D UI인 경우)
                randomPoint.z = initialPosition.z;

                transform.localPosition = randomPoint;

                elapsed += Time.deltaTime;
                yield return null; // 1프레임 대기
            }

            // 시간이 다 되면 깔끔하게 원래 위치로 복귀
            transform.localPosition = initialPosition;
        }
    }
}