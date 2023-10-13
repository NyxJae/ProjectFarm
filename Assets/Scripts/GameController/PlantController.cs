using UnityEngine;
using QFramework;
using System;

namespace ObjectFarm
{
    public partial class PlantController : ViewController, IController
    {
        // Qframework 实现架构接口的方法
        public IArchitecture GetArchitecture()
        {
            // 返回架构
            return ObjectFarmArchitecture.Interface;
        }
        /// <summary>
        /// model层
        /// </summary>
        private ObjectFarmModel mModel = null;


        private int xCell = 0;
        /// <summary>
        /// 种植地的Xcell
        /// </summary>
        public int XCell { get => xCell; set => xCell = value; }

        private int yCell = 0;
        /// <summary>
        /// 种植地的Ycell
        /// </summary>
        public int YCell { get => yCell; set => yCell = value; }
        /// <summary>
        /// 地块数据
        /// </summary>
        private GridData grid = null;


        // 植物格子数据
        private EasyGrid<GameObject> plantGrids = null;

        /// <summary>
        /// 生长天数
        /// </summary>
        private int days = 0;

        /// <summary>
        /// 当天是否可生长
        /// </summary>
        private bool isGrow = true;



        /// <summary>
        /// 种子-幼苗天数
        /// </summary>
        [SerializeField]
        [Header("种子-幼苗天数")]
        public int SeedlingTime = 1;

        /// <summary>
        /// 种子-成熟天数
        /// </summary>
        [SerializeField]
        [Header("种子-成熟天数")]
        public int MatureTime = 4;



        private void Start()
        {
            // 获取model层
            mModel = this.GetModel<ObjectFarmModel>();
            // 获取grid数据
            grid = mModel.Grids.Value[XCell, YCell];
            // 获取植物格子数据
            plantGrids = mModel.PlantGrids.Value;

            // 注册事件当mModel.Grids.Value发生变化时，执行以下代码,gameObject销毁后自动取消注册
            mModel.Grids.Register(newValue =>
            {

                // 如果土地状态是潮湿的,并且当天可生长,就增加生长天数
                if (grid.landState == GridData.LandState.Moist && isGrow)
                {
                    // 增加生长天数
                    days++;
                    // 设置当天不可生长
                    isGrow = false;
                }
                // 如果土地状态是泥土未开垦,就删除植物
                else if (grid.landState == GridData.LandState.Soil)
                {
                    // 获取植物格子数据
                    plantGrids = mModel.PlantGrids.Value;
                    // 如果xCell,yCell的植物数据不为空
                    if (plantGrids != null)
                    {
                        if (plantGrids[xCell, yCell] != null)
                        {
                            // 就删除植物
                            plantGrids[xCell, yCell].DestroySelf();
                        }
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 注册事件当mModel.Data.Value发生变化时，执行以下代码,gameObject销毁后自动取消注册
            mModel.Date.Register(newValue =>
            {
                // 设置当天可生长
                isGrow = true;
                // 如果时间大于幼苗天数，小于成熟天数
                if (days >= SeedlingTime && days < MatureTime)
                {
                    // 种子变成幼苗
                    gameObject.GetComponent<SpriteRenderer>().sprite = seedling;
                }
                // 如果时间大于成熟天数
                else if (days >= MatureTime)
                {
                    // 种子变成成熟    
                    gameObject.GetComponent<SpriteRenderer>().sprite = mature;
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);



        }

        /// <summary>
        /// 公开获取果实方法
        /// </summary>
        public void GetFruit()
        {
            // 如果xCell,yCell的植物数据不为空
            if (plantGrids != null)
            {
                // 如果是成熟的
                if (plantGrids[xCell, yCell].GetComponent<SpriteRenderer>().sprite == mature)
                {
                    // 就删除植物
                    plantGrids[xCell, yCell].DestroySelf();
                    // 就增加背包内果实数量
                    mModel.FruitNum.Value++;


                }
            }

        }
    }
}
