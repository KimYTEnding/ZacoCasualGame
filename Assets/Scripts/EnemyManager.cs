using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Transform enemy;
    public bool attack;

    public TextMeshPro CounterText;
    [SerializeField] private GameObject stickman;

    [SerializeField, Range(0f, 1f)] private float distanceFactor, radius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < Random.Range(20, 120); i++)
        {
            Instantiate(stickman, transform.position, new Quaternion(0f, 180f, 0f, 1f), transform);
        }
        enemy = null;
        attack = false;
        CounterText.text = transform.childCount.ToString();
        FormatStickMan();
    }

    public void Refresh()
    {
        for (int i = 0; i < Random.Range(20, 120); i++)
        {
            Instantiate(stickman, transform.position, new Quaternion(0f, 180f, 0f, 1f), transform);
        }
        enemy = null;
        attack = false;
        CounterText.text = transform.childCount.ToString();
        FormatStickMan();
    }

    // Update is called once per frame
    void Update()
    {
        if (attack && transform.childCount > 1)
        {
            Vector3 enemyPos = new Vector3(enemy.position.x, transform.position.y, enemy.position.z);
            Vector3 enemyDirection = enemy.position - transform.position;


            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation = Quaternion.Slerp(transform.GetChild(i).rotation, Quaternion.LookRotation(enemyDirection, Vector3.up), Time.deltaTime * 3f);
                

                if (enemy.childCount > 1)
                {
                    Vector3 distance = enemy.GetChild(1).position - transform.GetChild(i).position;

                    if (distance.magnitude < 4f)
                    {
                        transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position, enemy.GetChild(1).position, Time.deltaTime * 2.5f);
                    }
                }
            }
        }
    }

    private void FormatStickMan()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            float x = distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * radius);
            float y = distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * radius);

            Vector3 NewPos = new Vector3(x, -0.5066667f, y);

            transform.transform.GetChild(i).localPosition = NewPos;
        }
    }

    public void AttackPlayer(Transform enemyForce)
    {
        enemy = enemyForce;
        attack = true;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("Run", true);
        }
    }

    public void StopAttacking()
    {
        PlayerManager.Instance.gameState = attack = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("Run", false);
        }
    }
}
