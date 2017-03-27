using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Viola
{
    public enum ProjectStatus
    {
        [Display(Name = "Open")]
        Open,

        [Display(Name = "Paused")]
        Paused,

        [Display(Name = "Closed")]
        Closed
    }

    public enum TaskStatus
    {
        [Display(Name = "Open")]
        Open,

        [Display(Name = "Closed")]
        Closed
    }

    public enum TaskPriority
    {
        [Display(Name = "Normal")]
        Normal,

        [Display(Name = "High")]
        High,

        [Display(Name = "Showstopper")]
        Showstopper
    }

    public enum UserRole
    {
        [Display(Name = "External")]
        External,

        [Display(Name = "User")]
        User,

        [Display(Name = "Admin")]
        Admin
    }

    public enum DateFormat
    {
        [Display(Name = "Day/Month/Year")]
        DMY,

        [Display(Name = "Month/Day/Year")]
        MDY,

        [Display(Name = "Year/Month/Day")]
        YMD,

        [Display(Name = "Year/Day/Month")]
        YDM
    }

    public enum DateSeperator
    {
        [Display(Name = "Dot (.)")]
        Dot,

        [Display(Name = "Dash (-)")]
        Dash,

        [Display(Name = "Slash (/)")]
        Slash
    }

    //scelikol Cost Module
    public enum TaxNum
    {
        [Display(Name = " ")]
        none,

        [Display(Name = "KDV %1")]
        One,

        [Display(Name = "KDV %8")]
        Eight,

        [Display(Name = "KDV %18")]
        Eighteen
    }
    //
}