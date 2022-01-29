    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    [RequireComponent(typeof(Collider))]
    public class InteractOnTrigger : MonoBehaviour
    {
        public UnityEvent OnEnter, OnExit;


        void OnTriggerEnter(Collider other)
        {
            //if (0 != (layers.value & 1 << other.gameObject.layer))
            //{
            //    ExecuteOnEnter(other);
            //}

            if (other.CompareTag("Player"))
            {
                ExecuteOnEnter(other);
            }
        }

        protected virtual void ExecuteOnEnter(Collider other)
        {
            OnEnter.Invoke();
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ExecuteOnExit(other);
            }
        }


        protected virtual void ExecuteOnExit(Collider other)
        {
            OnExit.Invoke();
        }

    }
