using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    void LoadData(GameData data);

    // ref�� ���� ������ �����͸� ������ �� ������
    // ���� ��ũ��Ʈ�� �����͸� �����ϵ��� ����ϱ⸦ ���ϱ� ����
    void SaveData(ref GameData data);
}
