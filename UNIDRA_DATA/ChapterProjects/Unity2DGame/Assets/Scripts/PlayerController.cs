using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveStartDistance = 10f;
    public float moveForce = 150f;
    public float maxSpeed = 100f;
    float targetPointX;
    bool facingRight = true;

    void Start()
    {
        Vector3 screen_point = Camera.main.WorldToScreenPoint(transform.position);
        targetPointX = screen_point.x;
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;
        targetPointX = Input.mousePosition.x;
    }

    void FixedUpdate()
    {
        // 3D座標をスクリーン座標に変換
        Vector3 screen_point = Camera.main.WorldToScreenPoint(transform.position);

        // 移動先までの距離が一定以下ならば移動処理をしません
        if (Mathf.Abs(targetPointX - screen_point.x) <= moveStartDistance)
            return;

        // 移動先が右か左かを計算し、その方向に移動する力を加えます
        float horizontal = Mathf.Sign(targetPointX - screen_point.x);
        rigidbody2D.AddForce(Vector2.right * horizontal * moveForce);

        // 移動速度を制限
        if (Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
            rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

        // プレイヤーの向きを調整
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 local_scale = transform.localScale;
            local_scale.x *= -1;
            transform.localScale = local_scale;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fire")
        {
            Animator myAnimator = GetComponent<Animator>();
            myAnimator.SetTrigger("Damage");
        }
    }
}