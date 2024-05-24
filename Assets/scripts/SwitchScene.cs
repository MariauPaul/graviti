using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
     public Scene currentScene;
    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (currentScene.name == "MAP1")
        {
            SceneManager.LoadScene(2);
        }
        if (currentScene.name == "MAP2")
        {
            SceneManager.LoadScene(3);
        }
        if (currentScene.name == "MAP3")
        {
            SceneManager.LoadScene(4);
        }
        if (currentScene.name == "MAP4")
        {
            SceneManager.LoadScene(5);
        }
    }
}
