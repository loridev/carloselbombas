using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CargarTienda : MonoBehaviour
{
    public Transform prefabFila;
    public Transform prefabBoton;
    public Transform contenedor;
    public Text labelMonedas;
    public Text status;
    private bool inProgress = false;
    
    // Start is called before the first frame update
    async void Start()
    {
        status.text = "Cargando...";
        Item[] shopItems = await ApiRequests.GetShop();

        DisplayItems(shopItems);
        status.text = "";
    }

    private void DisplayItems(Item[] items)
    {
        labelMonedas.text = "Monedas: " + Globals.CurrentUser.money;
        Transform actualRow = null;
        
        for (int i = 0; i < items.Length; i++)
        {
            if (i % 4 == 0)
            {
                actualRow = Instantiate(prefabFila, contenedor);
            }

            Transform button = Instantiate(prefabBoton, actualRow);
            Image buttonImg = button.gameObject.GetComponent<Image>();
            Text buttonText = button.gameObject.GetComponentInChildren<Text>();
            Button buttonButton = button.GetComponent<Button>();

            if (Globals.CurrentUser.HasItem(items[i]))
            {
                buttonText.text = items[i].skin_texture.Replace('_', ' ').ToUpper()
                                                                        + "\n" + items[i].type.ToUpper() + "\n" + " (OWNED)";
                buttonText.color = Color.gray;

                buttonButton.enabled = false;
            }
            else
            {
                int i1 = i;
                buttonButton.onClick.AddListener(() => BuyItem(items[i1]));
                buttonText.text = items[i].skin_texture.Replace('_', ' ').ToUpper() 
                                                                        + "\n" + items[i].type.ToUpper() + "\n" + items[i].price + " monedas";
                switch (items[i].category)
                {
                    case "RARE":
                        buttonImg.color = Color.blue;
                        break;
                    case "LEGENDARY":
                        buttonImg.color = new Color32(239, 184, 16, 100);
                        break;
                }
            }
        }
    }

    private async void BuyItem(Item item)
    {
        if (!inProgress)
        {
            status.text = "Procesando compra...";
            inProgress = true;

            if (Globals.CurrentUser.money >= item.price)
            {
                if (await ApiRequests.BuyItem(item.id, Globals.Token))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    Globals.CurrentUser.items.Add(item);
                    Globals.CurrentUser.money -= item.price;
                }
            }
            
            inProgress = false;

            status.text = "";
        }
    }
}
