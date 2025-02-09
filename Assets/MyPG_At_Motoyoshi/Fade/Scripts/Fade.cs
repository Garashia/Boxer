﻿/*
 The MIT License (MIT)

Copyright (c) 2013 yamamura tatsuhiko

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Collections;
using UnityEngine;

public class Fade : MonoBehaviour
{
    private IFade fade;

    private void Start()
    {
        Init();
        fade.Range = cutoutRange;
    }

    private float cutoutRange;

    private void Init()
    {
        fade = GetComponent<IFade>();
    }

    private void OnValidate()
    {
        Init();
        fade.Range = cutoutRange;
    }

    private IEnumerator FadeoutCoroutine(float time, System.Action action)
    {
        cutoutRange = 1.0f;
        fade.Range = cutoutRange;

        float endTime = Time.unscaledTime + time * (cutoutRange);
        var endFrame = new WaitForSecondsRealtime(0.0f);

        while (Time.unscaledTime <= endTime)
        {
            cutoutRange = (endTime - Time.unscaledTime) / time;
            fade.Range = cutoutRange;

            yield return endFrame;
        }
        cutoutRange = 0;
        fade.Range = cutoutRange;

        if (action != null)
        {
            action();
        }
    }

    private IEnumerator FadeinCoroutine(float time, System.Action action)
    {
        cutoutRange = 0;
        fade.Range = cutoutRange;

        float endTime = Time.unscaledTime + time * (1 - cutoutRange);

        var endFrame = new WaitForSecondsRealtime(0.0f);

        while (Time.unscaledTime <= endTime)
        {
            cutoutRange = 1 - ((endTime - Time.unscaledTime) / time);
            fade.Range = cutoutRange;
            yield return endFrame;
        }
        cutoutRange = 1;
        fade.Range = cutoutRange;

        if (action != null)
        {
            action();
        }
    }

    public Coroutine FadeOut(float time, System.Action action)
    {
        StopAllCoroutines();
        return StartCoroutine(FadeoutCoroutine(time, action));
    }

    public Coroutine FadeOut(float time)
    {
        return FadeOut(time, null);
    }

    public Coroutine FadeIn(float time, System.Action action)
    {
        StopAllCoroutines();
        return StartCoroutine(FadeinCoroutine(time, action));
    }

    public Coroutine FadeIn(float time)
    {
        return FadeIn(time, null);
    }
}