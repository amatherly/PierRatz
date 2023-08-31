using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button start;
    [SerializeField] private Button quit;
    [SerializeField] private Button options;
    [SerializeField] private Button credits;
    
    
    // Start is called before the first frame update
    void Start()
    {
        start.onClick.AddListener(LoadNextScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void LoadNextScene()
    {
        int nextSceneToLoad = (2);
        LoadingScreenController.LoadScene(nextSceneToLoad);
    }

}
