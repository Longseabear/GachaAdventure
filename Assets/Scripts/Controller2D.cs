using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

    public LayerMask collisionMask;

    const float skinWidth = 0.015f; // skin
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D collider;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;

    void Start() {
        collider = GetComponent<BoxCollider2D>();
        CaculateRaySpacing();
    }

    public void Move(Vector3 velocity) {
        UpdateRaycastOrigins();
        collisions.Reset();

        // Check Collisions
        if (velocity.x != 0)
            HorizontalCollisions(ref velocity);
        if (velocity.y != 0)
            VerticalCollisions(ref velocity);

        //Moves the transform in the direction and distance of translation.
        transform.Translate(velocity);
    }
    void HorizontalCollisions(ref Vector3 velocity) {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth; // Next position pred

        // 충돌하였을 때 player의 velocity를 초기화 할 거 같은데?
        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (i * horizontalRaySpacing); // 와 시발 지림

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            if (hit) {
                velocity.x = (hit.distance - skinWidth) * directionX; // Hit한 거리만큼만 이동. ok
                rayLength = hit.distance; // it's getting smaller. 

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }
    void VerticalCollisions(ref Vector3 velocity) {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth; // Next position pred

        // 충돌하였을 때 player의 velocity를 초기화 할 거 같은데?
        for (int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (i * verticalRaySpacing +
                velocity.x); // 와 시발 지림

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit) {
                velocity.y = (hit.distance - skinWidth) * directionY; // Hit한 거리만큼만 이동. ok
                rayLength = hit.distance; // it's getting smaller. 

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }
    void UpdateRaycastOrigins() {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2); // minus니까 두배 줄임

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CaculateRaySpacing() {
        // Caclulate Ray Spacing -> 분할비율 설정
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2); // minus니까 두배 줄임

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    struct RaycastOrigins {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;

        public void Reset() {
            above = below = false;

            left = right = false;
        }
    }
}
