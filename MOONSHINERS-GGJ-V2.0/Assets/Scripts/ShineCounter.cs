using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShineCounter : MonoBehaviour
{
    public Slider shineSlider;

    public void SumShine()
    {
        shineSlider.value = shineSlider.value + 0.1f;
    }


}
