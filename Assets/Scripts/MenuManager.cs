using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Text topScoresText;

    // Start is called before the first frame update
    void Start()
    {
        this.topScoresText.text = TopScoreManager.Instance.GetTopScoresString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
