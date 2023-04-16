using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Instance;
    public static Managers Instance { get { Init(); return s_Instance; } }

    RespawnManager _game = new RespawnManager();
	public static RespawnManager Game { get { return Instance._game; } }

	DataManager _data = new DataManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerG _scene = new SceneManagerG();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();

    //NetworkManager _network = new NetworkManager();

    public static DataManager Data { get { return Instance._data; } }
	public static ResourceManager Resource { get { return Instance._resource; } }
	public static PoolManager Pool { get { return Instance._pool; } }
    public static SceneManagerG Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }

    //public static NetworkManager Network { get { return Instance._network; } }

    void Start()
    {
        Init();
    }

    static void Init()
    {
        if(s_Instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            
            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<Managers>();
            

            //매니저 start에서 초기화
            s_Instance._data.Init();
            s_Instance._pool.Init();
            s_Instance._sound.Init();
        }
    }

    //씬넘어갈때 메모리 초기화
    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        Pool.Clear();
    }
}