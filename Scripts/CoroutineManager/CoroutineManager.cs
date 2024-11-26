using System.Collections;
using UnityEngine;

namespace GameSystem
{
    public sealed class CoroutineManager : MonoBehaviour
    {
        public static CoroutineManager Instance { get; private set; }


        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public Coroutine StartManagedCoroutine(IEnumerator coroutine)
            => StartCoroutine(coroutine);

        public void StopManagedCoroutine(Coroutine coroutine)
            => StopCoroutine(coroutine);

        public IEnumerator WaitAndExecute(float waitTime, System.Action callback)
        {
            yield return new WaitForSeconds(waitTime);
            callback?.Invoke();
        }
    }
}
