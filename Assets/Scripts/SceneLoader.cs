using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadScenes(int id)
    {
        SceneManager.LoadScene(id);
    }
}