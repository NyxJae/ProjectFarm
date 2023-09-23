using UnityEngine;
using QFramework;
using System;

namespace ObjectFarm
{
    public partial class SeedController : ViewController, IController
    {
        public IArchitecture GetArchitecture()
        {
            // 返回架构
            return ObjectFarmArchitecture.Interface;
        }

        /// <summary>
        /// 播种日期
        /// </summary>
        private DateTime starTime;

        /// <summary>
        /// model层
        /// </summary>
        private ObjectFarmModel mModel = null;

        /// <summary>
        /// 种子-幼苗时长
        /// </summary>
        [SerializeField]
        [Header("种子-幼苗时长")]
        public int SeedlingTime = 1;

        /// <summary>
        /// 种子-成熟时长
        /// </summary>
        [SerializeField]
        [Header("种子-成熟时长")]
        public int MatureTime = 4;


        private void Start()
        {
            // 获取model层
            mModel = this.GetModel<ObjectFarmModel>();
            // 获取当前日期
            starTime = mModel.Date.Value;
            // 注册事件当mModel.Grids.Value发生变化时，执行以下代码,gameObject销毁后自动取消注册
            mModel.Date.Register(newValue =>
            {
                // 当前日期与播种日期的差(天数)
                int days = (int)(newValue - starTime).TotalDays;
                // 如果时间大于幼苗时长，小于成熟时长
                if (days >= SeedlingTime && days < MatureTime)
                {
                    // 种子变成幼苗
                    gameObject.GetComponent<SpriteRenderer>().sprite = seedling;
                }
                // 如果时间大于成熟时长
                else if (days >= MatureTime)
                {
                    // 种子变成成熟
                    gameObject.GetComponent<SpriteRenderer>().sprite = mature;
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);


        }

    }
}
