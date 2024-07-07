using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public int[] test;
    public void StartButton()
    {
        SceneManager.LoadScene("deningRoomScene");
    }
    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
