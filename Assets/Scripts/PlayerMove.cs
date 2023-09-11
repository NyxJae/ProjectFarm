using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    /// <summary>
    /// 角色移动速度
    /// </summary>
    [Tooltip("角色移动速度")]
    public float moveSpeed = 5f;
    /// <summary>
    /// 角色移动方向
    /// </summary>
    private Vector2 moveDirection;

    // 当 Move 动作被触发时，此函数将被调用
    public void OnMove(InputAction.CallbackContext context)
    {
        // 读取移动方向
        moveDirection = context.ReadValue<Vector2>();
    }
    // 当 FixedMove 动作被触发时，此函数将被调用
    void FixedUpdate()
    {
        // 移动角色
        transform.Translate(moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}
