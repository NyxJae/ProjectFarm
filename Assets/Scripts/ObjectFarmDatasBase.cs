namespace ObjectFarm
{
    /// <summary>
    /// 土地数据类
    /// </summary>
    public class GridData
    {
        /// <summary>
        /// 土地状态属性枚举
        /// </summary>
        public enum LandState
        {
            Moist,
            Reclaimed,
            Soil
        }
        /// <summary>
        /// 土地状态
        /// </summary>
        public LandState landState { get; set; }

        /// <summary>
        /// 种植状态枚举
        /// </summary>
        public enum PlantState
        {
            None,
            Seed,
            tree,
            Dead
        }

        /// <summary>
        /// 种植状态
        /// </summary>
        public PlantState plantState { get; set; }




        /// <summary>
        /// 无参构造函数
        /// </summary>
        public GridData()
        {
            this.landState = LandState.Soil;
            this.plantState = PlantState.None;
        }



    }

}