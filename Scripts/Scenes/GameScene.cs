using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameScene : BaseScene, IDataPersistance
{
    [SerializeField]
    private bool _isNewGame;

    GameData gameData;
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        

        //Managers.UI.ShowSceneUI<UI_Inven>();

        //Dictionary<int, Stat> dict = Managers.Data.StatDict;

    }

    private void Start()
    {
        Managers.Sound.Play("Sounds/BGM/carefree_loop", Define.Sound.Bgm);

    }

    private void OnEnable()
    {
        CharacterEvents.s_playerDie += SetNewGame;
    }

    private void OnDisable()
    {
        CharacterEvents.s_playerDie += SetNewGame;
    }

    public override void Clear()
    {

    }

    public void SetNewGame()
    {
        if(gameData != null)
        {
            gameData.SetNewGameData();
            _isNewGame = false;
            DataPersistanceManager.Instance.SaveGame();
            //Debug.Log("뉴게임상태로 변경됐습니다");
        }
    }

    public void LoadData(GameData data)
    {
        if(!data.IsNewGame)
        {
            _isNewGame = true;
        }
        else
        {
            _isNewGame = data.IsNewGame;
        }

        gameData = data;
    }

    public void SaveData(ref GameData data)
    {

        data.IsNewGame = _isNewGame;
        
    }
}
