using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    This script references Workshop5-Solution.
    Url:https://github.com/COMP30019/Workshop-5-Solution
*/


public class EnemyManager : MonoBehaviour
{
    private MeshRenderer _renderer;
    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    [SerializeField] private float aimTime;
    [SerializeField] private GameObject EnemyProjectilePrefab;
    [SerializeField] private GameObject PlayerProjectilePrefab;
    [SerializeField] private ParticleSystemRenderer bloodEffect;
    [SerializeField] private ParticleSystemRenderer bloodSplatEffect;
    [SerializeField] private ParticleSystemRenderer gluttonyAbilityEffect;
    [SerializeField] private ParticleSystemRenderer greedAbilityEffect;
    [SerializeField] private float visionRange = 15f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private AudioClip enemyGetHitSound;

    private PlayerCharacter _playerCharacter;
    private Vector3 _directionOfAttack;
    private Color _baseColor;
    public GameObject MoneyDrop;
    private Coroutine _attackCoroutine = null;

    private NavMeshAgent _agent;
    private Rigidbody _rigidbody;

    public GameObject BossDrop;
    private Weapon weaponScript;


    List<GameObject> playerTeam = new List<GameObject>();
    List<GameObject> enemyTeam = new List<GameObject>();
    private enum EnemyState
    {
        Idle,
        Attacking,
        Bribed
    }
    private EnemyState _state;

    private void Awake()
    {
        this._playerCharacter = FindObjectOfType<PlayerCharacter>();
        this._renderer = gameObject.GetComponentInChildren<MeshRenderer>();
        this._state = EnemyState.Attacking;
        this._baseColor = this._renderer.material.color;
        this._agent = GetComponent<NavMeshAgent>();
        this._rigidbody = GetComponent<Rigidbody>();
        weaponScript = GetComponentInChildren<Weapon>();

    }

    // Update is called once per frame
    // public void UpdateHealthColor(float healthPercentage)
    // {
    //     this._renderer.material.color = this._baseColor * healthPercentage;
    // }

    private void FixedUpdate()
    {
        if (this._playerCharacter == null)
        {
            return;
        }
        if (this._state == EnemyState.Attacking && this._playerCharacter.IsAlive)
        {
            HandleAttackState();
        }
        else if (this._state == EnemyState.Bribed)
        {
            HandleBribedState();
        }
    }

    private void UpdateDirectionTowardsTarget()
    {
        GameObject target = null;

        if (this.tag == "Enemy" || this.tag == "Boss")
        {
            target = GetClosestGameObject(playerTeam);
        }
        else if (this.tag == "BribedEnemy")
        {
            target = GetClosestGameObject(enemyTeam, exclude: gameObject);
        }

        if (target != null)
        {
            UpdateDirectionAndRotation(target.transform.position);
        }
    }

    private void HandleAttackState()
    {
        if (playerTeam.Count == 0)
        {
            return;
        }
        else if (playerTeam.Count == 1 && _playerCharacter != null)
        {
            if (Vector3.Distance(this.transform.position, _playerCharacter.transform.position) <= visionRange)
            {
                // UpdateDirectionAndRotation(_playerCharacter.transform.position);
                if (_attackCoroutine == null)
                {
                    _attackCoroutine = StartCoroutine(AttackSequence());
                    _agent.updateRotation = false;
                }
            }
            else if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _agent.updateRotation = true;
                _attackCoroutine = null;
            }
            return;
        }

