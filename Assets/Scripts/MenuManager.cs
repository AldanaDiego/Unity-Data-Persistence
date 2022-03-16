using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Text topScoresText;
    [SerializeField] private InputField nameInput;
    [SerializeField] private Button startButton;

    // Start is called before the first frame update
    void Start()
    {
        this.topScoresText.text = TopScoreManager.Instance.GetTopScoresString();
        this.nameInput.onValueChanged.AddListener(delegate {NameInputValueChanged();});
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NameInputValueChanged()
    {
        if (this.nameInput.text == "") {
            this.startButton.interactable = false;
        } else {
            this.startButton.interactable = true;
        }
    }

    public void StartGame()
    {
        TopScoreManager.Instance.SetCurrentPlayer(this.nameInput.text);
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        TopScoreManager.Instance.SaveTopScores();
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
