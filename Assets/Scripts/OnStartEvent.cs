using UltEvents;
using UnityEngine;

public class OnStartEvent : MonoBehaviour
{
    public UltEvent OnStart;

    void Start()
    {
        OnStart.Invoke();
    }
}
