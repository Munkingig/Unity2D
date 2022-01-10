using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform John;

    void Update()//Se actualiza a cada frame por segundo.
    {
        if (John != null)//Si el PJ existe.
        {
            Vector3 position = transform.position;//el vector posicion es igual al componente transform.position
            position.x = John.position.x; //La posicion x es igual a donde esta el PJ.x
            transform.position = position;//La posicion del componente transform.position es igual a la posicion(donde esta el PJ respecto "x")
        }
    }
}
