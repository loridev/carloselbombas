using System.Collections;
using System.Collections.Generic;
using DB.Models;
using UnityEngine;
using UnityEngine.UI;

public class Rankings : MonoBehaviour
{
    private List<Ranking> rankingListIndiv;
    private List<User> rankingListMult;
    public Text status;
    public Transform container;
    public Transform rankingPrefab;
    public Button nivelMenos;
    public Button nivelMas;
    public Button modoMenos;
    public Button modoMas;
    public Text textoNivel;
    public Text textoModo;
    private int worldNumGlob = 1;
    private int levelNumGlob = 1;
    private List<Transform> rankingsVisibles;

    async void Start()
    {
        status.text = "Cargando...";
        rankingsVisibles = new List<Transform>();
        rankingListIndiv = await ApiRequests.GetRankingsIndiv();
        //rankingListMult = await ApiRequests.GetRankingsMult();
        if (rankingListIndiv == null)
        {
            status.text = "Something went wrong when trying to load some or all the rankings";
        }
        else
        {
            status.text = "";
            LoadRankings("indiv", 1, 1);
            nivelMenos.enabled = false;
        }
        
        nivelMenos.onClick.AddListener(() =>
        {
            worldNumGlob = levelNumGlob == 1 ? worldNumGlob -= 1 : worldNumGlob;
            levelNumGlob = levelNumGlob == 1 ? 4 : levelNumGlob -= 1;
            LoadRankings(
                "indiv",
                worldNumGlob,
                levelNumGlob
            );
            CheckBtns();
        });
        
        nivelMas.onClick.AddListener(() =>
        {
            worldNumGlob = levelNumGlob == 4 ? worldNumGlob += 1 : worldNumGlob;
            levelNumGlob = levelNumGlob == 4 ? 1 : levelNumGlob += 1;
            LoadRankings(
                "indiv",
                worldNumGlob,
                levelNumGlob
            );
            CheckBtns();
        });
    }

    private void LoadRankings(string mode, int worldNum = 0, int levelNum = 0)
    {
        if (mode == "indiv")
        {
            if (rankingsVisibles.Count > 0)
            {
                foreach (Transform tr in rankingsVisibles)
                {
                    if (tr != null) Destroy(tr.gameObject);
                }  
            }
            textoNivel.text = "NIVEL " + worldNum + "-" + levelNum;
            List<Ranking> filtered = rankingListIndiv.FindAll((Ranking ranking) => ranking.world_num == worldNum
                && ranking.level_num == levelNum);

            if (filtered.Count == 0)
            {
                filtered.Add(new Ranking(404, new User("No rankings found: ")));
            }
            filtered.Sort();

            for (int i = 0; i < 5; i++)
            {
                if (filtered.Count > i)
                {
                    Transform rankItem = Instantiate(rankingPrefab, container);

                    rankItem.GetComponentsInChildren<Text>()[0].text = filtered[i].user.name;
                    
                    rankItem.GetComponentsInChildren<Text>()[1].text = filtered[i].time + "";
                    
                    rankingsVisibles.Add(rankItem);
                }
            }
        }
    }

    private void CheckBtns()
    {
        nivelMenos.enabled = worldNumGlob != 1 || levelNumGlob != 1;
        nivelMenos.GetComponent<Image>().color = nivelMenos.enabled ? Color.white : Color.grey;
        
        nivelMas.enabled = worldNumGlob != 3 || levelNumGlob != 4;
        nivelMas.GetComponent<Image>().color = nivelMas.enabled ? Color.white : Color.grey;
    }
}
