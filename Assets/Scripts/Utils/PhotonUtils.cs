using System.Collections;
using UnityEngine;

namespace Utils
{
    public static class PhotonUtils
    {
        public static IEnumerator PhotonDestroyWithDelay(GameObject obj, float time)
        {
            yield return new WaitForSeconds(time);
            PhotonNetwork.Destroy(obj);
        }
    }
}