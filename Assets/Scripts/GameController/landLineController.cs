using UnityEngine;
using QFramework;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace ObjectFarm
{
    public partial class landLineController : ViewController, IController
    {
        // 世界坐标
        private Vector2 mouseWorldPosition = Vector2.zero;
        // tilemap坐标
        private Vector3Int mouseTilePosition = Vector3Int.zero;
        // tilemap
        public Tilemap Tilemap = null;


        void Start()
        {
            // 从场景中获取tilemap
            Tilemap = GameObject.FindObjectOfType<Tilemap>();
        }

        /// <summary>
        /// 跟随鼠标移动回调事件,当MouseMove动作被触发时调用
        /// </summary>
        /// <param name="context"></param>
        public void OnMouseMove(InputAction.CallbackContext context)
        {
            // 获取鼠标的坐标,转换为世界坐标
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            // 将鼠标的世界坐标转换为Tilemap的坐标
            Vector3Int cellPosition = Tilemap.WorldToCell(mouseWorldPosition);
            // 获取地块的中心点坐标
            Vector2 cellCenter = (Vector2)Tilemap.GetCellCenterWorld(cellPosition);
            // 移动角色
            transform.position = cellCenter;
        }

        // Qframework 实现架构接口的方法
        public IArchitecture GetArchitecture()
        {
            return ObjectFarmArchitecture.Interface;
        }
    }
}
