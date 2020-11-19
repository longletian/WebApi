using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApi.Models.Models.Identity
{
    /// <summary>
    /// 菜单实体
    /// </summary>
    public class MenuModel:Entity
    {
        [Required]
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
        [Required]
        /// <summary>
        /// 菜单路径
        /// </summary>
        public string MenuPath { get; set; }
        [Required]
        /// <summary>
        /// 菜单编码
        /// </summary>
        public string MenuCode { get; set; }
        /// <summary>
        /// 父菜单编码
        /// </summary>
        public string ParentMenuCode { get; set; }
        [Required]
        /// <summary>
        /// 菜单跳转url
        /// </summary>
        public string MenuUrl { get; set; }
        /// <summary>
        /// 菜单备注
        /// </summary>
        public string MenuRemark { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public long  SortNum  { get; set; }


        public IdentityRole IdentityRoles  { get; set; }
    }
}
