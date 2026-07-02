using System.Collections;
using UnityEngine;

public class BattleActor : MonoBehaviour
{
    public BattleUnit unit;
    public bool isTargeted;
    public RenderableController renderControl;
    public Transform projectileSpawn;

    public Vector3 OriginPosition { get; private set; }
    public Transform MenuAnchor;
    [HideInInspector] public BattleActorUI statsUI;
    [HideInInspector] public BattleQueueUIController queuePortraitUI;

    public void Setup(BattleUnit _unit)
    {
        unit = _unit;

        OriginPosition = transform.position;

        isTargeted = false;
        renderControl = GetComponentInChildren<RenderableController>();

        if (renderControl != null)
        {
            renderControl.SetFacingOrientation(unit.isEnemy ? -1 : 1);
            renderControl.spriteRenderer.sprite = unit.charData.idleSprite;
            renderControl.animator.runtimeAnimatorController = unit.charData.animatorController;
        }
    }
       
    public void SetTargeted(bool isTargeted)
    {
        this.isTargeted = isTargeted;
        renderControl.SetHighlight(isTargeted ? Color.yellow : Color.white);
    }

    public IEnumerator MoveTo(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, time / duration);

            yield return null;
        }

        transform.position = target;
    }

    public IEnumerator Return()
    {
        yield return MoveTo(OriginPosition, 0.2f);
    }

    public IEnumerator Flash(Color color)
    {
        renderControl.SetHighlight(color);

        yield return new WaitForSeconds(0.1f);

        renderControl.SetHighlight(Color.white);
    }

    public IEnumerator ShootProjectile(CharacterData attackerData, BattleActor target)
    {
        GameObject projectileObject = Instantiate(attackerData.basicAttackProjectile, projectileSpawn.position, Quaternion.identity);

        BattleProjectile projectile = projectileObject.GetComponent<BattleProjectile>();

        yield return projectile.MoveTo(target.transform, attackerData.projectileSpeed);

        Destroy(projectileObject);
    }
}
