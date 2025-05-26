//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.InteropServices.WindowsRuntime;
//using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine;
//using UnityEngine.UI;

//public class SelectionManager : MonoBehaviour
//{
//    public SelectionManager Instance { get; set; }

//    public bool onTarget;
//    public GameObject selectedObject;
//    public GameObject interaction_Info_UI;
//    Text interaction_text;

//    public Image centerDotImage;
//    public Image handIcon;

//    private void Start()
//    {
//        onTarget = false;
//        interaction_text = interaction_Info_UI.GetComponent<Text>();
//    }

//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//        }
//        else
//        {
//            Instance = this;
//        }
//    }

//    private void Update()
//    {
//        Animal  animal = GetComponent<Animal>();

//        if (animal && animal.playerInRange)
//        {
//            interaction_text.text = animal.animalName;
//            interaction_Info_UI.SetActive(true);

//            if (Input.GetMouseButtonDown(0) && Instance.IsHoldingWeapon())
//            {
//                StartCoroutine(DealDamageTo(animal, 0, 3f, Instance.GetWeaponDamage()));
//            }
//            else
//            {
//                interaction_text.text = "";
//                interaction_Info_UI.SetActive(false);
//            }
//        }
//    }

//    internal int GetWeaponDamage()
//    {
//        if
//        GetComponent<Weapon>().weaponDamage = 10;
//    }
    

//    private bool IsHoldingWeapon()
//    {
        
//    }

//    IEnumerator DealDamageTo(Animal animal, float delay, int damage)
//    {
//        yield return new WaitForSeconds(delay);

//        animal.TakeDamage(damage);
//    }
//}
