using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public enum AbilityState { None, Sword, Shield, Gun, Wand }
    public AbilityState currentAbility = AbilityState.None;
    public int indexActiveAbility = 0; // 0 - 4
    public GameObject sword;
    public GameObject shield;
    public GameObject gun;
    public GameObject wand;
    //
    public GameObject handInstWalls;
    public float swingAngle = 90f;
    public float swingDuration = 0.3f;
    public GameObject wallPrefab;
    public Transform wallInstPos;
    public float wallCooldownTime; private float wallInCooldownTimer;
    public LayerMask wallCanNotInst;
    private Vector3 wallSize;
    private Vector3 wallCenter;
    //
    public bool gizmosOn;
    public Color gizmosColor;
    //

    void Start()
    {
        
    }

    private void SetActiveAbility()
    {
        switch (indexActiveAbility)
        {
            case 0: ActivateAbility(AbilityState.None); break;
            case 1: ActivateAbility(AbilityState.Sword); break;
            case 2: ActivateAbility(AbilityState.Shield); break;
            case 3: ActivateAbility(AbilityState.Gun); break;
            case 4: ActivateAbility(AbilityState.Wand); break;
            default: Debug.LogError("indexToSetActive (Ability)"); break;
        }
    }
    private void ActivateAbility(AbilityState ability)
    {
        currentAbility = ability;

        sword.SetActive(ability == AbilityState.Sword);
        shield.SetActive(ability == AbilityState.Shield);
        gun.SetActive(ability == AbilityState.Gun);
        wand.SetActive(ability == AbilityState.Wand);
    }
    private void AbilityChangeNumberWithSound(int num)
    {
        indexActiveAbility = num;

        if (indexActiveAbility > 4)
            indexActiveAbility = 0;
        else if (indexActiveAbility < 0)
            indexActiveAbility = 4;

        AudioManager.Instance.PlayPlayerChangeAbility();
    }
    private void InputLogic()
    {
        // Ability Change:
        if (Input.GetKeyDown(KeyCode.Alpha0) && indexActiveAbility != 0)
            AbilityChangeNumberWithSound(0);
        else if (Input.GetKeyDown(KeyCode.Alpha1) && indexActiveAbility != 1)
            AbilityChangeNumberWithSound(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2) && indexActiveAbility != 2)
            AbilityChangeNumberWithSound(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3) && indexActiveAbility != 3)
            AbilityChangeNumberWithSound(3);
        else if (Input.GetKeyDown(KeyCode.Alpha4) && indexActiveAbility != 4)
            AbilityChangeNumberWithSound(4);

        if (Input.GetKeyDown(KeyCode.E))
            AbilityChangeNumberWithSound(indexActiveAbility + 1);
        else if (Input.GetKeyDown(KeyCode.Q))
            AbilityChangeNumberWithSound(indexActiveAbility - 1);


        SetActiveAbility();
        // Ability Change;

        // Active The Ability:
        if (Input.GetKeyDown(KeyCode.Mouse1) && currentAbility != AbilityState.None)
            BasicWallsAbility();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            switch (currentAbility)
            {
                case AbilityState.Sword:
                    sword.GetComponent<Sword>().TriggerSword();
                    break;
                case AbilityState.Shield:
                    shield.GetComponent<Shield>().inputShieldOn = true;
                    break;
                case AbilityState.Gun:
                    gun.GetComponent<Gun>().ShootBullet();
                    break;
                case AbilityState.Wand:
                    wand.GetComponent<Wand>().MakeWall();
                    break;
            }
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            switch (currentAbility)
            {
                case AbilityState.Shield:
                    // if (!Input.GetKey(KeyCode.Mouse0))
                    break;
            }
        }
        else
        {
            switch (currentAbility)
            {
                case AbilityState.Shield:
                    shield.GetComponent<Shield>().inputShieldOn = false;
                    break;
            }
        }
        // Active The Ability;


        // Change in Ability:
        if (Input.GetKeyDown(KeyCode.F))
        {
            switch(currentAbility)
            {
                case AbilityState.Gun:
                    gun.GetComponent<Gun>().NextTypeOfBullet();
                    break;
                case AbilityState.Wand:
                    wand.GetComponent<Wand>().NextTypeOfWall();
                    break;
            }
        }
        // Change in Ability;
    }

    void Update()
    {
        InputLogic();

        BasicWallsAbilityColldown();
    }


    /// Basic Wall Abilitiy:
    private void BasicWallsAbility()
    {
        UpdateWallParameters();
        if (CanSpawnBox(wallCenter, wallSize, wallCanNotInst) && wallInCooldownTimer <= 0)
        {
            handInstWalls.GetComponent<SwingComponent>().TriggerSwing(swingAngle, swingDuration, Vector3.right);

            Instantiate(wallPrefab, wallInstPos.position, wallInstPos.rotation);

            wallInCooldownTimer = wallCooldownTime;
            // Sound:
            AudioManager.Instance.PlayPlayerInstWallSimple();
        }
    }
    public bool CanSpawnBox(Vector3 center, Vector3 size, LayerMask mask)
    {
        Vector3 halfExtents = size * 0.5f; // Unity logic
        return !Physics.CheckBox(center, halfExtents, wallInstPos.rotation, mask);
    }
    private void UpdateWallParameters()
    {
        BoxCollider boxCol = wallPrefab.GetComponentInChildren<BoxCollider>();
        wallSize = Vector3.Scale(boxCol.size, boxCol.transform.lossyScale);
        wallCenter = wallInstPos.position;// + Vector3.Scale(boxCol.center, wallPrefab.transform.lossyScale);
    }
    private void BasicWallsAbilityColldown()
    {
        if (wallInCooldownTimer > 0)
        {
            wallInCooldownTimer -= Time.deltaTime;
        }
    }
    /// Basic Wall Abilitiy;

    void OnDrawGizmos()
    {
        if (gizmosOn)
        {
            /// ליצירת התיבה של הקירות:
            UpdateWallParameters();

            Gizmos.color = gizmosColor;

            Matrix4x4 rotationMatrix = Matrix4x4.TRS(wallCenter, wallInstPos.rotation, Vector3.one);
            Gizmos.matrix = rotationMatrix;

            Gizmos.DrawWireCube(Vector3.zero, wallSize);
            /// ליצירת התיבה של הקירות;
        }
    }

}