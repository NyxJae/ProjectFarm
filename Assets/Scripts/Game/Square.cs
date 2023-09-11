using UnityEngine;
using QFramework;
using UnityEngine.InputSystem;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace ObjectFarm
{
	public partial class Square : ViewController
	{
		void Start()
		{
			// Code Here
		}
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
}
