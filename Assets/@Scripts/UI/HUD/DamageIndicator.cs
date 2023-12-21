using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    #region Fields

    private Image _image;
    private float _flashSpeed;
    private Coroutine _coroutine;

    #endregion

    #region Properties

    public float FlashSpeed
    {
        get => _flashSpeed;
        set => _flashSpeed = value;
    }

    public Coroutine Coroutine
    {
        get => _coroutine;
        set => _coroutine = value;
    }

    public Image Image
    {
        get => _image;
        set => _image = value;
    }
    
    #endregion

    public void Start()
    {
        Image = GetComponent<Image>();
        FlashSpeed = 0.5f;
        PlayerCondition.OnDamageable += Flash;
    }

    private void Flash()
    {
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
        }

        Image.enabled = true;
        Image.color = Color.red;
        Coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0.0f)
        {
            a -= (startAlpha / FlashSpeed) * Time.deltaTime;
            Image.color = new Color(1.0f, 0.0f, 0.0f, a);
            yield return null;
        }
        Image.enabled = false;
    }
}
