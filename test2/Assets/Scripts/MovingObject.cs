using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour
{

    public float movetime = 0.1f;
    public LayerMask blockinglayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inversemovetime;


    // Use this for initialization
   	protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inversemovetime = 1f / movetime;
    }

    protected bool move(int xdir, int ydir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xdir, ydir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockinglayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrremainingdistance = (transform.position - end).sqrMagnitude;

        while (sqrremainingdistance > float.Epsilon)
        {
            Vector3 newposition = Vector3.MoveTowards(rb2D.position, end, inversemovetime * Time.deltaTime);
            rb2D.MovePosition(newposition);
            sqrremainingdistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected virtual void attemptmove<T>(int xdir, int ydir)
    where T : Component
    {
        RaycastHit2D hit;
        bool canmove = move(xdir, ydir, out hit);
        if (hit.transform == null)
            return;

        T hitcomponent = hit.transform.GetComponent<T>();
        if (!canmove && hitcomponent != null)
            oncantmove(hitcomponent);
    }

    protected abstract void oncantmove<T>(T component)
        where T : Component;
}
