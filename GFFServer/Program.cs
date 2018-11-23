﻿/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：20da4241-0bdc-4a06-8793-6d0889c31f95
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：MCITest


 * 创建年份：2015
 * 创建时间：2015-12-02 11:15:24
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/

using GFF.Component.Config;
using GFF.Helper;
using SAEA.MessageSocket;
using SAEA.MVC;
using SAEA.Sockets.Interface;
using System;

namespace GFFServer
{
    internal class Program
    {
        private static MessageServer messageServer;

        private static SAEAMvcApplication mvcApplication;

        private static void Main(string[] args)
        {
            Console.Title = "GFFServer";


            ConsoleHelper.WriteLine("正在初始化消息服务器...", ConsoleColor.Green);
            messageServer = new MessageServer();
            messageServer.OnAccepted += Server_OnAccepted;
            messageServer.OnError += Server_OnError;
            messageServer.OnDisconnected += Server_OnDisconnected;
            ConsoleHelper.WriteLine("消息服务器初始化完毕...", ConsoleColor.Green);



            ConsoleHelper.WriteLine("正在初始化文件服务器...", ConsoleColor.DarkYellow);
            var filePort = ServerConfig.Instance().FilePort;
            mvcApplication = new SAEAMvcApplication(port: filePort);
            mvcApplication.SetDefault("File", "Test");
            ConsoleHelper.WriteLine("文件服务器初始化完毕，http://127.0.0.1:" + filePort + "/...", ConsoleColor.DarkYellow);



            ConsoleHelper.WriteLine("正在启动消息服务器...", ConsoleColor.Green);
            messageServer.Start();
            ConsoleHelper.WriteLine("消息服务器启动完毕...", ConsoleColor.Green);



            ConsoleHelper.WriteLine("正在启动文件服务器...", ConsoleColor.DarkYellow);
            mvcApplication.Start();
            ConsoleHelper.WriteLine("文件服务器启动完毕...", ConsoleColor.DarkYellow);



            ConsoleHelper.WriteLine("点击回车，结束服务");
            Console.ReadLine();
        }

        private static void Server_OnDisconnected(string ID, Exception ex)
        {
            ConsoleHelper.WriteInfo(string.Format("客户端{0}已断开连接，当前连接数共记：{1}", ID, messageServer.ClientCounts));
        }

        private static void Server_OnError(string ID, Exception ex)
        {
            ConsoleHelper.WriteErr(ex);
        }

        private static void Server_OnAccepted(IUserToken userToken)
        {
            ConsoleHelper.WriteInfo(string.Format("客户端{0}已连接，当前连接数共记：{1}", userToken.ID, messageServer.ClientCounts));
        }
    }
}