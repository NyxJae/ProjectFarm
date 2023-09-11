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
		private EasyGrid<GridData> grids = new EasyGrid<GridData>(10, 10);

		/// <summary>
		/// 笔刷 生成地块用
		/// </summary>
		[Tooltip("笔刷 生成地块用")]
		public Tile brush;



		void Start()
		{
			// 填充 grids 的数据
			grids.Fill(new GridData());
			// 生成地块
			grids.ForEach((x, y, gridData) =>
			{
				if (gridData != null)
				{
					// 生成地块
					Tilemap.SetTile(new Vector3Int(x, y), brush);
				}
			});
		}
	}
}
