using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BarraVida : MonoBehaviour
{
    public float vida = 100;
    public Slider sliderVida;
    // Start is called before the first frame update
    void Start()
    {
        sliderVida = sliderVida.GetComponent<Slider>();
        sliderVida.GetComponent<Slider>().value = vida;
    }

    // Update is called once per frame
    void Update()
    {
        sliderVida.GetComponent<Slider>().value = vida;
    }
}
