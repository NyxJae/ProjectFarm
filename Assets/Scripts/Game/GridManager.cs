using UnityEngine;
using QFramework;
using UnityEngine.Tilemaps;


namespace ObjectFarm
{
	public partial class GridManager : ViewController
	{
		/// <summary>
		/// 地块数据
		/// </summary>
		private EasyGrid<GridData> mGrids = new EasyGrid<GridData>(10, 10);


		public EasyGrid<GridData> Grids
		{
			get { return mGrids; }
		}


		/// <summary>
		/// 笔刷 生成地块用
		/// </summary>
		[Tooltip("笔刷 生成地块用")]
		public Tile brush;



		void Start()
		{

		}

		private void Update()
		{

		}
	}
}
