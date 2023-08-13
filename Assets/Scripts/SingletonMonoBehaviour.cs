using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (_instance == null)
                {
                    GameObject singletonObject = new();
                    _instance = singletonObject.AddComponent<T>();

                    singletonObject.name = typeof(T).ToString();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        
    }

    public virtual void InitializeSingletonOnAwake() { }
}
