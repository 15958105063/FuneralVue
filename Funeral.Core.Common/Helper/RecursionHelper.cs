using System.Collections.Generic;
using System.Linq;

namespace Funeral.Core.Common.Helper
{
    /// <summary>
    /// 泛型递归求树形结构
    /// </summary>
    public static class RecursionHelper
    {
        public static void LoopToAppendChildren(List<PermissionTree> all, PermissionTree curItem, int pid, bool needbtn)
        {

            var subItems = all.Where(ee => ee.Pid == curItem.value).ToList();

            var btnItems = subItems.Where(ss => ss.isbtn == true).ToList();
            if (subItems.Count > 0)
            {
                curItem.btns = new List<PermissionTree>();
                curItem.btns.AddRange(btnItems);
            }
            else
            {
                curItem.btns = null;
            }

            if (!needbtn)
            {
                subItems = subItems.Where(ss => ss.isbtn == false).ToList();
            }
            if (subItems.Count > 0)
            {
                curItem.children = new List<PermissionTree>();
                curItem.children.AddRange(subItems);
            }
            else
            {
                curItem.children = null;
            }

            if (curItem.isbtn)
            {
                //curItem.label += "按钮";
            }

            foreach (var subItem in subItems)
            {
                if (subItem.value == pid && pid > 0)
                {
                    //subItem.disabled = true;//禁用当前节点
                }
                LoopToAppendChildren(all, subItem, pid, needbtn);
            }
        }



        /// <summary>
        /// 泛型获取树状菜单
        /// </summary>
        /// <param name="all"></param>
        /// <param name="curItem"></param>
        public static void LoopNaviBarAppendChildren(List<NavigationBar> all, NavigationBar curItem)
        {

            //获取菜单第一级（pid=0）
            var subItems = all.Where(ee => ee.pid == curItem.id).ToList();

            if (subItems.Count > 0)
            {
                curItem.children = new List<NavigationBar>();
                curItem.children.AddRange(subItems);
            }
            else
            {
                curItem.children = null;
            }

            foreach (var subItem in subItems)
            {
                LoopNaviBarAppendChildren(all, subItem);
            }
        }



        public static void LoopToAppendChildrenT<T>(List<T> all, T curItem, string parentIdName = "Pid", string idName = "value", string childrenName = "children")
        {
            var subItems = all.Where(ee => ee.GetType().GetProperty(parentIdName).GetValue(ee, null).ToString() == curItem.GetType().GetProperty(idName).GetValue(curItem, null).ToString()).ToList();

            if (subItems.Count > 0) curItem.GetType().GetField(childrenName).SetValue(curItem, subItems);
            foreach (var subItem in subItems)
            {
                LoopToAppendChildrenT(all, subItem);
            }
        }
    }

    /// <summary>
    /// 菜单路由树状类
    /// </summary>
    public class PermissionTree
    {
        public int value { get; set; }
        public int Pid { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool isbtn { get; set; }
        public bool disabled { get; set; }
        public List<PermissionTree> children { get; set; }
        public List<PermissionTree> btns { get; set; }
    }

    //菜单路由树
    public class NavigationBar
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 父级菜单ID
        /// </summary>
        public int pid { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int order { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 是否隐藏菜单
        /// </summary>
        public bool IsHide { get; set; } = false;
        /// <summary>
        /// 是否按钮
        /// </summary>
        public bool IsButton { get; set; } = false;
        /// <summary>
        /// 路由地址
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 按钮事件
        /// </summary>
        public string Func { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string iconCls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public NavigationBarMeta meta { get; set; }
        /// <summary>
        /// 子级菜单
        /// </summary>
        public List<NavigationBar> children { get; set; }
    }

    public class NavigationBarMeta
    {
        public string title { get; set; }
        public bool requireAuth { get; set; } = true;
        public bool NoTabPage { get; set; } = false;
        public bool keepAlive { get; set; } = false;


    }
}
