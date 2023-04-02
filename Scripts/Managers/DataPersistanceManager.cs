using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class DataPersistanceManager : MonoBehaviour
{

    [Header("File Stroage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;

    private List<IDataPersistance> dataPersistanceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistanceManager Instance { get; private set; }


    //public void Init()
    //{ 
    //}

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Data Persistance Manager in the scene.");
        }
        Instance = this;
    }

    private void Start()        // 게임이 시작 될 때
    {
        // 영구데이터 경로는 Unity프로젝트에서 데이터를 유지하기 위한 시스템 표준 디렉터리
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        // Linq를 사용하고 있어서 데이터 지속성 인터페이스를 구현한 오브젝트를 모두 찾을 수 있다.
        // 이러한 방식으로 모노비헤비어를 찾으려면 해당 스크립트도 확장해야 한다는 점을 명심
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // TODO - Load any saved data from a file using the data handler
        this.gameData = dataHandler.Load();

        // if no data can be loaded, initialize to a new game
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        // TODO - push the loaded data to all other scripts that need it
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.LoadData(gameData);
        }

        //Debug.Log("Loaded death count = " + gameData.deathCount);
    }
    public void SaveGame()
    {
        // TODO - pass the data to other scripts
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.SaveData(ref gameData);
        }

        //Debug.Log("Saved death count = " + gameData.deathCount);

        // TODO - save that data to a file using the data handler
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()        // 게임이 종료 될 때
    {
        SaveGame();
    }
}
