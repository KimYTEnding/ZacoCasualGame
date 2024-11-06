using System.Collections;
using UnityEngine;

public class DestroyPopcorn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(1f);
        Destroy(gameObject);
    }
}
