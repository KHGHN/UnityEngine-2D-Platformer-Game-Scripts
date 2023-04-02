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
        // " / " �� �Ἥ ��θ� ������ �� ������ �ü������ ���ϱ��� ��ȣ�� �ٸ��Ƿ� �̸� �����ϴ� �� ���� ����� 
        // Combine()�� ���°��̴�
        //string fullPath = dataDirPath + "/" + dataFileName;
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            // ��ǻ�Ϳ� ���� �������� �ʴ� ��츦 ����Ͽ� ���͸� ��θ� ������ ��
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // ���� ������ ��ü�� JSON���ڿ��� ����ȭ�ϱ����� �����Ͷ�� �� ���ڿ��� ����� ������ ����
            // JSON ��ƿ��Ƽ�� �����ϰ� JSON���� ������ ���� �����͸� ������.
            string dataToStore = JsonUtility.ToJson(data, true);

            // optionally encrypt the data
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // ���� �б� �Ǵ� ���⸦ ó���� ��, ����� ����ϴ� ���� ���� ����.
            // �ֳĸ� ���� �б� �Ǵ� ���Ⱑ �Ϸ�Ǹ� �ش� ���Ͽ� ���� ������ �������� �ϱ� ����
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
