using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextScene : MonoBehaviour
{
    public Image load;
    public Text loadingtext;

    private static NextScene Instance;
    private static bool shoulPlayOpeningAnimation = false;
   
    private Animator animator;
    private AsyncOperation loadingSceneOperation;
    
   public static void SwitchToScene(int _SceneLoader)
   {
        Instance.animator.SetTrigger("sceneClosing");

        Instance.loadingSceneOperation = SceneManager.LoadSceneAsync(_SceneLoader);
        Instance.loadingSceneOperation.allowSceneActivation = false;
   } 
    
    private void Start()
    {
        Instance = this;

        animator = GetComponent<Animator>();

        if (shoulPlayOpeningAnimation) animator.SetTrigger("sceneOpening");
    }

    private void Update()
    {
        if (loadingSceneOperation != null)
        {
            loadingtext.text = Mathf.RoundToInt(loadingSceneOperation.progress * 100)+ "%";
            load.fillAmount = loadingSceneOperation.progress;
        }
    }

    public void OnAnimationOver()
    {
        shoulPlayOpeningAnimation = true;
        loadingSceneOperation.allowSceneActivation = true;
    }
}
