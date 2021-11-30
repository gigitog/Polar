using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControllerAR : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) BackToMenu();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(1);
    }
}