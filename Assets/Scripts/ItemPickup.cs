using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        ExtraBomb,
        SpeedIncrease,
        BlastRadius
    }

    public ItemType type;


    private void OnItemPickup(GameObject player)
    {
        switch (type)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb();
                break;

            case ItemType.BlastRadius:
                player.GetComponent<BombController>().IncreaseBlastRadius();
                break;

            case ItemType.SpeedIncrease:
                player.GetComponent<MovementController>().IncreaseSpeed();
                break;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnItemPickup(collision.gameObject);
        }
    }
}
