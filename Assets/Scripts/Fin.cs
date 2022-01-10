using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fin : MonoBehaviour
{
    BarraVida lifeBar;
    // Start is called before the first frame update
    void Start()
    {
        lifeBar = GameObject.Find("Slider").GetComponent<BarraVida>();
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeBar.GetComponent<BarraVida>().vida <= 0 ){
            SceneManager.LoadScene("Start");
        }
    }
}
