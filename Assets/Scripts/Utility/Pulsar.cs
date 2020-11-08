using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Hitman GO/Utility/Pulsar")]
public class Pulsar : MonoBehaviour
{
    [SerializeField] AnimationCurve curve = default;

    void Update()
    {
        float size = curve.Evaluate(Time.time);
        transform.localScale = new Vector3(size, size, size);
    }
}
