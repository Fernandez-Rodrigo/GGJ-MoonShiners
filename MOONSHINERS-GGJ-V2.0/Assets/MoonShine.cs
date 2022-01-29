using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoonShine : MonoBehaviour
{
    public UnityEvent OnTake;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnTake.Invoke();
            Inventory.Instance.hasMoonShine = true;
            Destroy(this.gameObject);
        }
    }
}
