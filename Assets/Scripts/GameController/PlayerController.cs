using UnityEngine;
using QFramework;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace ObjectFarm
{
    public partial class PlayerController : ViewController, IController
    {
        #region 角色移动相关字段

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



        #region 角色互动相关字段

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
        /// 地块的数据
        /// </summary>
        private EasyGrid<GridData> grid = null;


        #endregion

        #region 系统相关字段

        /// <summary>
        /// model层
        /// </summary>
        private ObjectFarmModel mModel = null;



        #endregion


        #region 生命周期函数

        // 第一次Update之前调用
        private void Start()
        {
            // 获取model层
            mModel = this.GetModel<ObjectFarmModel>();
            // 获取地块数据
            grid = mModel.Grids.Value;

        }

        /// <summary>
        /// 物理更新
        /// </summary>
        void FixedUpdate()
        {
            // 移动角色
            transform.Translate(moveDirection * moveSpeed * Time.fixedDeltaTime);
            // 显示鼠标所在的地块
            ShowMousePosition(mouseWorldPosition);
        }

        #endregion



        #region 公开事件

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
            // 获取鼠标的坐标,转换为世界坐标
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        }

        #endregion



        #region 私有方法 

        /// <summary>
        /// 使用工具
        /// </summary>
        /// <param name="playerPosition">角色的位置和</param>
        /// <param name="mousePosition">鼠标的位置</param>
        private void UseTools(Vector2 playerPosition, Vector2 mousePosition)
        {
            // 将鼠标的世界坐标转换为Tilemap的坐标
            Vector3Int cellPosition = Tilemap.WorldToCell(mousePosition);
            // 获取地块的中心点坐标
            Vector2 cellCenter = (Vector2)Tilemap.GetCellCenterWorld(cellPosition);
            // 计算地块的中心点坐标和角色的距离
            float distance = Vector2.Distance(playerPosition, cellCenter);
            // 如果距离小于 distanceBetweenMouseAndPlayer
            if (distance < distanceBetweenMouseAndPlayer)
            {
                // 如果 cellPosition.x, cellPosition.y 在10*10的范围内
                if (cellPosition.x >= 0 && cellPosition.x < 10 && cellPosition.y >= 0 && cellPosition.y < 10)
                {
                    grid[cellPosition.x, cellPosition.y] = new GridData();
                    mModel.Grids.Value = grid;  // 重新设置以触发事件
                    Debug.Log("使用工具");

                }


            }
        }


        /// <summary>
        /// 显示鼠标所在的地块
        /// </summary>
        /// <param name="mousePosition">鼠标的世界坐标</param>
        private void ShowMousePosition(Vector2 mousePosition)
        {
            // 将鼠标的世界坐标转换为Tilemap的坐标
            Vector3Int cellPosition = Tilemap.WorldToCell(mousePosition);
            // 获取地块的中心点坐标
            Vector2 cellCenter = (Vector2)Tilemap.GetCellCenterWorld(cellPosition);
            // 获取地块的大小
            Vector3 cellSize = Tilemap.cellSize;


        }



        public IArchitecture GetArchitecture()
        {
            return ObjectFarmArchitecture.Interface;
        }


        #endregion

    }


}
