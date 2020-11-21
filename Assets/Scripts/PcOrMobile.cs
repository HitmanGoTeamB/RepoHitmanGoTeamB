using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Hitman GO/UI/Pc or Mobile")]
public class PcOrMobile : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] GameObject pc = default;
    [SerializeField] GameObject mobile = default;

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        pc.SetActive(false);
        mobile.SetActive(true);
#else
        pc.SetActive(true);
        mobile.SetActive(false);
#endif
    }
}
