using System.Globalization;
using UnityEngine;
using QFramework;
using UnityEngine.Tilemaps;


namespace ObjectFarm
{
    public partial class GridController : ViewController, IController
    {



        #region  tilemap相关字段

        /// <summary>
        /// 笔刷 生成地块用
        /// </summary>
        [Tooltip("笔刷 生成地块用")]
        public Tile brush = null;


        public IArchitecture GetArchitecture()
        {
            return ObjectFarmArchitecture.Interface;
        }

        #endregion



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
            // 当mModel.Grids.Value发生变化时，执行以下代码,gameObject销毁后自动取消注册
            mModel.Grids.Register(newValue =>
            {
                Debug.Log("触发了地图绘制");
                if (newValue != null)
                {
                    mModel.Grids.Value.ForEach((x, y, gridData) =>
                    {
                        // 如果地块数据不为空
                        if (gridData == null)
                        {
                            // 清除地块
                            Tilemap.SetTile(new Vector3Int(x, y, 0), null);
                        }
                        else if (gridData != null)
                        {
                            // 生成地块
                            Tilemap.SetTile(new Vector3Int(x, y, 0), brush);

                        }

                    });
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        #endregion

    }
}
