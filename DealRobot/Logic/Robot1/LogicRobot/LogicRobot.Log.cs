using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DealRobot
{
    partial class LogicRobot
    {
        public static string[] msg_array = {
            @"机器人开始重启",//0
            @"机器人重启完成",//1
            @"机器人开始运行",//2
            @"机器人收到取片信号去取片",//3
            @"机器人取片真空建立",//4
            @"机器人离开取片区域",//5
            @"机器人去精定位",//6
            @"机器人第1次精定位成功",//7
            @"机器人第2次精定位成功",//8
            @"机器人离开精定位",//9
            @"机器人去平台放片",//10
            @"机器人到达平台",//11
            @"机器人平台真空交接成功",//12
            @"机器人离开平台"//13
        };

        public static string[] alarm_array =
        {
            @"机器人粗定位真空建立失败",
            @"机器人第1次精定位失败",
            @"机器人第2次精定位失败",
            @"机器人平台交接失败",
            @"机器人运行中真空破坏",
            @"机器人粗定位取片位置X超限",
            @"机器人粗定位取片位置Y超限",
            @"机器人平台放片位置X超限",
            @"机器人平台放片位置Y超限",
            @"机器人复位位置异常"
        };
    }
}
