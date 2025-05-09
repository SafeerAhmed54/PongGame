using DG.Tweening;
using UnityEngine;

public class BoundaryHitScript : MonoBehaviour
{
    [SerializeField] private GameObject title;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            title.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f, 10, 1);
        }
    }
}
