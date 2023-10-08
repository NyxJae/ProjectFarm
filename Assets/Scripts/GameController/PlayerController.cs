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

        /// <summary>
        /// 将鼠标的世界坐标转换为Tilemap的坐标
        /// </summary>
        private Vector3Int cellPosition = Vector3Int.zero;
        /// <summary>
        /// 获取地块的中心点坐标
        /// </summary>
        private Vector2 cellCenter = Vector2.zero;


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

        // gui更新
        private void OnGUI()
        {
            // 显示日期
            GUI.Label(new Rect(10, 0, 200, 200), mModel.Date.Value.ToString("yyyy-MM-dd"));
            // 显示按键功能

            GUI.Label(new Rect(10, 20, 200, 200), "鼠标左键:开垦地块");
            GUI.Label(new Rect(10, 40, 200, 200), "J键:消除地块");
            GUI.Label(new Rect(10, 60, 200, 200), "K键:种植植物");
            GUI.Label(new Rect(10, 80, 200, 200), "L键:浇水");
            GUI.Label(new Rect(10, 100, 200, 200), "P键:过一天");



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
                UseTools(cellPosition);
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



        /// <summary>
        /// 当 TestTools 动作被触发时，此函数将被调用
        /// </summary>
        /// <param name="context"></param>
        public void OnTestTools(InputAction.CallbackContext context)
        {
            // 按下时触发，松开时不触发 并且 鼠标和角色够近
            if (context.started && IsClose(transform.position, mouseWorldPosition))
            {
                // 获取触发该动作的控件的名称
                string controlName = context.control.name;
                // 将鼠标的世界坐标转换为Tilemap的坐标
                cellPosition = Tilemap.WorldToCell(mouseWorldPosition);



                // 如果是 键盘的 J 键
                if (controlName == "j")
                {
                    // 恢复泥土
                    GridController.ReSoilGrid(cellPosition);
                }
                // 如果是 键盘的 K 键
                else if (controlName == "k")
                {
                    // 种植植物
                    GridController.Plant(cellPosition);
                }
                // 如果是 键盘的 L 键
                else if (controlName == "l")
                {
                    // 浇水
                    GridController.Water(cellPosition);
                }
                // 如果是 键盘的 P 键
                else if (controlName == "p")
                {
                    // 过一天
                    PassOneDay();
                }
                else if (controlName == "o")
                {
                    // 获取果实
                    Debug.Log("获取果实");
                }
            }
        }


        #endregion



        #region 私有方法 

        /// <summary>
        /// 开垦地块
        /// </summary>
        /// <param name="cellPosition"> 地块的坐标</param>
        private void UseTools(Vector3Int cellPosition)
        {
            // 开垦
            GridController.Reclaimed(cellPosition);
        }


        /// <summary>
        /// 鼠标和角色是否够近
        /// </summary>
        /// <param name="playerPositio"> 玩家的位置</param>
        /// <param name="mousePosition"> 鼠标的位置</param>
        /// <returns> 是否够近</returns>
        private bool IsClose(Vector2 playerPositio, Vector2 mousePosition)
        {

            // 将鼠标的世界坐标转换为Tilemap的坐标
            Vector3Int cellPosition = Tilemap.WorldToCell(mousePosition);
            // 获取地块的中心点坐标
            Vector2 cellCenter = (Vector2)Tilemap.GetCellCenterWorld(cellPosition);
            // 计算地块的中心点坐标和角色的距离
            float distance = Vector2.Distance(playerPositio, cellCenter);
            return distance < distanceBetweenMouseAndPlayer;
        }


        // 过一天
        private void PassOneDay()
        {
            // 日期加一天
            mModel.Date.Value = mModel.Date.Value.AddDays(1);
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


        /// <summary>
        /// 控制器接口实现
        /// </summary>
        /// <returns></returns>
        public IArchitecture GetArchitecture()
        {
            return ObjectFarmArchitecture.Interface;
        }


        #endregion
    }


}
