using System.Threading;
using Cysharp.Threading.Tasks;
using Root.Util;
using UnityEngine;

namespace InGame.Cases.TowerDefense.Enemy
{
    /// <summary>
    /// 적이 피격될 때 깜빡이는 효과를 관리하는 컨트롤러입니다.
    /// 알파 값을 조정하여 피격 효과를 연출합니다.
    /// </summary>
    public sealed class EnemyHitEffectController : MonoBehaviourBase
    {
        /// <summary>
        /// Destroy시 UniTask의 null 에러를 방지하기 위한 토큰입니다.
        /// </summary>
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        /// <summary>
        /// 원래의 SpriteRenderer 색상을 저장하는 변수입니다.
        /// 피격 후 원래 색상으로 복구할 때 사용됩니다.
        /// </summary>
        private Color _originalColor;

        /// <summary>
        /// 현재 진행 중인 피격 효과의 횟수를 추적하는 변수입니다.
        /// 피격 효과가 진행 중이면 지속 시간을 연장하는 데 사용됩니다.
        /// </summary>
        private int _hitCount; 

        /// <summary>
        /// 깜빡임 효과를 적용할 대상 SpriteRenderer입니다.
        /// </summary>
        [SerializeField] private SpriteRenderer _spriteRenderer;

        /// <summary>
        /// 깜빡임 효과가 지속되는 시간(ms)입니다.
        /// </summary>
        [SerializeField] private float FLASH_DURATION_MS = 50f;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(EnemyHitEffectController), _spriteRenderer);
            
            _originalColor = _spriteRenderer.color;
            _hitCount = 0;
        }

        /// <summary>
        /// 피격 효과를 즉시 실행하며, 연속 피격 시 지속 시간을 연장합니다.
        /// </summary>
        public void PlayHitFlash()
        {
            ++_hitCount;
            
            if (_hitCount == 1) // 첫 번째 피격 시에만 실행
            {
                PlayHitFlashAsync(_cts.Token).Forget();
            }
        }

        /// <summary>
        /// 알파값을 빠르게 깜빡이도록 조정하는 비동기 메서드입니다.
        /// </summary>
        private async UniTask PlayHitFlashAsync(CancellationToken token)
        {
            Color modifiedColor = _originalColor;
            modifiedColor.a = 0.2f; // 피격 시 알파값을 낮춰서 투명하게 보이게 함
            _spriteRenderer.color = modifiedColor;

            while (_hitCount > 0)
            {
                await UniTask.Delay((int)FLASH_DURATION_MS, cancellationToken: token);
                _hitCount--;
            }

            _spriteRenderer.color = _originalColor; // 원래 색상 복구
        }

        /// <summary>
        /// 객체가 파괴될 때 실행됩니다.
        /// CancellationToken을 정리하여 불필요한 비동기 작업을 방지합니다.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            CancelTokenHelper.ClearToken(in _cts);
        }
    }
}
