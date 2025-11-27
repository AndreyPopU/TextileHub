using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLoad : MonoBehaviour
{
    public static AsyncLoad instance;

    private CanvasGroup group;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        group = GetComponentInChildren<CanvasGroup>();
    }

    public void LoadNextScene() => StartCoroutine(LoadSceneAsync());
  
    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        operation.allowSceneActivation = false;

        while(group.alpha < 1)
        {
            group.alpha += .1f;
            yield return waitForFixedUpdate;
        }

        operation.allowSceneActivation = true;
        yield return new WaitForSeconds(1);

        while (group.alpha > 0)
        {
            group.alpha -= .1f;
            yield return waitForFixedUpdate;
        }
    }
}
