using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    public GameObject RestartButton;
    public GameObject BackToMenuButton;
    
    private bool m_Started = false;
    private int m_Points;
    private int brickCount;
    
    //private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FillBoard());
        BestScoreText.text = TopScoreManager.Instance.GetBestScore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        brickCount--;
        if (brickCount <= 0) {
            Ball.gameObject.GetComponent<Ball>().AddMaxSpeed();
            StartCoroutine(FillBoard());
        }
    }

    public void GameOver()
    {
        //m_GameOver = true;
        GameOverText.SetActive(true);
        RestartButton.SetActive(true);
        BackToMenuButton.SetActive(true);
    }

    private IEnumerator FillBoard()
    {
        yield return new WaitUntil(() => Ball.gameObject.transform.position.y <= 2.0f);
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        brickCount = 0;
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
                brickCount++;
            }
        }
    }
}
