using UnityEngine;
using System.Collections;

public class AnimationPath : MonoBehaviour
{
    public AnimationCurve XCurve;

    public float TotalTravelTime = 5.0f;

    public float TravelSpeed = 50.0f;

    public float XRange = 10.0f;

    void Start()
    {
        StartCoroutine("Travel");
    }

    IEnumerator Travel()
    {
        float ElapsedTime = 0.0f;

        while (ElapsedTime < TotalTravelTime)
        {
            float XPos = XCurve.Evaluate(ElapsedTime / TotalTravelTime) * XRange;

            transform.localPosition= new Vector3(XPos, -10 +  XPos, transform.localPosition.z + TravelSpeed * - Time.deltaTime);

            yield return null;

            ElapsedTime += Time.deltaTime;
        }
    }
}