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
    async void Start()
    {
        status.text = "Cargando...";
        rankingListIndiv = await ApiRequests.GetRankingsIndiv();
        //rankingListMult = await ApiRequests.GetRankingsMult();
        if (rankingListIndiv == null)
        {
            status.text = "Something went wrong when trying to load some or all the rankings";
        }
        else
        {
            status.text = "";
            LoadRankings("indiv", 1, 2);
        }
    }

    private void LoadRankings(string mode, int worldNum, int levelNum)
    {
        if (mode == "indiv")
        {
            List<Ranking> filtered = rankingListIndiv.FindAll((Ranking ranking) => ranking.world_num == worldNum
                && ranking.level_num == levelNum);
            //TODO : SORT TIME

            for (int i = 0; i < 5; i++)
            {
                if (filtered.Count > i)
                {
                    Transform rankItem = Instantiate(rankingPrefab, container);

                    rankItem.GetComponentsInChildren<Text>()[0].text = filtered[i].user.name;
                    
                    rankItem.GetComponentsInChildren<Text>()[1].text = filtered[i].time + "";
                }
            }
        }
    }
}
