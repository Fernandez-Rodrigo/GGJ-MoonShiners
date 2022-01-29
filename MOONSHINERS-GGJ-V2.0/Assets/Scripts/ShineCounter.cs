using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShineCounter : MonoBehaviour
{
    public Slider shineSlider;
    public int Threshold = 10;

    public void SumShine()
    {
        Inventory.Instance.shines++;
        shineSlider.value += 0.1f;
    }

    
   

}
