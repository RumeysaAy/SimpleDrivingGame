using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier; // Her saniyede kaç puan kazanılsın?

    public const string HighScoreKey = "HighScore";

    private float score;

    // Update is called once per frame
    void Update()
    {
        // daha uzun süre hayatta kalırsanız daha fazla puan alırsınız
        score += Time.deltaTime * scoreMultiplier; // her saniyede 5 puan kazansın

        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    // bu nesne yok edildiğinde çağrılır
    private void OnDestroy()
    {
        // default değeri sıfırdır.
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0); // en yüksek değeri tutacak

        // eğer önceki skor, şu anki skordan küçükse HighScore'a şu anki skor değeri atanır.
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, Mathf.FloorToInt(score));
        }

    }
}
