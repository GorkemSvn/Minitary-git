using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SRCountdownButton : SelfReportingButton
{
    public ButtonEvent OnTimeOut;
    Coroutine countDownCo;
    public void Set(string name,float currentTime,float maxTime,bool countDown)
    {
        GetComponentInChildren<Text>().text = name;

        if(countDownCo!=null)
        {
            StopCoroutine(countDownCo);
            countDownCo = null;
        }
        if(countDown)
            countDownCo = StartCoroutine(Countdown(currentTime, maxTime));
    }

    IEnumerator Countdown(float currentTime,float maxTime)
    {
        var slider = GetComponentInChildren<SliderBar>();

        for (float t = currentTime; t < maxTime; t+=Time.deltaTime)
        {
            slider.Setfillrate(t / maxTime,true);
            yield return null;
        }
        OnTimeOut(transform.GetSiblingIndex());
        countDownCo = null;
    }
}
