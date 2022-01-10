using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiempoDeVidaSonidos : MonoBehaviour
{
    public float tiempoDeVida;
    // Start is called before the first frame updat
    void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
