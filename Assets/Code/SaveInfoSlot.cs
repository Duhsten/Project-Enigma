using ProjectEnigma.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveInfoSlot : MonoBehaviour
{
    public TMPro.TMP_Text saveName;
    public TMPro.TMP_Text saveTime;
    public Button clickAction;

    public void SetInfo(Save save)
    {
        saveName.text = save.Name;
        saveTime.text = save.LastPlayed.ToString();
        GetComponent<Button>().onClick.AddListener(() => { GameObject.FindGameObjectWithTag("Mainmenu").GetComponent<Mainmenu>().UpdateSelectedSave(save); });

    }
}
