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

        /// <summary>
        /// 植物格子数据
        /// </summary>
        private EasyGrid<GameObject> plantGrids = null;


        #endregion


        #region 生命周期函数

        private void Start()
        {
            // 获取model层
            mModel = this.GetModel<ObjectFarmModel>();
            // 获取地块数据
            grids = mModel.Grids.Value;
            // 获取植物格子数据
            plantGrids = mModel.PlantGrids.Value;
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

        //
        /// <summary>
        /// 公开开垦地块方法
        /// </summary>
        /// <param name="cellPosition"></param>
        public void Reclaimed(Vector3Int cellPosition)
        {
            // 获取地块数据
            grids = mModel.Grids.Value;
            // 如果地块数据不为空
            if (grids != null)
            {
                // 如果需开垦地块的坐标在grids的范围内
                // TODO:有BUG,应该是0-cellPosition.x
                if (grids.Width > cellPosition.x && grids.Height > cellPosition.y)
                {
                    // 如果地块数据的状态为泥土
                    if (grids[cellPosition.x, cellPosition.y].landState == GridData.LandState.Soil)
                    {
                        // 将地块数据的状态改为湿润
                        grids[cellPosition.x, cellPosition.y].landState = GridData.LandState.Reclaimed;
                        // 重新设置以触发地图重绘事件
                        mModel.Grids.Value = grids;
                    }
                }
            }
        }
        // 公开恢复泥土方法
        public void ReSoilGrid(Vector3Int cellPosition)
        {
            // 获取地块数据
            grids = mModel.Grids.Value;
            // 如果地块数据不为空
            if (grids != null)
            {
                // 如果需开垦地块的坐标在grids的范围内
                if (grids.Width > cellPosition.x && grids.Height > cellPosition.y)
                {
                    // 设置土地状态为泥土
                    grids[cellPosition.x, cellPosition.y].landState = GridData.LandState.Soil;
                    // 设置土地种植状态为未种植
                    grids[cellPosition.x, cellPosition.y].plantState = GridData.PlantState.None;
                    // 重新设置以触发地图重绘事件
                    mModel.Grids.Value = grids;
                }
            }
        }

        // 公开浇水方法
        public void Water(Vector3Int cellPosition)
        {
            // 获取地块数据
            grids = mModel.Grids.Value;
            // 如果地块数据不为空
            if (grids != null)
            {
                // 如果需开垦地块的坐标在grids的范围内
                if (grids.Width > cellPosition.x && grids.Height > cellPosition.y)
                {
                    // 如果地块数据的状态为开垦
                    if (grids[cellPosition.x, cellPosition.y].landState == GridData.LandState.Reclaimed)
                    {
                        // 将地块数据的状态改为湿润
                        grids[cellPosition.x, cellPosition.y].landState = GridData.LandState.Moist;
                        // 重新设置已触发地图重绘事件
                        mModel.Grids.Value = grids;
                    }
                }
            }
        }

        // 公开播种方法
        public void Plant(Vector3Int cellPosition)
        {
            // 获取地块数据
            grids = mModel.Grids.Value;
            // 如果地块数据不为空
            if (grids != null)
            {
                // 如果需开垦地块的坐标在grids的范围内
                if (grids.Width > cellPosition.x && grids.Height > cellPosition.y)
                {
                    // 如果地块数据的状态为开垦或者湿润,且土地种植状态为未种植
                    if ((grids[cellPosition.x, cellPosition.y].landState == GridData.LandState.Reclaimed || grids[cellPosition.x, cellPosition.y].landState == GridData.LandState.Moist) && grids[cellPosition.x, cellPosition.y].plantState == GridData.PlantState.None)
                    {
                        // rescontroller 生成种子,链式方法 
                        // 种子的位置是 cellCenter地块中点
                        // 种子的父物体是 Tilemap.transform
                        GameObject plant = ResController.Instance.Plant.Instantiate().Position((Vector2)Tilemap.GetCellCenterWorld(cellPosition)).Parent(Tilemap.transform);
                        // 设置植物的地块坐标
                        plant.gameObject.GetComponent<PlantController>().XCell = cellPosition.x;
                        plant.gameObject.GetComponent<PlantController>().YCell = cellPosition.y;
                        // 添加到植物地块数据中
                        mModel.PlantGrids.Value[cellPosition.x, cellPosition.y] = plant;
                        // 设置植物的地块数据
                        grids[cellPosition.x, cellPosition.y].plantState = GridData.PlantState.Seed;
                    }
                }
            }
        }


        // 公开收获方法
        public void GetFruit(Vector3Int cellPosition)
        {
            // 获取地块数据
            grids = mModel.Grids.Value;
            // 如果地块数据不为空
            if (grids != null)
            {
                // 如果需开垦地块的坐标在grids的范围内
                if (grids.Width > cellPosition.x && grids.Height > cellPosition.y)
                {
                    // 如果植物格子数据的x,y的植物控制器组件的植物状态为成熟
                    if (plantGrids[cellPosition.x, cellPosition.y].GetComponent<PlantController>() != null)
                    {
                        // 获取植物控制器组件的获取果实方法
                        plantGrids[cellPosition.x, cellPosition.y].GetComponent<PlantController>().GetFruit();
                        // 设置地块数据的种植状态为未种植
                        grids[cellPosition.x, cellPosition.y].plantState = GridData.PlantState.None;
                    }



                }
            }
        }

        #endregion
    }
}