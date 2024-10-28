using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text energyText; // play butonunun text'ine kalan enerji yazdırılacak
    [SerializeField] private Button playButton;
    [SerializeField] private AndroidNotificationHandler androidNotificationHandler; // inspector'de AndroidNotificationHandler.cs'i referans olarak vereceğiz
    [SerializeField] private IOSNotificationHandler iOSNotificationHandler; // inspector'de AndroidNotificationHandler.cs'i referans olarak vereceğiz
    [SerializeField] private int maxEnergy = 5; // maksimum enerji
    // bir dakika sonra beşe kadar tamamen şarj olur
    [SerializeField] private int energyRechargeDuration = 1; // şarj süresi

    private int energy;

    private const string EnergyKey = "Energy"; // kalan enerji
    private const string EnergyReadyKey = "EnergyReady"; // şarj süresinin bittiği zaman

    private void Start()
    {
        OnApplicationFocus(true);

    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            return;
        }

        CancelInvoke();

        // highScoreText'e ScoreSystem.cs'de hesaplanan en yüksek skor değerini getireceğiz
        int highScore = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);
        highScoreText.text = $"High Score: {highScore}";

        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

        if (energy == 0) // enerji bittiyse şarj edilmesi gerekir
        {
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);
            if (energyReadyString == string.Empty)
            {
                return;
            }

            // enerjinin hazır olduğu tarih
            DateTime energyReady = DateTime.Parse(energyReadyString);

            if (DateTime.Now > energyReady)
            {
                // şu an, şarj süresinin bittiği zamanı geçtiyse
                energy = maxEnergy;
                PlayerPrefs.SetInt(EnergyKey, energy);
            }
            else
            {
                playButton.interactable = false;
                // ana menüye gittiğimizde enerji hazır değilse ne kadar süre kaldığını bulmamız gerekir.
                Invoke(nameof(EnergyRecharged), (energyReady - DateTime.Now).Seconds);
            }
        }

        energyText.text = $"Play ({energy})"; // PlayButtonText
    }

    private void EnergyRecharged()
    {
        // şarj süresi bittiyse
        playButton.interactable = true;
        energy = maxEnergy;
        PlayerPrefs.SetInt(EnergyKey, energy);
        energyText.text = $"Play ({energy})"; // PlayButtonText
    }

    // play butonuna bastığımızda bu fonksiyon çalışacak
    public void Play()
    {
        if (energy < 1) { return; }

        energy--; // her engele çarptığında enerji 1 birim azalır

        PlayerPrefs.SetInt(EnergyKey, energy);

        if (energy == 0)
        {
            // şu anki zamana "energyRechargeDuration" dakika eklersek sarj süresinin bittiği zamanı (energyReady) buluruz
            DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeDuration);
            PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());


#if UNITY_ANDROID
            // Android için geliştirmediğimiz sürece derlenmeyecek.
            androidNotificationHandler.ScheduleNotification(energyReady);
#elif UNITY_IOS
            iOSNotificationHandler.ScheduleNotification(energyRechargeDuration);
#endif
        }

        SceneManager.LoadScene(1);
    }
}
