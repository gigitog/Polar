using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }
}