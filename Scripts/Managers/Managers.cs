using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;

    public static Managers Instance { get { return s_instance; } }

    ResourceManager _resource = new ResourceManager();
    DataManager _data = new DataManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();

    public static ResourceManager Resource { get { return Instance._resource; } }
    public static DataManager Data { get { return Instance._data; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }


    private void Awake()
    {
        Init();    
    }

    private void Init()
    {
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go == null)
            {
                go = new GameObject() { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._data.Init();
            s_instance._sound.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
    }

}
