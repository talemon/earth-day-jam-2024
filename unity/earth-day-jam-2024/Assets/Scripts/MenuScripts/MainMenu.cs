using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MenuScripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameStateManager gameStateManager;

        [SerializeField] private RectTransform[] mainTransforms;
        [SerializeField] private RectTransform[] playMenuTransforms;

        [SerializeField] private TitleAnimation titleAnimation;
        [SerializeField] private TMP_Text pressAnyKey;

        [SerializeField] private TMP_InputField shipNameInput;
        [SerializeField] private Button startButton;
        
        [SerializeField] private float animationTime = 1f;
        [SerializeField] private float animationInterval = 1f;

        [SerializeField] private SceneAsset playScene;

        private bool _isMenuShowing = false;
    
        private void Start()
        {
            DOTween.Init(false, true, LogBehaviour.Default);

            var screenWidth = Screen.width;

            foreach (var childTransform in mainTransforms)
            {
                MoveOut(childTransform);
            }
            foreach (var childTransform in playMenuTransforms)
            {
                MoveOut(childTransform);
            }

            pressAnyKey.DOFade(1f, animationTime).SetEase(Ease.OutSine).SetDelay(1f);

            return;

            void MoveOut(RectTransform t)
            {
                var pos = t.anchoredPosition;
                pos.x = screenWidth;
                t.anchoredPosition = pos;
            }
        }

        private void Update()
        {
            if (!_isMenuShowing && Input.anyKeyDown)
            {
                AnimateIn(mainTransforms);
                titleAnimation.MoveDown(animationTime);
                
                pressAnyKey.DOKill();
                pressAnyKey.DOFade(0f, animationTime * 0.5f).SetEase(Ease.OutSine);
                
                _isMenuShowing = true;
            }
            else if (_isMenuShowing && Input.GetButtonDown("Cancel"))
            {
                AnimateOut(mainTransforms);
                titleAnimation.MoveUp(animationTime);
                
                pressAnyKey.DOKill();
                pressAnyKey.DOFade(1f, animationTime).SetEase(Ease.OutSine).SetDelay(1f);
                
                _isMenuShowing = false;
            }
        }

        public void OnPlayClicked()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(AnimateOut(mainTransforms));
            seq.Append(AnimateIn(playMenuTransforms));
        }

        public void OnStartClicked()
        {
            gameStateManager.Initialize();
            gameStateManager.GetGameState().ShipName = shipNameInput.text;
            SceneManager.LoadScene(playScene.name);
        }

        public void OnCreditsClicked()
        {
        
        }

        public void OnExitClicked()
        {
            Application.Quit();
        }

        public void OnPlayBackClicked()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(AnimateOut(playMenuTransforms));
            seq.Append(AnimateIn(mainTransforms));
        }

        public void OnShipNameChanged(string newInput)
        {
            startButton.interactable = !string.IsNullOrEmpty(newInput);
        }
        
        private Sequence AnimateOut(RectTransform[] transforms)
        {
            Sequence seq = DOTween.Sequence();
            
            var width = Screen.width;
            var interval = animationInterval;
            foreach (var child in transforms)
            {
                var tween = child.DOAnchorPosX(width, animationTime).SetEase(Ease.InBack);
                seq.Insert(interval, tween);
                interval += animationInterval;
            }
            
            return seq;
        }

        private Sequence AnimateIn(RectTransform[] transforms)
        {
            Sequence seq = DOTween.Sequence();
            
            var interval = animationInterval;
            foreach (var child in transforms)
            {
                var tween = child.DOAnchorPosX(0f, animationTime).SetEase(Ease.OutBack);
                seq.Insert(interval, tween);
                interval += animationInterval;
            }

            return seq;
        }
    }
}