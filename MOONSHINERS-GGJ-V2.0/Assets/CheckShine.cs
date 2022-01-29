using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckShine : MonoBehaviour
{
    public Transform targetTransform;
    public GameObject moonShine;
    public ShineCounter shineCounter;

    public void Check()
    {
        if (Inventory.Instance.shines >= shineCounter.Threshold)
        {
            Instantiate(moonShine, targetTransform.position, Quaternion.identity);
            Inventory.Instance.shines -= shineCounter.Threshold;
            shineCounter.shineSlider.value = 0;
        }
    }
}
