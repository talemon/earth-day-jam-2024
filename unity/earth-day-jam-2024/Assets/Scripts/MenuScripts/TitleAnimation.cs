using DG.Tweening;
using UnityEngine;

namespace MenuScripts
{
    public class TitleAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform buoy;
        [SerializeField] private RectTransform wave;

        [SerializeField] private RectTransform group;

        [SerializeField] private float horizontalDuration = 6f;
        [SerializeField] private float verticalDuration = 3f;
        [SerializeField] private float buoyDelay = 0.4f;

        [SerializeField] private float groupMove = -100f;
        [SerializeField] private float moveUpTime = 1f;

        private Vector2 _groupStartPosition;
    
        private void Start()
        {
            _groupStartPosition = group.anchoredPosition;
        
            wave.DOAnchorPosY(0f, verticalDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            wave.DOAnchorPosX(0f, horizontalDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        
            buoy.DOAnchorPosY(0f, verticalDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(buoyDelay);
            buoy.DOAnchorPosX(0f, horizontalDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(buoyDelay);
        }

        public void MoveDown(float duration)
        {
            group.DOKill();
            group.DOAnchorPosY(_groupStartPosition.y + groupMove, duration).SetEase(Ease.OutSine);
        }

        public void MoveUp(float delay)
        {
            group.DOKill();
            group.DOAnchorPosY(_groupStartPosition.y, moveUpTime).SetEase(Ease.OutSine).SetDelay(delay);
        }
    }
}
