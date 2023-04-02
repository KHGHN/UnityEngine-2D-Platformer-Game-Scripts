using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private bool useEncryption = false;
    private readonly string EncryptionCodeWord = "word";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }


                // deserialize the data from JSON back into the C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file : " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        // " / " 를 써서 경로를 연결할 수 있지만 운영체제마다 파일구분 기호가 다르므로 이를 수행하는 더 좋은 방법은 
        // Combine()을 쓰는것이다
        //string fullPath = dataDirPath + "/" + dataFileName;
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            // 컴퓨터에 아직 존재하지 않는 경우를 대비하여 디렉터리 경로를 만들어야 함
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // 게임 데이터 개체를 JSON문자열로 직렬화하기위해 데이터라는 새 문자열을 만들어 저장한 다음
            // JSON 유틸리티와 동일하게 JSON으로 설정한 다음 데이터를 전달함.
            string dataToStore = JsonUtility.ToJson(data, true);

            // optionally encrypt the data
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // 파일 읽기 또는 쓰기를 처리할 때, 블록을 사용하는 것이 가장 좋다.
            // 왜냐면 파일 읽기 또는 쓰기가 완료되면 해당 파일에 대한 연결이 닫히도록 하기 때문
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }


        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file : " + fullPath + "\n" + e);
        }
    }

    // the below is a simple implementation of XOR encryption
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ EncryptionCodeWord[i % EncryptionCodeWord.Length]);
        }

        return modifiedData;
    }
}
