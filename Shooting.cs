using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MyMonoBehaviour
{
    public string editorName; // Editor only!

    [Header("Shooting")]
    public GameObject bullet;
    public Transform shootingPoint;
    public List<Collider> collidersToIgnore;
    public float timeToShoot = 2;
    public float timerToShoot;
    [Space(10)]
    public bool shootOnlyIfCanSeeTraget = true;
    private Vector3 targetPosition;
    public float detectRadius;
    public LayerMask detectLayers;
    private float detectDistance;
    private Vector3 detectDirection;

    [Header("Add Ones:")]
    public float bulletSpeed = 0; // 0 == null
    public float bulletSpeedAdd = 0; // 0 == null
    public float bulletSpeedMultiply = 0; // 0 == null
    public int bulletDamage = 0; // 0 == null
    public int bulletDamageAdd = 0; // 0 == null
    public float bulletDamageMultiply = 0; // 0 == null

    [Header("Gzimos:")]
    public bool showGizmos = false;
    public Color color = Color.red;
    public int segments = 10;

    void OnEnable()
    {
        timerToShoot = timeToShoot;
    }
    private void Start()
    {
        detectRadius = bullet.transform.lossyScale.y * bullet.GetComponent<SphereCollider>().radius;
    }

    void Update()
    {
        ShootingTimerUpdate();
    }

    public void ShootingTimerUpdate()
    {
        if (timerToShoot <= 0)
        {
            timerToShoot = timeToShoot;
            ShootBullet();
        }
        timerToShoot -= Time.deltaTime;
    }
    public void ShootBullet()
    {
        targetPosition = FindAnyObjectByType<PlayerGeneral>().centerToShootAt.position;

        detectDistance = Vector3.Distance(shootingPoint.position, targetPosition);
        detectDirection = (targetPosition - shootingPoint.position).normalized;

        if (!shootOnlyIfCanSeeTraget || (shootOnlyIfCanSeeTraget && !IsObjectsAhead(shootingPoint.position, detectRadius, detectDirection, detectDistance, detectLayers)))
        {
            GameObject tmpBullet = Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);

            TransformValuseToBullet(tmpBullet.GetComponent<Bullet>());

            Destroy(tmpBullet, 30f);
        }
    }
    private void TransformValuseToBullet(Bullet bullet)
    {
        bullet.collidersToIgnore = collidersToIgnore;

        if (bulletSpeed > 0)
            bullet.speed = bulletSpeed;
        if (bulletSpeedAdd != 0)
            bullet.speed += bulletSpeedAdd;
        if (bulletSpeedMultiply > 0)
            bullet.speed *= bulletSpeedMultiply;
        if (bullet.speed <= 0)
            Debug.Log(gameObject.name + ": create a bullet with 0 or less speed, correct this.");

        if (bulletDamage > 0)
            bullet.damagePoints = bulletDamage;
        if (bulletDamageAdd != 0)
            bullet.damagePoints += bulletDamageAdd;
        if (bulletDamageMultiply != 0)
            bullet.damagePoints = Mathf.RoundToInt(bullet.damagePoints * bulletDamageMultiply);
        if (bullet.damagePoints <= 0)
            Debug.Log(gameObject.name + ": create a bullet with 0 or less damage points, correct this.");
    }

    private void OnDrawGizmos()
    {
        if (shootingPoint == null || !this.enabled)
            return;

        targetPosition = FindAnyObjectByType<PlayerGeneral>().centerToShootAt.position;

        detectDistance = Vector3.Distance(shootingPoint.position, targetPosition);
        detectDirection = (targetPosition - shootingPoint.position).normalized;

        Gizmos.color = color;
        DrawSphereCastGizmo(shootingPoint.position, detectDirection, detectRadius, detectDistance);

        // draw hit:
        if (Physics.SphereCast(shootingPoint.position, detectRadius, detectDirection, out RaycastHit hit, detectDistance, detectLayers))
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(hit.point, 0.2f);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(shootingPoint.position, hit.point);
        }
    }
    void DrawSphereCastGizmo(Vector3 origin, Vector3 direction, float radius, float distance)
    {
        for (int i = 0; i <= segments; i++)
        {
            float step = distance * i / segments;
            Vector3 pos = origin + direction * step;
            Gizmos.DrawWireSphere(pos, radius);
        }
    }

}