        GameObject target = GetClosestGameObject(playerTeam);
        if (target != null && Vector3.Distance(this.transform.position, target.transform.position) <= visionRange)
        {
            // UpdateDirectionAndRotation(target.transform.position);
            if (_attackCoroutine == null)
            {
                _attackCoroutine = StartCoroutine(AttackSequence());
                _agent.updateRotation = false;
            }
        }
        else if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _agent.updateRotation = true;
            _attackCoroutine = null;
        }
    }

    private void HandleBribedState()
    {
        if (enemyTeam.Count == 0)
        {
            return;
        }

        GameObject target = GetClosestGameObject(enemyTeam, exclude: gameObject);
        if (target != null && Vector3.Distance(this.transform.position, target.transform.position) <= visionRange)
        {
            // UpdateDirectionAndRotation(target.transform.position);
            if (_attackCoroutine == null)
            {
                _attackCoroutine = StartCoroutine(AttackSequence());
                _agent.updateRotation = false;
            }
        }
        else if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _agent.updateRotation = true;
            _attackCoroutine = null;
        }
    }



    private GameObject GetClosestGameObject(List<GameObject> objects, GameObject exclude = null)
    {
        GameObject closestObject = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject obj in objects)
        {
            if (obj != null && obj != exclude)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = obj;
                }
            }
        }

        return closestObject;
    }


    private void UpdateDirectionAndRotation(Vector3 targetPosition)
    {
        this._directionOfAttack = (targetPosition - this.transform.position).normalized;
        Quaternion desiredRotation = Quaternion.LookRotation(this._directionOfAttack);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }

    private void UpdatePlayers()
    {
        playerTeam.Clear();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTeam.Add(player);
        }
        playerTeam.AddRange(GameObject.FindGameObjectsWithTag("BribedEnemy"));
    }

    private void UpdateEnemies()
    {
        enemyTeam.Clear();
        enemyTeam.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    private IEnumerator AttackSequence()
    {
        while (this._playerCharacter.IsAlive)
        {
            yield return StartCoroutine(IdleAndFaceTarget());
            if (this.tag == "Enemy" || this.tag == "Boss")
            {
                yield return StartCoroutine(AttackAndFaceTarget());
            }
            else if (this.tag == "BribedEnemy")
            {
                yield return StartCoroutine(BribedAttackAndFaceTarget());
            }
        }
    }

    private IEnumerator IdleAndFaceTarget()
    {
        float idleDuration = Random.Range(this.minIdleTime, this.maxIdleTime);
        float elapsed = 0;
        this._state = EnemyState.Idle;

        while (elapsed < idleDuration)
        {
            UpdateDirectionTowardsTarget();
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator AttackAndFaceTarget()
    {
        this._state = EnemyState.Attacking;
        UpdateDirectionTowardsTarget();
        yield return new WaitForSeconds(this.aimTime);
        yield return Fire();
    }

    private IEnumerator BribedAttackAndFaceTarget()
    {
        this._state = EnemyState.Bribed;
        UpdateDirectionTowardsTarget();
        yield return new WaitForSeconds(this.aimTime);
        yield return Fire();
    }
    public void Bribe()
    {
        this.tag = "BribedEnemy";
        this._renderer.material.color = Color.green;
        //Debug.Log(gameObject.name + " has been bribed and is in state: " + this._state);
        GlobalEnemyEvents.Instance.UpdatePlayerList();
        GlobalEnemyEvents.Instance.UpdateEnemyList();
    }

    private IEnumerator Fire()
    {
        // Quaternion rotation = Quaternion.LookRotation(_directionOfAttack);
        Vector3 shootPosition = new Vector3(transform.position.x, 1, transform.position.z);
        if (this._state == EnemyState.Bribed)
        {
            weaponScript.bullet = weaponScript.bribedBullet;
            // var projectile = Instantiate(PlayerProjectilePrefab, shootPosition, transform.rotation);
            weaponScript.Fire(_directionOfAttack);

            //Debug.Log("Firing at direction: " + _directionOfAttack);
        }
        else
        {
            // var projectile = Instantiate(EnemyProjectilePrefab, shootPosition, transform.rotation);
            weaponScript.Fire(_directionOfAttack);
        }

        // // Disable NavMeshAgent
        // _agent.enabled = false;

        // _rigidbody.AddForce(-this._directionOfAttack * 2, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f); // Small delay after fire.

        // // Enable NavMeshAgent
        // _agent.enabled = true;
    }

    private void OnEnable()
    {
        GlobalEnemyEvents.Instance.OnPlayerListUpdate += UpdatePlayers;
        GlobalEnemyEvents.Instance.OnEnemyListUpdate += UpdateEnemies;
    }

    private void OnDisable()
    {
        GlobalEnemyEvents.Instance.OnPlayerListUpdate -= UpdatePlayers;
        GlobalEnemyEvents.Instance.OnEnemyListUpdate -= UpdateEnemies;
    }

    public void Kill()
    {

        Vector3 effectPosition = new Vector3(transform.position.x, 1, transform.position.z);
        var particles = Instantiate(this.bloodEffect, effectPosition, Quaternion.identity);
        // particles.transform.position = transform.position + Vector3.up;
        // particles.material.color = this._baseColor;
        Vector3 MoneyPosition = new Vector3(transform.position.x, 0.5f, transform.position.z);
        Instantiate(MoneyDrop, MoneyPosition, Quaternion.identity);
        if (this.tag == "Boss")
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 90);
            GameObject BossWeapon = Instantiate(BossDrop, MoneyPosition, rotation);
            BossWeapon.tag = "DroppedWeapon";
        }
    }

    public void TakeDamageEffect()
    {
        if (enemyGetHitSound != null)
        {
            AudioSource.PlayClipAtPoint(enemyGetHitSound, transform.position, 0.5f);
        }
        Vector3 effectPosition = new Vector3(transform.position.x, 1, transform.position.z);
        var particles = Instantiate(this.bloodSplatEffect, effectPosition, transform.rotation);
    }

    public void TakeGluttonyAbilityEffect()
    {
        Vector3 effectPosition = new Vector3(transform.position.x, 1, transform.position.z);
        var particles = Instantiate(this.gluttonyAbilityEffect, effectPosition, transform.rotation);
    }

    public void TakeGreedAbilityEffect()
    {
        Vector3 effectPosition = new Vector3(transform.position.x, 0.5f, transform.position.z);
        var particles = Instantiate(this.greedAbilityEffect, effectPosition, Quaternion.identity);
    }
}
