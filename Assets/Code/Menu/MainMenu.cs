using UnityEngine;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void LoadScene(string scene)
    {
        ServiceProvider.Instance.GetService<SceneLoader>().LoadScene(scene);
    }
}
