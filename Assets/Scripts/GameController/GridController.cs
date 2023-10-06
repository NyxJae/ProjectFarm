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

        #region 地块相关字段
        /// <summary>
        /// 地块格子数据
        /// </summary>
        private EasyGrid<GridData> grids = null;


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
            // 注册事件当mModel.Data.Value发生变化时，执行以下代码,gameObject销毁后自动取消注册
            mModel.Date.Register(newValue =>
            {
                // 获取地块数据
                grids = mModel.Grids.Value;
                // 所有湿润的地块变成已开垦
                grids.ForEach((x, y, gridData) =>
                {
                    if (gridData.landState == GridData.LandState.Moist)
                    {
                        gridData.landState = GridData.LandState.Reclaimed;
                    }
                });
                // 重新设置已触发事件
                mModel.Grids.Value = grids;


            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            // 绘制初始地块
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




        #endregion



    }
}
