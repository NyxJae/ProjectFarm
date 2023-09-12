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
            // 在这里初始化 ObjectFarmModel 类中的属性或者执行其他必要的操作
            Grids.Value = new EasyGrid<GridData>(10, 10);
        }
    }
}


