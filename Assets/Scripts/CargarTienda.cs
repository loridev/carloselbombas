using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CargarTienda : MonoBehaviour
{
    public Transform prefabFila;
    public Transform prefabBoton;
    public Transform contenedor;
    public Text labelMonedas;
    
    // Start is called before the first frame update
    async void Start()
    {
        Item[] shopItems = await ApiRequests.GetShop();

        DisplayItems(shopItems);
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

            button.gameObject.GetComponentInChildren<Text>().text = items[i].skin_texture.Replace('_', ' ').ToUpper()
                                                 + "\n" + items[i].type.ToUpper() + "\n" + items[i].price + " monedas";

            Image buttonImg = button.gameObject.GetComponent<Image>();

            switch (items[i].category)
            {
                case "RARE":
                    buttonImg.color = Color.blue;
                    break;
                case "LEGENDARY":
                    buttonImg.color = new Color32(239, 184, 16, 100);
                    break;
            }
            
            //TODO: CHECKIAR SI EL USUARIO TIENE EL ITEM
        }
    }
}
