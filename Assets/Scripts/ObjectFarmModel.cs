using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;

namespace ObjectFarm
{
    /// <summary>
    /// 全局数据管理
    /// </summary>
    public class ObjectFarmModel : AbstractModel
    {
        // 地图宽度
        private int width = 10;
        // 地图高度
        private int height = 10;



        /// <summary>
        /// BindableProperty 地图数据
        /// </summary>
        public BindableProperty<EasyGrid<GridData>> Grids = new BindableProperty<EasyGrid<GridData>>();

        /// <summary>
        /// 日期
        /// </summary>
        public BindableProperty<DateTime> Date = new BindableProperty<DateTime>();

        /// <summary>
        /// 植物数据
        /// </summary>
        public BindableProperty<EasyGrid<GameObject>> PlantGrids = new BindableProperty<EasyGrid<GameObject>>();

        /// <summary>
        /// 背包内果实数量
        /// </summary>
        public BindableProperty<int> FruitNum = new BindableProperty<int>();

        // 当前物品编号
        public BindableProperty<int> ItemNum = new BindableProperty<int>();


        protected override void OnInit()
        {
            #region 植物数据初始化
            // 为PlantGrids.Value注册一个比较器，只要改了就会触发事件
            PlantGrids.WithComparer((EasyGrid<GameObject> a, EasyGrid<GameObject> b) =>
            {
                // 因为EasyGrid是引用类型，比较的是引用地址，得深克隆,麻烦,直接返回false
                return false;
            });
            // 设置植物格子数据大小为地图大小
            PlantGrids.Value = new EasyGrid<GameObject>(width, height);

            #endregion


            #region 地图数据初始化
            // 为Grids.Value注册一个比较器，只要改了就会触发事件
            Grids.WithComparer((EasyGrid<GridData> a, EasyGrid<GridData> b) =>
                {
                    // 因为EasyGrid是引用类型，比较的是引用地址，得深克隆,麻烦,直接返回false
                    return false;
                });

            // 设置地图大小
            Grids.Value = new EasyGrid<GridData>(width, height);

            // 遍历所有的格子，初始化为泥土
            Grids.Value.ForEach((x, y, gridData) =>
            {
                Grids.Value[x, y] = new GridData();
            });

            // 
            #endregion

            #region 日期初始化
            // 初始日期为 2020年1月1日(字符串转日期)
            Date.Value = DateTime.Parse("2020-01-01");
            Date.WithComparer((DateTime a, DateTime b) =>
            {
                // 因为DateTime是引用类型，比较的是引用地址，得深克隆,麻烦,直接返回false
                return false;
            });
            #endregion

            // 当前物品编号初始化
            ItemNum.Value = 0;


        }
    }
}


