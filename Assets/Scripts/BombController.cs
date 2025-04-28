using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    public GameObject bombPrefab;
    public KeyCode inpuKey = KeyCode.Space;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombsRemaining;

    [Space,Header("Explosions")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Space, Header("Destructibles")]
    public Tilemap destructibelTiles;
    public Destructible destructiblePrefab;



    private Coroutine bombCoroutine;
    private WaitForSeconds waitForBombFuse;

    [SerializeField] private AudioClip explodeClip;
    [SerializeField] private AudioClip placeBombClip;
    private void Awake()
    {
        waitForBombFuse = new WaitForSeconds(bombFuseTime);
    }

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    private void OnDisable()
    { 
        if(bombCoroutine != null)
            StopCoroutine(bombCoroutine);
    }

    private void Update()
    {
        if(Input.GetKeyDown(inpuKey) && bombsRemaining > 0)
        {
            bombCoroutine = StartCoroutine(PlaceBomb());
        }
    }

    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        
        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        AudioSource.PlayClipAtPoint(placeBombClip,transform.position);

        yield return waitForBombFuse;

        position = bomb.transform.position;

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);
        AudioSource.PlayClipAtPoint(explodeClip, bomb.transform.position);
        Destroy(bomb);
        bombsRemaining++;
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if(length <= 0)
        {
            return;
        }


        position += direction;

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructibelTiles(position);
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
      
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);
        //Call recursively.
        Explode(position, direction, length - 1);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            collision.isTrigger = false;
        }
    }

    private void ClearDestructibelTiles(Vector2 position)
    {
        Vector3Int cell = destructibelTiles.WorldToCell(position);
        TileBase tile = destructibelTiles.GetTile(cell);

        if(tile != null)
        {
           Instantiate(destructiblePrefab, position, Quaternion.identity);
           destructibelTiles.SetTile(cell, null);
        }
    }

    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining = bombAmount;
    }
    public void IncreaseBlastRadius()
    {
        explosionRadius++;
    }
}
