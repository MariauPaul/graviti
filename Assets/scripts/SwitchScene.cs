using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
            CheckPhysic();
            SceneManager.LoadScene(2);
        }
        if (currentScene.name == "MAP2")
        {
            CheckPhysic();
            SceneManager.LoadScene(3);
        }
        if (currentScene.name == "MAP3")
        {
            CheckPhysic();
            SceneManager.LoadScene(4);
        }
        if (currentScene.name == "MAP4")
        {
            CheckPhysic();
            SceneManager.LoadScene(5);
        }
    }

    private void CheckPhysic()
    {
        if (Physics.gravity.y > 0)
        {
            Physics.gravity *= -1;
        }
    }
}
