using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // başlangıç hızı
    [SerializeField] private float speedGainPerSecond = 0.3f; // her saniyede eklenen hız miktarı
    [SerializeField] private float turnSpeed = 200f; // dönüş hızı

    private int steerValue; // sağa mı gidiyor yoksa sola mı?

    // Update is called once per frame
    void Update()
    {
        // Oyunu zorlaştırmak için her saniye hızı sabit miktarda arttıracağım
        speed += speedGainPerSecond * Time.deltaTime;
        // Sağa veya sola dönmek için y eksenini kullanacağım. -y'ye doğru ise sola, +y'ye doğru ise sağa gidebilirim.
        transform.Rotate(0f, steerValue * turnSpeed * Time.deltaTime, 0f); // steerValue değerine göre ilerler
        // araba +z yönünde ilerliyor
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ne zaman bir engelle çarpışma tespit edilirse ana menü sahnesini yüklenecek
        if (other.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene(0); // "SceneMainMenu"
        }
    }

    public void Steer(int value)
    {
        // ekranın sol tarafına dokunulduğunda sola doğru
        // sağ tarafına dokunulduğunda ise sağa doğru ilerler

        //
        steerValue = value;


    }
}
