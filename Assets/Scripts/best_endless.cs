using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class best_endless : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;

    void Start()
    {
        int time = SaveManager.instance.activeSave.endless_time;
        _text.text = $"Best Time: {time/60}:{time%60}";
    }
}
