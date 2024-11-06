using TMPro;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    public TextMeshPro GateNo;
    public int randomNumber;
    public bool multiply;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if (Random.Range(0, 2) == 1)
        //{
        //    multiply = true;
        //}
        //else
        //{
        //    multiply = false;
        //}

        if (multiply)
        {
            randomNumber = Random.Range(1, 3);
            GateNo.text = "X" + randomNumber;
        }
        else
        {
            randomNumber = Random.Range(10, 100);
            if (randomNumber % 2 != 0)
            {
                randomNumber += 1;
            }
            GateNo.text = "+" + randomNumber.ToString();
        }
    }

    public void Refresh()
    {
        if (Random.Range(0, 2) == 1)
        {
            multiply = true;
        }
        else
        {
            multiply = false;
        }

        if (multiply)
        {
            randomNumber = Random.Range(1, 4);
            GateNo.text = "X" + randomNumber;
        }
        else
        {
            randomNumber = Random.Range(10, 100);
            if (randomNumber % 2 != 0)
            {
                randomNumber += 1;
            }
            GateNo.text = "+" + randomNumber.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
