using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public bool lang = false;
    public GameObject EnglishPrefab;
    public GameObject DanskPrefab;
    public GameObject dnew, dlang;
    public GameObject enew, elang;


    void Start()
    {
        EnglishPrefab = GameObject.Find("English");
        DanskPrefab = GameObject.Find("Danish");

        enew = GameObject.Find("SelectionSliderNewGame");
        elang = GameObject.Find("SelectionSliderLanguage");

        dnew = GameObject.Find("SelectionSliderNytSpil");
        dlang = GameObject.Find("SelectionSliderSprog");


    }

    void FixedUpdate () {



        if (lang == false)
        {


            DanskPrefab.transform.position = new Vector3(1000, -100, -1000);
            EnglishPrefab.transform.position = new Vector3(0, 0, 20);


        }
        else
        {


            DanskPrefab.transform.position = new Vector3(0, 0, 20);
            EnglishPrefab.transform.position = new Vector3(1000, -100, -1000);


        }


        if (elang.GetComponent<SelectionSlider>().m_BarFilled == true || dlang.GetComponent<SelectionSlider>().m_BarFilled == true)
        {
            StartCoroutine("changeLanguage", 0.5f);
            elang.GetComponent<SelectionSlider>().m_BarFilled = false;
            dlang.GetComponent<SelectionSlider>().m_BarFilled = false;
        }




        if (enew.GetComponent<SelectionSlider>().m_BarFilled == true || dnew.GetComponent<SelectionSlider>().m_BarFilled == true)
        {
            SceneManager.LoadScene("Game");
        }




    }

    IEnumerator changeLanguage(float delay)
    {
           if(lang != !lang)
        {
            lang = !lang;
            
        }

        yield return new WaitForSeconds(delay);

    }
}
