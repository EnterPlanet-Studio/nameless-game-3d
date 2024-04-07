using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class lvlManager : MonoBehaviour
{
    int sceneCount = 0;
    [SerializeField] private GameObject btnPrefab;
    [SerializeField] private Transform forBtns;
    [SerializeField] private AudioSource _click;
    [SerializeField] private Transform lvlPanel;
    [SerializeField] private TMP_Text lvlText;
    [SerializeField] private TMP_Text bestTimeText;

    private int _curLvl = 0;


    void Start()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings - 2;
        for (int i = 1; i <= sceneCount; i++) {
            GameObject btn = Instantiate(btnPrefab, forBtns.transform);
            btn.transform.SetParent(forBtns);
            btn.transform.localScale = new Vector3(1,1,1);
            EventTrigger trigger = btn.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;

            int j = i;

            if (i == 1)   btn.GetComponentInChildren<TMP_Text>().text = "Tutorial";
            else          btn.GetComponentInChildren<TMP_Text>().text = $"{j-1}";
    
            if (i > SaveManager.instance.activeSave.unlocked + 1) btn.GetComponent<Button>().interactable = false;
            else {
                entry.callback.AddListener((lvlbtnfun) => { OpenPanel(j); });
                trigger.triggers.Add(entry);
            }
        }
    }

    public void OpenPanel(int level) {
        int time = 0;
        if (level <= SaveManager.instance.activeSave.time.Count) time = SaveManager.instance.activeSave.time[level - 1];
        _click.Play();
        if (level == 1)   lvlText.text = "Tutorial";
        else              lvlText.text = $"Level {level - 1}";
        bestTimeText.text = $"Best Time: {time/60}:{time%60}";
        _curLvl = level;
        lvlPanel.gameObject.SetActive(true);
    }
    public void ClosePanel() {
        lvlPanel.gameObject.SetActive(false);
    }
    public void LoadLevel() {
        SceneManager.LoadScene(_curLvl);
    }
}
