using System.Collections;
using UnityEngine;

public class BattleProjectile : MonoBehaviour
{
    public IEnumerator MoveTo(Transform target, float speed)
    {
        Vector3 start = transform.position;
        float time = 0;

        while (target && Vector3.Distance(transform.position, target.position) > 0.05f)
        {
            time += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            yield return null;
        }
    }
}
