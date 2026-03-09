using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerWeaponsManager : MonoBehaviour
{
    [Header("Hand")]
    public Image handImage;
    public Sprite idleSprite;
    public Sprite attackSprite;

    [Header("Timing")]
    public float attackImageDuration = 0.1f;
    public float attackCooldown = 0.3f;

    [Header("Idle Motion")]
    public float idleBobAmount = 5f;
    public float idleBobSpeed = 2f;

    [Header("Attack Ray")]
    public float attackDistance = 3f;
    public int damage = 1;
    public LayerMask enemyMask;

    bool canAttack = true;
    bool isAttacking = false;

    RectTransform handRect;
    Vector2 basePos;

    void Start()
    {
        handRect = handImage.rectTransform;
        basePos = handRect.anchoredPosition;
    }

    void Update()
    {
        if (!isAttacking)
        {
            IdleMotion();
        }

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            StartCoroutine(Punch());
        }
    }

    void IdleMotion()
    {
        float offset = Mathf.Sin(Time.time * idleBobSpeed) * idleBobAmount;
        handRect.anchoredPosition = basePos + new Vector2(0f, offset);
    }

    IEnumerator Punch()
    {
        canAttack = false;
        isAttacking = true;

        handRect.anchoredPosition = basePos;

        handImage.sprite = attackSprite;

        HitEnemy();

        yield return new WaitForSeconds(attackImageDuration);

        handImage.sprite = idleSprite;

        isAttacking = false;

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    void HitEnemy()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance, enemyMask))
        {
            EnemyMelee enemy = hit.collider.GetComponentInParent<EnemyMelee>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}