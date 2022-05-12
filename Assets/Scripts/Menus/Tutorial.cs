using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BGSoundScript.BackMusicPause();
        StartCoroutine(AcabarVideo());
    }

    private IEnumerator AcabarVideo()
    {
        yield return new WaitForSeconds(61);
        CargarMapa();
    }

    public void CargarMapa()
    {
        SceneManager.LoadScene("MapaDinamicoFinal");
    }
}
