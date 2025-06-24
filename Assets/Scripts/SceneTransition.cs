using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // Nama scene tujuan

    private bool isPlayerInRange = false;

    void Update()
    {
        // Tekan tombol untuk berpindah scene
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    // Trigger masuk area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    // Trigger keluar area
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
