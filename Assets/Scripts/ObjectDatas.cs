namespace ObjectFarm
{
    public class GridData
    {
        // 状态属性
        public enum State
        {
            Moist,
            Reclaimed,
            Soil
        }
        // 状态
        public State state { get; set; }



        /// <summary>
        /// 无参构造函数
        /// </summary>
        public GridData()
        {
            this.state = State.Soil;
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="state"></param>
        public GridData(State state)
        {
            this.state = state;
        }



    }

}