using System.Collections.Generic;
using System.Linq;

namespace Funeral.Core.Common.Helper
{
    /// <summary>
    /// 泛型递归求树形结构
    /// </summary>
    public static class RecursionHelper
    {
        public static void LoopToAppendChildren(List<PermissionTree> all, PermissionTree curItem, int pid)
        {

            var subItems = all.Where(ee => ee.Pid == curItem.Value).ToList();

           // var btnItems = subItems.Where(ss => ss.Isbtn == true).ToList();
            //if (subItems.Count > 0)
            //{
            //    curItem.Btns = new List<PermissionTree>();
            //    curItem.Btns.AddRange(btnItems);
            //}
            //else
            //{
            //    curItem.Btns = null;
            //}

            //if (!needbtn)
            //{
            //    subItems = subItems.Where(ss => ss.Isbtn == false).ToList();
            //}
            if (subItems.Count > 0)
            {
                curItem.Children = new List<PermissionTree>();
                curItem.Children.AddRange(subItems);
            }
            else
            {
                curItem.Children = null;
            }

            if (curItem.Isbtn)
            {
                //curItem.label += "按钮";
            }

            foreach (var subItem in subItems)
            {
                if (subItem.Value == pid && pid > 0)
                {
                    //subItem.disabled = true;//禁用当前节点
                }
                LoopToAppendChildren(all, subItem, pid);
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
            var subItems = all.Where(ee => ee.Pid == curItem.Id).ToList();

            if (subItems.Count > 0)
            {
                curItem.Children = new List<NavigationBar>();
                curItem.Children.AddRange(subItems);
            }
            else
            {
                curItem.Children = null;
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
        public int Value { get; set; }
        public int Pid { get; set; }
        public string Label { get; set; }
        public int Order { get; set; }
        public bool Isbtn { get; set; }
        public bool Disabled { get; set; }
        public List<PermissionTree> Children { get; set; }
        public List<PermissionTree> Btns { get; set; }
    }

    //菜单路由树
    public class NavigationBar
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 父级菜单ID
        /// </summary>
        public int Pid { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
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
        public string Path { get; set; }
        /// <summary>
        /// 按钮事件
        /// </summary>
        public string Func { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string IconCls { get; set; }
        /// <summary>
        /// api接口地址
        /// </summary>
        public string ApiLink { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public NavigationBarMeta Meta { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// api接口id
        /// </summary>
        public int Mid { get; set; }

        /// <summary>
        /// 子级菜单
        /// </summary>
        public List<NavigationBar> Children { get; set; }
    }


    //菜单配置树
    public class NavigationBarConfiguar
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 父级菜单ID
        /// </summary>
        public int Pid { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否按钮
        /// </summary>
        public bool IsButton { get; set; } = false;
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 图片地址/
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 子级菜单
        /// </summary>
        public List<NavigationBar> Children { get; set; }
    }

    public class NavigationBarMeta
    {
        public string Title { get; set; }
        public bool RequireAuth { get; set; } = true;
        public bool NoTabPage { get; set; } = false;
        public bool KeepAlive { get; set; } = false;


    }
}
