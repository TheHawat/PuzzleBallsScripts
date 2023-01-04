using System;
using UnityEngine;
using Random = UnityEngine.Random;

// RequireComponent is probably cool
// https://docs.unity3d.com/ScriptReference/RequireComponent.html
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BallBehaviour : MonoBehaviour
{
    public Rigidbody2D Rb2d;
    public IAudioManager Speaker;
    public bool IsOnTop = false;
    public bool IsLaunched = false;
    public event Action<Vector2> OnCeilingHit;
    private Vector2 _startingVelocity;
    private int _bounceCount = 0;
    public int Index;
    public void Start() {
        InitaliseBalls();
    }

    private void InitaliseBalls() {
        Rb2d = GetComponent<Rigidbody2D>();
        Rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Bounce() {
        Speaker.PlayTheSoundOf("Bounce");
        _bounceCount++;
        float _multiplier = (_bounceCount % 2 == 1) ? -1.0f : 1.0f;
        Rb2d.velocity = new Vector2(_startingVelocity.x * _multiplier, _startingVelocity.y);
    }
    public Vector2 Velocity() {
        return Rb2d.velocity;
    }
    public Vector2 Position() {
        return Rb2d.position;
    }
    public void OnCollisionEnter2D(Collision2D collision) {
        if (!(IsLaunched && !IsOnTop)) return;
        var Collider = collision.collider;
        if (Collider.CompareTag("Sticky")) OnCeilingHit?.Invoke(Position());
        else if (Collider.CompareTag("Bouncy")) Bounce();
    }
    public void Launch(Vector2 velocity) {
        Speaker.PlayTheSoundOf("Shoot");
        Rb2d.velocity = velocity;
        _startingVelocity = velocity;
        Rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        IsLaunched = true;
    }
    public void Freeze(Vector2 position) {
        Speaker.PlayTheSoundOf("Ceiling");
        Rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.position = position;
        IsOnTop = true;
    }
    public void Drop() {
        Rb2d.gravityScale = 1.3f;
        Rb2d.constraints = RigidbodyConstraints2D.None;
        CircleCollider2D BallCollider = GetComponent<CircleCollider2D>();
        BallCollider.enabled = false;
        Vector2 RandomV = new(Random.Range(-7, 7), Random.Range(2, 8));
        Rb2d.velocity = RandomV;
    }
    public void Reposition(Vector2 position) {
        transform.position = position;
    }
    public GameObject UnderlyingObject() {
        return gameObject;
    }
}
