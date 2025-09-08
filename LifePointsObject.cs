using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePointsObject : MyMonoBehaviour
{
    [Header("Life Points Object:")]
    public GameObject theObjectToDestroy;
    public int startLifePoints = 20;
    public int lifePoints = 20;
    //
    public List<Renderer> renders;
    public float ShowHitTime = 0.1f; public float ShowHitTimer;
    private Color startColor;
    public Color takenDamageColor = Color.red;
    private Color colorAfterHit;

    public void LifePointsObjectStartMethod()
    {
        lifePoints = startLifePoints;
        if (renders.Count == 0)
            renders.Add(GetComponentInChildren<Renderer>());
        startColor = colorAfterHit = renders[0].material.color; // more lists...
    }

    /*public void TakenDamage(int damagePoints)
    {
        lifePoints -= damagePoints;
        if (lifePoints < 0)
            lifePoints = 0;

        if (lifePoints == 0)
        {
            if (theObjectToDestroy == FindAnyObjectByType<PlayerGeneral>().gameObject)
            {
                FindAnyObjectByType<StagesSceneManager>().OnPlayerLoose();
                Debug.LogWarning("GAME OVER!!!");
            }
            else
            {
                if (theObjectToDestroy != null) Destroy(theObjectToDestroy);
                else Destroy(gameObject);
            }

        }

        ShowHitTimer = ShowHitTime; //StartShowHit();
    }*/
    public bool TakenDamage(int damagePoints) // return true - alive
    {
        lifePoints -= damagePoints;
        if (lifePoints < 0)
            lifePoints = 0;

        if (lifePoints == 0)
        {
            if (theObjectToDestroy == FindAnyObjectByType<PlayerGeneral>().gameObject)
            {
                FindAnyObjectByType<StagesSceneManager>().OnPlayerLoose();
                Debug.LogWarning("GAME OVER!!!");
            }
            else
            {
                if (theObjectToDestroy != null) Destroy(theObjectToDestroy);
                else Destroy(gameObject);
            }
            return false;
        }

        ShowHitTimer = ShowHitTime; //StartShowHit();
        return true;
    }


    public void ShowHit()
    {
        if (ShowHitTimer > 0)
        {
            ShowHitTimer -= Time.deltaTime;

            float t = ShowHitTimer / ShowHitTime; // 0 - 1
            if (t > 0.5f) // first half
                ChangeRendersColor(Color.Lerp(takenDamageColor, colorAfterHit, (t - 0.5f) * 2));
            else if (t > 0 && t < 0.5f) // sec half
                ChangeRendersColor(Color.Lerp(colorAfterHit, takenDamageColor, t * 2));
        }
        else
        {
            colorAfterHit = Color.Lerp(takenDamageColor, startColor, Mathf.Clamp01((float)lifePoints / startLifePoints));
            ChangeRendersColor(colorAfterHit);
        }
    }
    private void ChangeRendersColor(Color color)
    {
        foreach (Renderer r in renders)
        {
            r.material.color = color;
        }
    }
}