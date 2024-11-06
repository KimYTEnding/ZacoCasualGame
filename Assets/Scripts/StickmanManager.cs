using DG.Tweening;
using UnityEngine;

public class StickmanManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem popcorn;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyChar") && other.transform.parent.childCount > 0)
        {
            other.enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            Destroy(other.gameObject);
            Destroy(gameObject);

            Instantiate(popcorn, transform.position, Quaternion.identity);
        }

        switch (other.tag)
        {
            case "EnemyChar":
                if (other.transform.parent.childCount > 0)
                {
                    other.enabled = false;
                    gameObject.GetComponent<Collider>().enabled = false;
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
                break;
            case "Jump":
                transform.DOJump(transform.position, 1f, 1, 1f).SetEase(Ease.Flash).OnComplete(PlayerManager.Instance.FormatStickMan);
                break;

        }
    }
}
