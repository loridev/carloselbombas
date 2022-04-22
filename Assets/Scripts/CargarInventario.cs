using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CargarInventario : MonoBehaviour
{
    public Transform prefabFila;
    public Transform prefabBoton;
    public Transform[] contenedores;
    public Text status;
    public Text carlosText;
    public Text carlaText;
    public Button carlosButton;
    public Button carlaButton;
    private bool inProgress = false;
    
    void Start()
    {
        status.text = "Cargando...";
        carlosButton.onClick.AddListener(() => SetCharacter("CARLOS"));
        carlaButton.onClick.AddListener(() => SetCharacter("CARLA"));
        DisplayItems(Globals.CurrentUser.items);
        status.text = "";
    }

    private void DisplayItems(List<Item> items)
    {
        if (items.Count == 0)
        {
            status.text = "No items found";
        }
        else
        {
            int[] itemCounts = {0, 0, 0, 0};
            int arrPos = 0;
            Transform[] actualRows = {null, null, null, null};
            foreach (Item item in items)
            {
                switch (item.type)
                {
                    case "HELMET":
                        arrPos = 0;
                        break;
                    case "BODY":
                        arrPos = 1;
                        break;
                    case "BOMB":
                        arrPos = 2;
                        break;
                    case "BAT":
                        arrPos = 3;
                        break;
                }

                if (itemCounts[arrPos] % 4 == 0)
                {
                    actualRows[arrPos] = Instantiate(prefabFila, contenedores[arrPos]);
                }

                Transform button = Instantiate(prefabBoton, actualRows[arrPos]);
                Image buttonImg = button.gameObject.GetComponent<Image>();
                Text buttonText = button.gameObject.GetComponentInChildren<Text>();
                Button buttonButton = button.GetComponent<Button>();

                if (Globals.CurrentUser.IsEquipped(item))
                {
                    buttonText.color = Color.green;
                }

                buttonText.text = item.skin_texture.ToUpper().Replace('_', ' ');

                switch (item.category)
                {
                    case "RARE":
                        buttonImg.color = Color.blue;
                        break;
                    case "LEGENDARY":
                        buttonImg.color = new Color32(239, 184, 16, 100);
                        break;
                }

                buttonButton.onClick.AddListener(() => ToggleEquipped(item));
                
                itemCounts[arrPos]++;
            }
        }

        switch (Globals.CurrentUser.character)
        {
            case "CARLOS":
                carlosText.color = Color.green;
                break;
            case "CARLA":
                carlaText.color = Color.green;
                break;
        }
    }

    private async void ToggleEquipped(Item item)
    {
        if (!inProgress)
        {
            status.text = "Procesando...";
            inProgress = true;

            if (!await ApiRequests.ToggleEquipped(Globals.Token, item))
            {
                status.text = "Ha ocurrido un error";
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            inProgress = false;
        }
    }

    private async void SetCharacter(string character)
    {
        if (!inProgress && Globals.CurrentUser.character != character)
        {
            status.text = "Procesando...";
            inProgress = true;

            if (!await ApiRequests.SetCharacter(Globals.Token, character))
            {
                status.text = "Ha ocurrido un error";
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            inProgress = false;
        }
    }
}