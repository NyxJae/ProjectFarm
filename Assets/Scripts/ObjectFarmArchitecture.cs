using QFramework;

namespace ObjectFarm
{
    public class ObjectFarmArchitecture : Architecture<ObjectFarmArchitecture>
    {
        // 初始化框架,注册各个模块
        protected override void Init()
        {
            // 注册数据层
            this.RegisterModel(new ObjectFarmModel());

        }
    }
}