using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private GameObject healthTextPrefab;

    [SerializeField] private Canvas gameCanvas;

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject successSave;

    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject confirmExit;
    [SerializeField] private GameObject continueButton;

    [SerializeField] private GameObject Restart;


    private void Awake()
    {
        gameCanvas = FindObjectOfType<Canvas>();
    }
    private void OnEnable()
    {
        CharacterEvents.s_characterDamaged += CharacterTookDamage;
        CharacterEvents.s_characterHealed += CharacterHealed;
        CharacterEvents.s_playerDie += PopupRestartingUI;
    }

    private void OnDisable()
    {
        CharacterEvents.s_characterDamaged -= CharacterTookDamage;
        CharacterEvents.s_characterHealed -= CharacterHealed;
        CharacterEvents.s_playerDie -= PopupRestartingUI;
    }

    private void PopupRestartingUI()
    {
        Managers.Sound.Play("Sounds/BGM/MS02gameover2V1NL", Define.Sound.Bgm);
        Managers.Sound.StopLoop();
        Restart.SetActive(true);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        // Create text at character hit
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = damageReceived.ToString();   
    }

    public void CharacterHealed(GameObject character, int healthRestored = 1, string _string = "")
    {
        // Create text at character hit
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        if(_string == "")
        {
            tmpText.text = healthRestored.ToString();
        }
        else
        {
            tmpText.text = _string;
        }
        
    }

    public void OnClickSaveGameButton()
    {
        DataPersistanceManager.Instance.SaveGame();
        successSave.SetActive(true);
    }

    public void OnClickExitGameButton()
    {
        confirmExit.SetActive(true);


    }

    public void ExitGame()
    {
        #if (UNITY_EDITOR) // 유니티에디터에서 할 경우
                UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_STANDALONE) // PC같은 독립적인곳에서 할 경우
                            Application.Quit()
        #endif
    }


    public void OnExit(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(menu.activeSelf)
            {
                menu.SetActive(false);
            }
            else if(!menu.activeSelf) 
            {
                menu.SetActive(true);
            }

            //#if(UNITY_EDITOR || DEVELOPMENT_BUILD) // 에디터와 개발빌드에서만 실행 됨
            //            Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            //#endif

            //#if(UNITY_EDITOR) // 유니티에디터에서 할 경우
            //            UnityEditor.EditorApplication.isPlaying = false;
            //#elif(UNITY_STANDALONE) // PC같은 독립적인곳에서 할 경우
            //            Application.Quit();
            //#elif(UNITY_WEBGL) // WebGL을 사용할 경우
            //            SceneManager.LoadScene("QuitScene");
            //#endif
        }
    }
}
