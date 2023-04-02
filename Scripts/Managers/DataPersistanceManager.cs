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

    private void Start()        // ������ ���� �� ��
    {
        // ���������� ��δ� Unity������Ʈ���� �����͸� �����ϱ� ���� �ý��� ǥ�� ���͸�
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        // Linq�� ����ϰ� �־ ������ ���Ӽ� �������̽��� ������ ������Ʈ�� ��� ã�� �� �ִ�.
        // �̷��� ������� ������� ã������ �ش� ��ũ��Ʈ�� Ȯ���ؾ� �Ѵٴ� ���� ���
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

    private void OnApplicationQuit()        // ������ ���� �� ��
    {
        SaveGame();
    }
}
