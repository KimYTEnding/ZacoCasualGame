using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public Transform player;
    [SerializeField]private int stickmansCount, EnemyCount;
    [SerializeField]private bool attack;

    [SerializeField] private TextMeshPro CounterText;
    [SerializeField] private GameObject stickman;
    [SerializeField] private Transform[] roads;
    [SerializeField] private Transform[] enemys;
    [SerializeField] private Transform enemy;
    //[SerializeField] private Transform arrow;

    [SerializeField, Range(0f, 1f)] private float distanceFactor, radius;

    public bool gameState;
    private Vector3 mouseStartPos, playerStartPos;
    public float playerSpeed, roadSpeed;
    private Camera camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1.0f;
        player = transform;
        stickmansCount = transform.childCount - 1;

        CounterText.text = stickmansCount.ToString();

        camera = Camera.main;

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        GameUIManager.instance.SetScore(stickmansCount);
        if (attack)
        {
            Vector3 enemyDirection = new Vector3(enemy.position.x, transform.position.y, enemy.position.z) - transform.position;

            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation =
                    Quaternion.Slerp
                    (
                        transform.GetChild(i).rotation,
                        Quaternion.LookRotation(enemyDirection, Vector3.up),
                        Time.deltaTime * 3f
                    );
            }

            if (enemy.GetChild(1).childCount > 1)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Vector3 Distance = enemy.GetChild(1).GetChild(0).position - transform.GetChild(i).position;

                    if (Distance.magnitude < 4f)
                    {
                        transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position,
                            new Vector3(enemy.GetChild(1).GetChild(0).position.x, transform.GetChild(i).position.y,
                            enemy.GetChild(1).GetChild(0).position.z), Time.deltaTime * 1.5f);
                    }
                }
            }
            else
            {
                attack = false;
                roadSpeed = 4f;

                FormatStickMan();

                for (int i = 1; i < transform.childCount; i++)
                {
                    transform.GetChild(i).rotation = Quaternion.identity;
                }


                enemy.gameObject.SetActive(false);
                
                //enemy.GetChild(1).GetComponent<EnemyManager>().enabled = false;
            }

            if (transform.childCount == 1)
            {
                enemy.transform.GetChild(1).GetComponent<EnemyManager>().StopAttacking();
                GameUIManager.instance.isGameOver = true;
                GameUIManager.instance.ExecuteGameOver();
                gameObject.SetActive(false);
                return;
            }
        }
        else
        {
            MoveThePlayer();
        }
        //FormatStickMan();

        if (gameState)
        {
            for (int i = 0; i < roads.Length; i++)
            {
                roads[i].Translate(-roads[i].forward * Time.deltaTime * roadSpeed);
                if (roads[i].transform.position.z < -25f)
                {
                    roads[i].transform.position = new Vector3(roads[i].transform.position.x, roads[i].transform.position.y, 35f);
                    roads[i].GetChild(3).GetChild(0).GetComponent<BoxCollider>().enabled = true;
                    roads[i].GetChild(3).GetChild(1).GetComponent<BoxCollider>().enabled = true;
                    roads[i].GetChild(3).GetChild(0).GetComponent<GateManager>().Refresh();
                    roads[i].GetChild(3).GetChild(1).GetComponent<GateManager>().Refresh();
                    roads[i].GetChild(4).gameObject.SetActive(true);
                    roads[i].GetChild(4).GetChild(1).GetComponent<EnemyManager>().Refresh();
                }
            }


            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Animator>().SetBool("Run", true);
            }
        }

    }

    void MoveThePlayer()
    {
        //if (gameState)
        //{
        //    Plane plane = new Plane(Vector3.up, 0f);

        //    Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        //    if (plane.Raycast(ray, out float distance))
        //    {
        //        mouseStartPos = ray.GetPoint(distance + 1f);
        //        playerStartPos = transform.position;
        //    }
        //}

        Plane plane = new Plane(Vector3.up, 0f);
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 mousePos = ray.GetPoint(distance + 1f);

            Vector3 move = mousePos - mouseStartPos;

            Vector3 control = playerStartPos + move;

            if (stickmansCount > 50)
            {
                control.x = Mathf.Clamp(control.x, -3f, 3f);
            }
            else
            {
                control.x = Mathf.Clamp(control.x, -3.65f, 3.65f);
            }


            transform.position = new Vector3
                (
                    Mathf.Lerp
                    (
                        transform.position.x,
                        control.x,
                        Time.deltaTime * playerSpeed
                    ),
                    transform.position.y,
                    transform.position.z
                );
            //arrow.position = new Vector3
            //    (
            //        Mathf.Lerp
            //        (
            //            arrow.position.x,
            //            control.x,
            //            Time.deltaTime * playerSpeed
            //        ),
            //        arrow.position.y,
            //        arrow.position.z
            //    );
        }


    }

    public void FormatStickMan()
    {
        if (player.childCount - 1 < 150 && stickmansCount >= 150 && stickmansCount != player.childCount - 1)
        {
            for (int i = player.childCount - 1; i < 150; i++)
            {
                Instantiate(stickman, transform.position, Quaternion.identity, transform);
            }
        }
        for (int i = 1; i < player.childCount; i++)
        {
            float x = distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * radius);
            float y = distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * radius);

            Vector3 NewPos = new Vector3(x, -0.5066667f, y);

            player.transform.GetChild(i).DOLocalMove(NewPos, 0.5f).SetEase(Ease.OutBack);
        }
        CounterText.text = stickmansCount.ToString();
    }

    private void MakeStickman(int num)
    {
        for (int i = player.childCount - 1; i < num; i++)
        {
            if (player.childCount - 1 < 150)
            {
                Instantiate(stickman, transform.position, Quaternion.identity, transform);
            }
        }

        stickmansCount = num;

        CounterText.text = stickmansCount.ToString();

        FormatStickMan();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            other.transform.parent.GetChild(0).GetComponent<BoxCollider>().enabled = false;
            other.transform.parent.GetChild(1).GetComponent<BoxCollider>().enabled = false;

            GateManager gateManager = other.GetComponent<GateManager>();

            if (gateManager.multiply)
            {
                MakeStickman(stickmansCount * gateManager.randomNumber);
            }
            else
            {
                MakeStickman(stickmansCount + gateManager.randomNumber);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            enemy = other.transform;
            attack = true;

            roadSpeed = 1f;

            other.transform.GetChild(1).GetComponent<EnemyManager>().AttackPlayer(transform);

            StartCoroutine(UpdateTheEnemyAndPlayerStickMansNumbers());
        }
    }

    IEnumerator UpdateTheEnemyAndPlayerStickMansNumbers()
    {
        EnemyCount = enemy.transform.GetChild(1).childCount;
        int stickmans = stickmansCount;
        int index = stickmans - EnemyCount;

        while (stickmans > 0 && EnemyCount > 0)
        {
            EnemyCount--;
            stickmans--;

            enemy.transform.GetChild(1).GetComponent<EnemyManager>().CounterText.text = EnemyCount.ToString();
            CounterText.text = stickmans.ToString();

            yield return null;
        }

        if (EnemyCount == 0)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation = Quaternion.identity;
            }
        }
        if (index > 0)
        {
            stickmansCount = index;
        }
    }
}
