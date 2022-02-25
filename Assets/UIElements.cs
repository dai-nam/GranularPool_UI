using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElements : MonoBehaviour
{
    public void EnableUiElements()
    {
        gameObject.SetActive(true);
    }

    public void DisableUiElements()
    {
        gameObject.SetActive(false);
    }
}
