using DG.Tweening;
using TMPro;
using UnityEngine;

namespace MenuScripts
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text toolExitPrompt;
        [SerializeField] private float promptFadeTime = 0.4f;

        public static HUDManager Instance { get; private set; }

        private void Awake()
        {
            if (!ReferenceEquals(Instance, null))
            {
                Debug.LogError("There are more than 1 HUDManagers, destroying this one.", this);
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            toolExitPrompt.alpha = 0f;
        }

        public void ShowToolExitPrompt(bool isVisible)
        {
            toolExitPrompt.DOKill();
            toolExitPrompt.DOFade(isVisible ? 1f : 0f, promptFadeTime);
        }
    }
}