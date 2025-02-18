using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class BonusMalusHoldingScript : MonoBehaviour
{

    public string onlyindex ;
    public InputDataOnUIScript IDS;
    public AudioSource AS;
    public ControllMethod _ControllMethod;
    
    private List<BonusObject> LstBonus;
    
    private bool detect = false;
    private bool effect = false;


    public void cancelBonus()
    {
        if (detect)
        {
            LstBonus[indexObject].DisableEffect(this.transform);
        }
        IDS.ImgBonus.enabled = false;
        detect = false;
        effect = false;
    }
    
    private int indexObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tonneau"))
        {
            other.gameObject.GetComponentInParent<TonneauSpawnerScript>().startEffect();
        }
        

        
        if (other.CompareTag("Tonneau") && !detect)
        {
       
            detect = true;
            
            indexObject = Random.Range(0, LstBonus.Count);
            
            if (onlyindex != "")
            {
                do
                {
                    indexObject = Random.Range(0, LstBonus.Count);
                } while (LstBonus[indexObject].getName() != onlyindex);
            }
            
            IDS.ImgBonus.enabled = true;
            IDS.ImgBonus.sprite = LstBonus[indexObject].getCover();
            
            other.gameObject.GetComponentInParent<TonneauSpawnerScript>().Spawn();
     
            LstBonus[indexObject].LoadEffect(this.transform);
            Destroy(other.gameObject);
            
            
    
        }

        if (other.CompareTag("Tonneau") && detect)
        {
            other.gameObject.GetComponentInParent<TonneauSpawnerScript>().Spawn();
            Destroy(other.gameObject);
        }
    }

    private void Start()
    {
        var arr = FindObjectsOfType<MonoBehaviour>().OfType<BonusObject>();
        LstBonus = new List<BonusObject>();
        
        foreach (var s in arr) {
            LstBonus.Add(s);
        }
        
    }

    public UnityEvent GenerateBonus(List<UnityEvent> Evt)
    {
        return Evt[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (LstBonus.Count >=2)
        {
            if ( ( (Input.GetButtonDown("Fire1") && _ControllMethod == ControllMethod.Keyboard) && detect) ||
                ( (Input.GetButtonDown("Fire1C") && _ControllMethod == ControllMethod.Controller) && detect))
            { 
                effect = true;
                detect = false;
            }

            if (effect )
            {
                IDS.ImgBonus.enabled = false;
                LstBonus[indexObject].Starteffect(this.transform);
                effect = false;

                if (LstBonus[indexObject].getName() == "Canon")
                {
                    AS.Play();
                }

            }
        }

    }
}
