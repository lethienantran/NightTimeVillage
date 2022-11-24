using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCharacterSelectionUI : MonoBehaviour
{
    public GameObject CharacterSelectionUI;
    public bool playerIsHere = false;
    // Start is called before the first frame update
    void Start()
    {
        CharacterSelectionUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsHere)
        {
            if (Input.GetKeyUp(KeyCode.O)){
                if (!CharacterSelectionUI.activeSelf)
                {
                    CharacterSelectionUI.SetActive(true);
                }
                else
                {
                    CharacterSelectionUI.SetActive(false);
                }
            }
        }
        else
        {
            CharacterSelectionUI.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsHere = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsHere = false;
        }
    }
}
