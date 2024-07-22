using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    [Tooltip("������������� ���������������� ����� ��� ��������")]
    [SerializeField]
    private bool autoInitializeOnStart = false;

    [Tooltip("�� ���������� ������ ��� ������������ �� ������ �����")]
    [SerializeField]
    private bool dontDestroyOnLoad = false;

    /// <summary>
    /// �����, ����������� ��� ������������� ������
    /// </summary>
    public virtual void Initialize()
    {
    }

    protected virtual void Start()
    {
        if (autoInitializeOnStart)
        {
            Initialize();
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            if (this is T)
            {
                _instance = this as T;
                if (dontDestroyOnLoad && Application.isPlaying)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
        }
        else if (Application.isPlaying)
        {
            Debug.LogWarning($"[Singleton] Instance {typeof(T)} already exists. Destroying {name}...");
            DestroyImmediate(gameObject);
        }

    }

    private static T _instance;
    /// <summary>
    /// �������� ������ �� ��������� ������
    /// </summary>
    public static T Instance => _instance;

    /// <summary>
    /// ��������� ������������� ������
    /// </summary>
    public static bool Exists => _instance != null;
}

