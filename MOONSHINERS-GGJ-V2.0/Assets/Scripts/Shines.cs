using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shines : MonoBehaviour
{
    //public ShineCounter _shineCounter;
    public UnityEvent OnTake;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //_shineCounter.SumShine();
            OnTake.Invoke();
            Destroy(this.gameObject);
        }
    }
}
