using System.Globalization;
using UnityEngine;
using QFramework;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace ObjectFarm
{
    public partial class GridController : ViewController, IController
    {



        public IArchitecture GetArchitecture()
        {
            return ObjectFarmArchitecture.Interface;
        }


        #region 系统相关字段

        /// <summary>
        /// model层
        /// </summary>
        private ObjectFarmModel mModel = null;

        #endregion


        #region 生命周期函数

        private void Start()
        {
            // 获取model层
            mModel = this.GetModel<ObjectFarmModel>();
            // 注册事件当mModel.Grids.Value发生变化时，执行以下代码,gameObject销毁后自动取消注册
            mModel.Grids.Register(newValue =>
            {
                if (newValue != null)
                {
                    // 绘制地块
                    DrawTile();
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            // 绘制地块
            DrawTile();

        }



        /// <summary>
        /// 更新地块
        /// </summary>
        private void DrawTile()
        {
            mModel.Grids.Value.ForEach((x, y, gridData) =>
            {
                // 如果地块数据不为空
                if (gridData != null)
                {
                    // 如果地块数据的状态为泥土
                    if (gridData.landState == GridData.LandState.Soil)
                    {
                        // 绘制泥土
                        Tilemap.SetTile(new Vector3Int(x, y, 0), land_soil);
                        // 更新植物
                        UpdatePlant(x, y);

                    }
                    // 如果地块数据的状态为湿润
                    else if (gridData.landState == GridData.LandState.Moist)
                    {
                        // 绘制湿润
                        Tilemap.SetTile(new Vector3Int(x, y, 0), land_moist);
                    }
                    // 如果地块数据的状态为开垦
                    else if (gridData.landState == GridData.LandState.Reclaimed)
                    {
                        // 绘制开垦
                        Tilemap.SetTile(new Vector3Int(x, y, 0), land_Reclaimed);
                    }

                }

            });
        }

        /// <summary>
        /// 植物更新
        /// </summary>
        /// <param name="xCell"></param>
        /// <param name="yCell"></param>
        private void UpdatePlant(int xCell, int yCell)
        {
            // 获取植物格子数据
            EasyGrid<GameObject> plantGrids = mModel.PlantGrids.Value;
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



        #endregion



    }
}
