using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyMapping : MonoBehaviour
{

    [SerializeField] private GameObject inventory;
    private KeyCode[] quickSlotKeys;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        quickSlotKeys = new KeyCode[3] {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3};
    }

    // Update is called once per frame
    void Update()
    {
        // I key : inventory on/off
        if (Input.GetKeyDown(KeyCode.I)) {
            inventory.SetActive(true);
        }

        // 123 key : use quickslot
        foreach(var key in quickSlotKeys) {
            if (Input.GetKeyDown(key)) {
                // consume corresponding quickslot
            }
        }
    }
}
