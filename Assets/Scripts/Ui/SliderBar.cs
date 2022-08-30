using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SliderBar : MonoBehaviour {

    [SerializeField]
    private float FillRate;
    [SerializeField]
    private Color color;
    [SerializeField]
    private Image slider;
    Coroutine c;
    public void Setfillrate(float f,bool instant=false)
    {
        f = Mathf.Clamp(f, 0, 1);
        if (!instant)
        {
            if (c == null)
                c = StartCoroutine(LerpProces(f));
            else
            {
                StopCoroutine(c);
                c = StartCoroutine(LerpProces(f));
            }
        }
        else
            slider.fillAmount = f;
    }
    
    IEnumerator LerpProces(float f)
    {
        while (slider.fillAmount != f)
        {
            slider.fillAmount = Mathf.Lerp(slider.fillAmount, f, Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnValidate()
    {
        FillRate = Mathf.Clamp(FillRate, 0, 1);
        slider.fillAmount = FillRate;
        slider.color = color;
    }
}
