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

        }

        // gui更新
        private void OnGUI()
        {
            // 显示日期
            GUI.Label(new Rect(10, 0, 200, 200), mModel.Date.Value.ToString("yyyy-MM-dd"));
            // 显示按键功能

            GUI.Label(new Rect(10, 20, 200, 200), "鼠标左键:使用工具");
            GUI.Label(new Rect(10, 40, 200, 200), "1键:铲子消除地块");
            GUI.Label(new Rect(10, 60, 200, 200), "2键:锄头,开垦");
            GUI.Label(new Rect(10, 80, 200, 200), "3键:种子包,播种");
            GUI.Label(new Rect(10, 100, 200, 200), "4键:水壶,浇水");
            GUI.Label(new Rect(10, 120, 200, 200), "5键:镰刀,收获");
            GUI.Label(new Rect(10, 140, 200, 200), "p键:过一天");
            // 显示背包内果实数量
            GUI.Label(new Rect(10, 160, 200, 200), "背包内果实数量:" + mModel.FruitNum.Value);



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
            // 按下时触发，松开时不触发 并且 鼠标和角色够近
            if (context.started && IsClose(transform.position, mouseWorldPosition))
            {
                // 将鼠标的世界坐标转换为Tilemap的坐标
                cellPosition = Tilemap.WorldToCell(mouseWorldPosition);
                // 使用工具
                ToolsController.UseTools(cellPosition);
            }
        }


        /// <summary>
        /// 当 OnMouseMove 动作被触发时(鼠标移动时)，此函数将被调用
        /// </summary>
        /// <param name="context"></param>
        public void OnMouseMove(InputAction.CallbackContext context)
        {
            // 获取鼠标的坐标,转换为世界坐标
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        }


        #endregion



        #region 私有方法 



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
        public void OnPassDay(InputAction.CallbackContext context)
        {
            // 按下时触发，松开时不触发 并且 鼠标和角色够近
            if (context.started)
            {
                // 日期加一天
                mModel.Date.Value = mModel.Date.Value.AddDays(1);

            }
        }



        /// <summary>
        /// 控制器接口实现,才能被框架调用
        /// </summary>
        /// <returns></returns>
        public IArchitecture GetArchitecture()
        {
            return ObjectFarmArchitecture.Interface;
        }


        #endregion
    }


}
