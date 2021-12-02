using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Player attributes")]
    [SerializeField] protected Transform player;
    [SerializeField] protected Health playerHealth;

    protected float simulationSpeed = 1f;

    #region Unity Callbacks
    protected virtual void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        player = p.GetComponent<Transform>();
        playerHealth = p.GetComponent<Health>();
    }

    protected virtual void Update()
    {

    }

    protected void OnDestroy()
    {
        if (EnemyManager.Instance != null)
            EnemyManager.Instance.RemoveFromList(gameObject);
    }

    protected virtual void OnDrawGizmosSelected()
    {

    }
    #endregion

    protected float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    protected Vector3 GetDirectionToPlayer()
    {
        return (player.position - transform.position).normalized;
    }

    protected Quaternion GetLookRotation()
    {
        Vector3 dirToPlayer = GetDirectionToPlayer();
        return Quaternion.LookRotation(new Vector3(dirToPlayer.x, 0, dirToPlayer.z));
    }
}
