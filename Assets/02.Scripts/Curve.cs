using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Curve : MonoBehaviour
{
    public AnimationCurve animationCurve;
    float timePassed = 0;
    float lifeTime = 3f;
    float size_x = 10;
    float size_y = 2.5f;
    int posCount =30;
    Keyframe keyframe_first, keyframe_middle, keyframe_last;

    public Transform ball;
    bool isShooting;
    Vector3 newPosition;
    float chargeTime;
    float maxChargeTime = 3f;

    LineRenderer lr;

    public Text text_chargTime;
    float t;
    private void Start()
    {
        Application.targetFrameRate = 60;
        lr = GetComponent<LineRenderer>();
        keyframe_first.time = 0;
        keyframe_first.value = animationCurve.Evaluate(0);
        keyframe_middle.time = 0.5f;
        keyframe_middle.value = animationCurve.Evaluate(0.5f);
        keyframe_last.time = 1f;
        keyframe_last.value = animationCurve.Evaluate(1f);
        //animationCurve.MoveKey(0, keyframe_first);
        //animationCurve.MoveKey(1, keyframe_middle);
        //animationCurve.MoveKey(2, keyframe_last);
        //animationCurve.SmoothTangents(1, 0);

        lr.positionCount = posCount;
        for (int i = 1; i < lr.positionCount+1; i++)
        {
            lr.SetPosition(i - 1, new Vector3(i*size_x/posCount, animationCurve.Evaluate(i / (float)posCount) * size_y, 0));
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            t = Time.time;
        }
        if (Input.GetKey(KeyCode.Space) && !isShooting)
        {
            if (chargeTime < maxChargeTime)
            {
                //chargeTime += Time.deltaTime;
                chargeTime += Time.deltaTime;
                //text_chargTime.text = chargeTime.ToString();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (!isShooting)
            {
                StartCoroutine(Co_MoveBall());
            }
        }
        if (Input.GetKey(KeyCode.Q))
        {
            ValueUp(ref keyframe_first, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            ValueDown(ref keyframe_first, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            ValueUp(ref keyframe_middle, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            ValueDown(ref keyframe_middle, 1);
        }
        if (Input.GetKey(KeyCode.E))
        {
            ValueUp(ref keyframe_last, 2);
        }
        if (Input.GetKey(KeyCode.D))
        {
            ValueDown(ref keyframe_last, 2);
        }
    }
    void ValueUp(ref Keyframe keyframe, int position)
    {
        if (keyframe.value < 1)
        {
            keyframe.value += Time.deltaTime;
            animationCurve.MoveKey(position, keyframe);
            animationCurve.SmoothTangents(1, 0.3f);
            DrawLine();
        }
    }
    void ValueDown(ref Keyframe keyframe, int position)
    {
        if (keyframe.value > 0)
        {
            keyframe.value -= Time.deltaTime;
            animationCurve.MoveKey(position, keyframe);
            animationCurve.SmoothTangents(1, 0.3f);
            DrawLine();
        }
    }
    void DrawLine()
    {
        for (int i = 1; i < lr.positionCount + 1; i++)
        {
            lr.SetPosition(i - 1, new Vector3(i * size_x / posCount, animationCurve.Evaluate(i / (float)posCount) * size_y, 0));
        }
    }
    IEnumerator Co_MoveBall()
    {
        isShooting = true;
        ball.gameObject.SetActive(true);
        while (timePassed <= chargeTime / maxChargeTime)
        {
            timePassed += Time.deltaTime / lifeTime;
            newPosition.y = (animationCurve.Evaluate(timePassed) - animationCurve.Evaluate(0)) * size_y;
            newPosition.x = timePassed * size_x;
            ball.localPosition = newPosition;
            yield return null;
        }
        timePassed = 0f;
        chargeTime = 0f;
        isShooting = false;
        ball.gameObject.SetActive(false);
    }
}
