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
        /// <summary>
        /// BindableProperty 地图数据
        /// </summary>
        public BindableProperty<EasyGrid<GridData>> Grids { get; } = new BindableProperty<EasyGrid<GridData>>();

        /// <summary>
        /// 日期
        /// </summary>
        public BindableProperty<DateTime> Date { get; } = new BindableProperty<DateTime>();


        protected override void OnInit()
        {
            #region 地图数据初始化
            // 为Grids.Value注册一个比较器，只要改了就会触发事件
            Grids.WithComparer((EasyGrid<GridData> a, EasyGrid<GridData> b) =>
                {
                    // 因为EasyGrid是引用类型，比较的是引用地址，得深克隆,麻烦,直接返回false
                    return false;
                });

            // 在这里初始化 ObjectFarmModel 类中的属性或者执行其他必要的操作
            Grids.Value = new EasyGrid<GridData>(10, 10);

            // 遍历所有的格子，初始化为泥土
            Grids.Value.ForEach((x, y, gridData) =>
            {
                Grids.Value[x, y] = new GridData();
            });
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


        }
    }
}


