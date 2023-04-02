using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    void LoadData(GameData data);

    // ref를 쓰는 이유는 데이터를 저장할 때 실제로
    // 구현 스크립트가 데이터를 수정하도록 허용하기를 원하기 때문
    void SaveData(ref GameData data);
}
