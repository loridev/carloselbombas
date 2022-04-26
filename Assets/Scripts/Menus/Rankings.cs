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
        rankingListMult = await ApiRequests.GetRankingsMult();
        if (rankingListIndiv == null || rankingListMult == null)
        {
            status.text = "ERROR";
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
        
        modoMenos.onClick.AddListener(ChangeMode);
        
        modoMas.onClick.AddListener(ChangeMode);
    }

    private void LoadRankings(string mode, int worldNum = 0, int levelNum = 0)
    {
        if (rankingsVisibles.Count > 0)
        {
            foreach (Transform tr in rankingsVisibles)
            {
                if (tr != null) Destroy(tr.gameObject);
            }  
        }
        
        if (mode == "indiv")
        {
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
                    Text textName = rankItem.GetComponentsInChildren<Text>()[0];
                    Text textScore = rankItem.GetComponentsInChildren<Text>()[1];
                    
                    Color colorText = Globals.CurrentUser.Equals(filtered[i].user) ? Color.yellow : Color.white;

                    textName.color = colorText;
                    textScore.color = colorText;

                    textName.text = filtered[i].user.name;
                    textScore.text = filtered[i].time + "";
                    
                    rankingsVisibles.Add(rankItem);
                }
            }
        } else if (mode == "mult")
        {
            List<User> filtered = new List<User>(rankingListMult);

            if (filtered.Count == 0)
            {
                filtered.Add(new User("No rankings found: ", 404));
            }
            filtered.Sort();

            for (int i = 0; i < 5; i++)
            {
                if (filtered.Count > i)
                {
                    Transform rankItem = Instantiate(rankingPrefab, container);
                    Text textName = rankItem.GetComponentsInChildren<Text>()[0];
                    Text textScore = rankItem.GetComponentsInChildren<Text>()[1];
                    
                    Color colorText = Globals.CurrentUser.Equals(filtered[i]) ? Color.yellow : Color.white;

                    textName.color = colorText;
                    textScore.color = colorText;

                    textName.text = filtered[i].name;
                    textScore.text = filtered[i].multi_wins + "";
                    
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

    private void ChangeMode()
    {
        string modoCheck = textoModo.text == "INDIVIDUAL" ? "mult" : "indiv";
        textoModo.text = modoCheck == "indiv" ? "INDIVIDUAL" : "MULTIJUGADOR";
        
        LoadRankings(
            modoCheck,
            modoCheck == "indiv" ? worldNumGlob : 0,
            modoCheck == "indiv" ? levelNumGlob : 0
        );

        textoNivel.color = modoCheck == "indiv" ? Color.white : Color.grey;
        if (modoCheck == "mult")
        {
            nivelMenos.enabled = false;
            nivelMenos.GetComponent<Image>().color = nivelMenos.enabled ? Color.white : Color.grey;
            nivelMas.enabled = false;
            nivelMas.GetComponent<Image>().color = nivelMas.enabled ? Color.white : Color.grey;
        }
        else
        {
            CheckBtns();
        }
    }
}
