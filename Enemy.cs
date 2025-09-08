using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LifePointsObject
{
    /// <summary> *
    /// Enemy 1 - Static position, Shooting, Head follow Player
    /// Enemy 2 - Patrol position, Shooting, Head follow Player
    /// Enemy 3 - Patrol position + towards Player, Shooting, Head follow Player
    /// Enemy 4 - Patrol position, Damage on thouch
    /// Enemy 5 - Patrol position + towards Player, Damage on thouch
    /// Enemy 6 - Inst Enemies 5
    /// </summary>

    private Transform playerTr;

    [Header("Enemy:")]
    public int numberOfModes = 0;
    public int currentMode; // 0=<X=<4
    public float distanceFromPlayer;
    public float[] distancesModes = new float[5];
    public List<MonoBehaviour> mode0;
    public List<MonoBehaviour> mode1;
    public List<MonoBehaviour> mode2;
    public List<MonoBehaviour> mode3;
    public List<MonoBehaviour> mode4;
    public List<MonoBehaviour> mode5;
    private List<MonoBehaviour>[] listModes;

    [Header("Destroy Effects:")]
    public GameObject destroyEffect; // ps

    [Header("Add Ones:")]
    public List<Transform> followPosition;
    

    public void EnemyStartMetod()
    {
        LifePointsObjectStartMethod(); // LifePointsObject

        playerTr = FindAnyObjectByType<PlayerMovement2>().transform;

        if (distancesModes.Length > 5) Debug.LogError("Index out of Arr: distancesModes!");

        listModes = new List<MonoBehaviour>[] { mode0, mode1, mode2, mode3, mode4, mode5 };
        foreach (List<MonoBehaviour> listMono in listModes)
            if (listMono.Count == 0)
                listMono.AddRange(listModes[0]);
    }
    public void EnemyUpdateMethod()
    {
        ShowHit(); // LifePointsObject

        if (CheckClosestDistanceDescending() != currentMode && CheckClosestDistanceDescending() <= numberOfModes)
        {
            currentMode = CheckClosestDistanceDescending();
            SetActiveScriptsByMode();
        }
    }
    public void EnemyLateUpdateMethod()
    { 
        FollowObjects();
    }
    private void FollowObjects()
    {
        foreach (Transform go in followPosition)
            go.position = transform.position;
    }


    private int CheckClosestDistanceDescending()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, playerTr.transform.position);

        int closestDistanceIndex = 0;
        for (int i = 0; i < distancesModes.Length; i++)
            if (distanceFromPlayer < distancesModes[i])
                closestDistanceIndex = i + 1;
        return closestDistanceIndex;
    }   
    private void SetActiveScriptsByMode()
    {
        // turn off all scripts:
        foreach (List<MonoBehaviour> listMono in listModes)
            foreach (MonoBehaviour mono in listMono)
                mono.enabled = false;
        // turn on the scripts by mode:
        foreach (MonoBehaviour mono in listModes[currentMode])
            mono.enabled = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        /// Sword:
        if (other.TryGetComponent(out Sword sword))
        {
            if (sword.gameObject.GetComponent<SwingComponent>().FirstHalfOfSwing())
                if (!TakenDamage(sword.damagePoints))
                    EnemyDestroyed();
        }
        /// Sword;

        /// Bullet:
        if (other.TryGetComponent(out Bullet bullet))
        {
            if (!bullet.IgnoreCollider(GetComponent<Collider>()))
            {
                if (!TakenDamage(bullet.damagePoints))
                    EnemyDestroyed();
                bullet.BulletDestroy();
            }
        }
        /// Bullet;

        /// Enemy: (enemy 3)
        if (other.TryGetComponent(out Enemy enemy))
        {
            if (other.TryGetComponent(out Enemy3 enemy3))
            {
                enemy3.EnemyDestroyed();
                EnemyDestroyed();
            }
            else
            {
                Debug.LogError(gameObject.name + ": has eneterd " + other.name + " trigger");
            }
        }
        /// Enemy;
    }

    public void EnemyDestroyed()
    {
        Instantiate(destroyEffect, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(AudioManager.Instance.enemyDie, transform.position);
        if (gameObject != null) Destroy(gameObject);
    }

}
