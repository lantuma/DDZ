﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ETModel
{
	/// <summary>
	/// 用来与数据库操作代理
	/// </summary>
	public class DBProxyComponent: Component
	{
		public IPEndPoint dbAddress;
    }
}