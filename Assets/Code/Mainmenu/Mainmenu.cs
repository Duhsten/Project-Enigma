using ProjectEnigma.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mainmenu : MonoBehaviour
{
    [Header("Main Menu")]
    public TMPro.TMP_Text gameNameText;
    public TMPro.TMP_Text gameVersionText;
    public GameObject loadGameMenu;

    [Space(5)]
    [Header("Load Game Menu")]
    public GameObject SaveListUI;
    public Save SelectedSave;
    public Button LoadSaveButtom;
    // Start is called before the first frame update
    void Start()
    {
        gameNameText.text = ProjectEnigma.Info.Config.GAME_NAME;
        gameVersionText.text = $"v{ProjectEnigma.Info.Config.GAME_VERSION}\n Hollow Foot Studios";
    }

    // Update is called once per frame
    void Update()
    {
        if(SelectedSave != null)
            LoadSaveButtom.interactable = true;
        else
            LoadSaveButtom.interactable = false;

    }
    public void NewGame()
    {
        ProjectEnigma.Data.SaveManager.CurrentSave = new ProjectEnigma.Data.Save()
        {
            Name = $"save{System.Guid.NewGuid().ToString()}",
        };
        SceneManager.LoadScene(2);
    }
    public void LoadGameMenu()
    {
        loadGameMenu.SetActive(true);
        LoadSavedGames();
        LoadSaveButtom.interactable = false;
    }
    public void LoadOptionsMenu()
    {

    }
    public void QuitGame()
    {

    }

    #region LoadGame
    
    public void LoadSavedGames()
    {
        var saveList = ProjectEnigma.Data.SaveManager.GetSaves();
        foreach (var save in saveList)
        {
            var obj = GameObject.Instantiate(Resources.Load("Prefabs/SaveListSlot") as GameObject);
            obj.transform.parent = SaveListUI.transform;
            obj.GetComponent<SaveInfoSlot>().SetInfo(save);
        }
    }
    public void LoadSelectedSave()
    {
        ProjectEnigma.Data.SaveManager.CurrentSave = SelectedSave;
        SceneManager.LoadScene(2);
    }
    public void UpdateSelectedSave(Save save)
    {
        SelectedSave = save;
    }
    public void CloseLoadSaveMenu()
    {
        loadGameMenu.SetActive(false);
    }
    #endregion
}
