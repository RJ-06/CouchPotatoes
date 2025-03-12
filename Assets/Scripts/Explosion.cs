using NUnit.Framework;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    bool doExplode = false;

    [SerializeField] float startScaleSize;
    float scale;
    [SerializeField] float endScaleSize;
    [SerializeField] float timeForExplosion;
    float timer = 0f;
    [SerializeField] int damageDeal;

    //[SerializeField] FXManager fx;

    System.Collections.Generic.List<PlayerVals> playerList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ResetAndExplode();
    }

    public void ResetAndExplode()
    {

        gameObject.SetActive(true);
        transform.localScale = new Vector2(startScaleSize, startScaleSize);
        //fx.effects[0].Invoke();
        //fx.playParticle("PotatoExplode");
        timer = 0;
        doExplode = true;
        playerList.Clear();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!doExplode) return;

        scale = transform.localScale.x;
        timer += Time.fixedDeltaTime;
        while (timer < timeForExplosion) 
        {
            Mathf.Lerp(scale, endScaleSize, Time.fixedDeltaTime);
        }
        transform.localScale = new Vector2(scale, scale);
        if (scale >= endScaleSize) 
        {
            transform.localScale = new Vector2(startScaleSize, startScaleSize);
            gameObject.SetActive(false);
            doExplode = false;

        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player") || col.gameObject == transform.parent) return;

        var target = col.GetComponent<PlayerVals>();
        if (playerList.Contains(target)) return; // Prevent duplicate triggers
        playerList.Add(target);
        target.IncrementHealth(damageDeal);
    }
}
