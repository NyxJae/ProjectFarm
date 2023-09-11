using UnityEngine;
using QFramework;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace ObjectFarm
{
	public partial class Square : ViewController
	{
		void Start()
		{
			// Code Here
		}
		#region 角色移动相关
		/// <summary>
		/// 角色移动速度
		/// </summary>
		[Tooltip("角色移动速度")]
		public float moveSpeed = 5f;
		/// <summary>
		/// 角色移动方向
		/// </summary>
		private Vector2 moveDirection;
		#endregion

		#region 角色互动相关
		/// <summary>
		/// 鼠标的世界坐标
		/// </summary>
		private Vector2 mouseWorldPosition = Vector2.zero;
		/// <summary>
		/// 鼠标和角色的距离
		/// </summary>
		[Tooltip("鼠标和角色的距离")]
		public float distanceBetweenMouseAndPlayer = 1f;

		/// <summary>
		/// tilemap对象
		/// </summary>
		[Tooltip("tilemap对象")]
		public Tilemap tilemap = null;


		#endregion

		// 当 FixedMove 动作被触发时，此函数将被调用
		void FixedUpdate()
		{
			// 移动角色
			transform.Translate(moveDirection * moveSpeed * Time.fixedDeltaTime);
		}


		/// <summary>
		/// 当 Move 动作被触发时，此函数将被调用
		/// </summary>
		/// <param name="context"></param>
		public void OnMove(InputAction.CallbackContext context)
		{
			// 读取移动方向
			moveDirection = context.ReadValue<Vector2>();
		}


		/// <summary>
		/// 当 UseTools 动作被触发时(点击鼠标左键时)，此函数将被调用
		/// </summary>
		/// <param name="context"></param>
		public void OnUseTools(InputAction.CallbackContext context)
		{
			// 鼠标按下时，使用工具,松开时，不触发
			if (context.started)
			{
				// 使用工具
				UseTools(transform.position, mouseWorldPosition);
			}
		}
		/// <summary>
		/// 当 GetMousePosition 动作被触发时(鼠标移动时)，此函数将被调用
		/// </summary>
		/// <param name="context"></param>
		public void GetMousePosition(InputAction.CallbackContext context)
		{
			//获取鼠标的坐标,转换为世界坐标
			mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		}

		/// <summary>
		/// 使用工具
		/// </summary>
		/// <param name="playerPosition">角色的位置和</param>
		/// <param name="mousePosition">鼠标的位置</param>
		private void UseTools(Vector2 playerPosition, Vector2 mousePosition)
		{

			// 计算鼠标和角色的距离
			float distance = Vector2.Distance(playerPosition, mousePosition);
			// 如果距离小于 distanceBetweenMouseAndPlayer
			if (distance < distanceBetweenMouseAndPlayer)
			{
				// 将鼠标的位置转换为tilemap的位置
				Vector3Int tilePosition = tilemap.WorldToCell(mousePosition);
				// 获取tilemap上的tile
				TileBase tile = tilemap.GetTile(tilePosition);
				Debug.Log(tile.name);
				// 如果tile不为空
				if (tile != null)
				{
					// 将tilemap上的tile移除
					tilemap.SetTile(tilePosition, null);
				}
			}
		}

	}
}
