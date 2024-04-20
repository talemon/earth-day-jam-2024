using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPrompt : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float animTime = 0.4f;

    private void SetImageAlpha(float alpha)
    {
        var col = image.color;
        col.a = alpha;
        image.color = col;
    }
    
    public void Show(bool immediate = false)
    {
        if (immediate)
        {
            SetImageAlpha(1f);
            gameObject.SetActive(true);
            return;
        }

        SetImageAlpha(0f);
        gameObject.SetActive(true);
        image.DOFade(1f, animTime);
    }
    
    public void Hide(bool immediate = false)
    {
        if (immediate)
        {
            gameObject.SetActive(false);
            return;
        }
        
        SetImageAlpha(1f);
        image.DOFade(0f, animTime).OnComplete(() => gameObject.SetActive(false));
    }
}