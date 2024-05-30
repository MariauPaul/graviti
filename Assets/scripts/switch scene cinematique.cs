using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class switchscenecinematique : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(cinestart());
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }
    IEnumerator cinestart()
    {  yield return new WaitForSeconds(17.10f);
        SceneManager.LoadScene(1);
    }
}
