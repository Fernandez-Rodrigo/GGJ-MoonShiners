using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shines : MonoBehaviour
{
    public ShineCounter _shineCounter;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _shineCounter.SumShine();
            Destroy(this.gameObject);
        }
    }
}
