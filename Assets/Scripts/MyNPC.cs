using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyNPC : MonoBehaviour
{
    // Start is called before the first frame update
    public float displayTime = 5f;
    public GameObject dialogBox;
    float timerDisplay;
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerDisplay > 0)
        {
            timerDisplay -= Time.deltaTime;
            if(timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void ShowDialog(string dialog)
    {
        timerDisplay = displayTime;
        // dialogBox.GetComponent<Text>().text = dialog;
        dialogBox.SetActive(true);
    }
}
