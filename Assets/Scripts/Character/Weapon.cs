using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public int damageAmount = 20;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Animal animal = other.GetComponent<Animal>();
        if (animal != null)
        {
            animal.TakeDamage(damageAmount);
            Debug.Log("Hit " + animal.animalName + " for " + damageAmount + " damage.");
        }
    }
}
