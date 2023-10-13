using UnityEngine;
using QFramework;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using UnityEngine.Tilemaps;

namespace ObjectFarm
{
    public partial class toolsController : ViewController, IController
    {
        // 世界坐标
        private Vector2 mouseWorldPosition = Vector2.zero;

        // 工具相对鼠标x偏移量
        [SerializeField]
        private float offsetX = 0.5f;
        // 工具相对鼠标y偏移量
        [SerializeField]
        private float offsetY = 0.5f;

        /// <summary>
        /// 控制器接口实现,才能被框架调用
        /// </summary>
        /// <returns></returns>
        public IArchitecture GetArchitecture()
        {
            return ObjectFarmArchitecture.Interface;
        }

        /// <summary>
        /// model层
        /// </summary>
        private ObjectFarmModel mModel = null;

        // 当前选中物品编号
        private int ItemNum = 0;


        #region 公开按键回调方法
        /// <summary>
        /// 不同按键切换工具回调函数,当ChoiceTool动作被触发时调用
        /// </summary>
        /// <param name="context"></param>
        public void OnChoiceTool(InputAction.CallbackContext context)
        {
            // 如果按键是被按下的状态
            if (context.started)
            {
                // 获取按下的按键
                string key = context.control.name;
                // 如果按下的是1键
                if (key == "1")
                {
                    // 切换到铲子,显示铲子
                    SpriteRenderer.sprite = shovel;
                    // 设置当前选中物品编号为1
                    mModel.ItemNum.Value = 1;
                }
                // 如果按下的是2键
                else if (key == "2")
                {
                    // 切换到锄头,显示锄头
                    SpriteRenderer.sprite = hoe;
                    // 设置当前选中物品编号为2
                    mModel.ItemNum.Value = 2;
                }
                // 如果按下的是3键
                else if (key == "3")
                {
                    // 切换到种子包,显示种子包
                    SpriteRenderer.sprite = seedBag;
                    // 设置当前选中物品编号为3
                    mModel.ItemNum.Value = 3;
                }
                // 如果按下的是4键
                else if (key == "4")
                {
                    // 切换到水壶,显示水壶
                    SpriteRenderer.sprite = wateringCan;
                    // 设置当前选中物品编号为4
                    mModel.ItemNum.Value = 4;
                }
                else if (key == "5")
                {
                    // 切换到镰刀,显示镰刀
                    SpriteRenderer.sprite = sickle;
                    // 设置当前选中物品编号为5
                    mModel.ItemNum.Value = 5;
                }
                // 如果按下的是0键
                else if (key == "0")
                {
                    // 切换到手,渲染器关闭
                    Debug.Log("切换到手");
                    SpriteRenderer.sprite = null;
                    // 设置当前选中物品编号为0
                    mModel.ItemNum.Value = 0;
                }
            }
        }


        /// <summary>
        /// 跟随鼠标移动回调函数,当MouseMove动作被触发时调用
        /// </summary>
        /// <param name="context"></param>
        public void OnMouseMove(InputAction.CallbackContext context)
        {
            // 获取鼠标的坐标,转换为世界坐标
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            // 偏移
            mouseWorldPosition += new Vector2(offsetX, offsetY);
            // 移动角色
            transform.position = mouseWorldPosition;
        }

        /// <summary>
        /// 使用工具
        /// </summary>
        /// <param name="cellPosition"> 地块的坐标</param>
        public void UseTools(Vector3Int cellPosition)
        {
            // 获取当前选中物品编号
            ItemNum = mModel.ItemNum.Value;
            // 如果当前选中物品编号为1,铲子
            if (ItemNum == 1)
            {
                // 恢复泥土
                GridController.ReSoilGrid(cellPosition);
            }
            // 如果当前选中物品编号为2,锄头
            else if (ItemNum == 2)
            {
                // 开垦
                GridController.Reclaimed(cellPosition);
            }
            // 如果当前选中物品编号为3,种子包
            else if (ItemNum == 3)
            {
                // 种植植物
                GridController.Plant(cellPosition);
            }
            // 如果当前选中物品编号为4,水壶
            else if (ItemNum == 4)
            {
                // 浇水
                GridController.Water(cellPosition);
            }
            // 如果当前选中物品编号为5,镰刀
            else if (ItemNum == 5)
            {
                // 获取果实
                GridController.GetFruit(cellPosition);
            }
        }



        #endregion

        // 第一次Update之前调用
        private void Start()
        {
            // 获取model层
            mModel = this.GetModel<ObjectFarmModel>();

        }
    }
}
