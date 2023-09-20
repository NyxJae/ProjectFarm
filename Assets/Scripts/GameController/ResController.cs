using UnityEngine;
using QFramework;

namespace ObjectFarm
{
	// 资源管理器
	public partial class ResController : ViewController,ISingleton
	{
		

		// 单例对象
		public static ResController Instance => MonoSingletonProperty<ResController>.Instance;

        public void OnSingletonInit()
        {
            
        }

        void Start()
		{
			// Code Here
		}
	}
}
