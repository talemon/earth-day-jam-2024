using System;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MenuScripts
{
    public enum MainMenuState
    {
        Title,
        Main,
        Play,
        Credits
    }
    
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameStateManager gameStateManager;

        [SerializeField] private RectTransform[] mainTransforms;
        [SerializeField] private RectTransform[] playMenuTransforms;

        [SerializeField] private TitleAnimation titleAnimation;
        [SerializeField] private TMP_Text pressAnyKey;

        [SerializeField] private TMP_InputField shipNameInput;
        [SerializeField] private Button startButton;

        [SerializeField] private Image titleImage;
        [SerializeField] private CanvasGroup creditsGroup;
        
        [SerializeField] private float animationTime = 1f;
        [SerializeField] private float animationInterval = 1f;

        [SerializeField] private SceneAsset playScene;

        private MainMenuState _state = MainMenuState.Title;
        private Sequence _currentAnimSequence;

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

            _currentAnimSequence = DOTween.Sequence();
            _currentAnimSequence.Append(pressAnyKey.DOFade(1f, animationTime).SetEase(Ease.OutSine).SetDelay(1f));

            return;

            void MoveOut(RectTransform t)
            {
                var pos = t.anchoredPosition;
                pos.x = screenWidth;
                t.anchoredPosition = pos;
            }
        }

        private void SetState(MainMenuState newState)
        {
            if (_state == newState)
                return;
            
            if(_currentAnimSequence != null && _currentAnimSequence.IsActive())
                _currentAnimSequence.Kill(true);

            _currentAnimSequence = DOTween.Sequence();
            switch (_state)
            {
                case MainMenuState.Title:
                    _currentAnimSequence.Append(titleAnimation.MoveDown(animationTime));
                    _currentAnimSequence.Insert(0f, pressAnyKey.DOFade(0f, animationTime * 0.5f).SetEase(Ease.OutSine));
                    break;
                case MainMenuState.Main:
                    _currentAnimSequence.Append(AnimateOut(mainTransforms));
                    break;
                case MainMenuState.Play:
                    _currentAnimSequence.Append(AnimateOut(playMenuTransforms));
                    break;
                case MainMenuState.Credits:
                    _currentAnimSequence.Append(creditsGroup.DOFade(0f, animationTime).SetEase(Ease.InOutSine));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            var outAnimEnd = _currentAnimSequence.Duration();
            
            switch (newState)
            {
                case MainMenuState.Title:
                    _currentAnimSequence.Insert(outAnimEnd, titleAnimation.MoveUp(animationTime));
                    _currentAnimSequence.Insert(outAnimEnd, pressAnyKey.DOFade(1f, animationTime).SetEase(Ease.OutSine).SetDelay(1f));
                    break;
                case MainMenuState.Main:
                    _currentAnimSequence.Insert(outAnimEnd, titleImage.DOFade(1f, animationTime).SetEase(Ease.InOutSine));
                    _currentAnimSequence.Insert(outAnimEnd, AnimateIn(mainTransforms));
                    break;
                case MainMenuState.Play:
                    _currentAnimSequence.Append(AnimateIn(playMenuTransforms));
                    break;
                case MainMenuState.Credits:
                    _currentAnimSequence.Insert(outAnimEnd, titleImage.DOFade(0f, animationTime).SetEase(Ease.InOutSine));
                    _currentAnimSequence.Insert(outAnimEnd, creditsGroup.DOFade(1f, animationTime).SetEase(Ease.InOutSine));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
            _state = newState;
        }

        private void Update()
        {
            if (_state == MainMenuState.Title && Input.anyKeyDown)
            {
                SetState(MainMenuState.Main);
            }
            else if(Input.GetButtonDown("Cancel"))
            {
                switch (_state)
                {
                    case MainMenuState.Main:
                        SetState(MainMenuState.Title);
                        break;
                    case MainMenuState.Play:
                        SetState(MainMenuState.Main);
                        break;
                    case MainMenuState.Credits:
                        SetState(MainMenuState.Main);
                        break;
                    case MainMenuState.Title:
                    default:
                        return;
                }
            }
        }

        public void OnPlayClicked()
        {
            SetState(MainMenuState.Play);
        }

        public void OnStartClicked()
        {
            if(_currentAnimSequence != null && _currentAnimSequence.IsActive())
                _currentAnimSequence.Kill(true);
            
            gameStateManager.Initialize();
            gameStateManager.GetGameState().ShipName = shipNameInput.text;
            SceneManager.LoadScene(playScene.name);
        }

        public void OnCreditsClicked()
        {
            SetState(MainMenuState.Credits);
        }

        public void OnExitClicked()
        {
            Application.Quit();
        }

        public void OnPlayBackClicked()
        {
            SetState(MainMenuState.Main);
            // Sequence seq = DOTween.Sequence();
            // seq.Append(AnimateOut(playMenuTransforms));
            // seq.Append(AnimateIn(mainTransforms));
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