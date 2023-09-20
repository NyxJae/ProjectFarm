using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace ObjectFarm
{
    // 数据管理
    public class ObjectFarmModel : AbstractModel
    {
        // BindableProperty 地图 数据管理
        public BindableProperty<EasyGrid<GridData>> Grids { get; } = new BindableProperty<EasyGrid<GridData>>();

        protected override void OnInit()
        {
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


        }
    }
}


