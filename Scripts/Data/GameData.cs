using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [SerializeField] private bool _isNewGame;
    public bool IsNewGame { get { return _isNewGame; } set { _isNewGame = value; } }

    [SerializeField] private Vector3 _playerPosition;
    public Vector3 PlayerPosition { get { return _playerPosition; } set { _playerPosition = value; } }

    [SerializeField] private PlayerData _playerData;
    public PlayerData PlayerData { get { return _playerData; } set { _playerData = value; } }


    // �� �����ڿ��� �����ϴ� ��� ���� ������ �ʱ� ���� ��
    public GameData()
    {
        SetNewGameData();
    }

    public void SetNewGameData()
    {
        _playerData = new PlayerData();
        _playerPosition = Vector3.zero;
        _isNewGame = false;
    }
}


