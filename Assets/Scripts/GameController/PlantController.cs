using UnityEngine;
using QFramework;
using System;

namespace ObjectFarm
{
    public partial class PlantController : ViewController, IController
    {
        public IArchitecture GetArchitecture()
        {
            // 返回架构
            return ObjectFarmArchitecture.Interface;
        }
        /// <summary>
        /// model层
        /// </summary>
        private ObjectFarmModel mModel = null;

        /// <summary>
        /// 播种日期
        /// </summary>
        private DateTime starTime;

        // 种植地的xcell
        private int xCell = 0;
        public int XCell { get => xCell; set => xCell = value; }

        // 种植地的ycell
        private int yCell = 0;
        public int YCell { get => yCell; set => yCell = value; }


        /// <summary>
        /// 生长天数
        /// </summary>
        private int days = 0;



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
            // 获取start日期为播种日期
            starTime = mModel.Date.Value;
            // 注册事件当mModel.Grids.Value发生变化时，执行以下代码,gameObject销毁后自动取消注册
            mModel.Date.Register(newValue =>
            {
                // 获取grid数据
                GridData grid = mModel.Grids.Value[XCell, YCell];
                // 如果土地状态是潮湿的
                if (grid.landState == GridData.LandState.Moist)
                {
                    // 增加生长天数
                    days++;
                }
                
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

    }
}
