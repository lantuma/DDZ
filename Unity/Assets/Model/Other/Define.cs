namespace ETModel
{
	public static class Define
	{
#if UNITY_EDITOR && !ASYNC
		public static bool IsAsync = false;
#else
        public static bool IsAsync = true;
#endif

#if UNITY_EDITOR
		public static bool IsEditorMode = true;
#else
		public static bool IsEditorMode = false;
#endif

#if DEVELOPMENT_BUILD
		public static bool IsDevelopmentBuild = true;
#else
		public static bool IsDevelopmentBuild = false;
#endif

#if ILRuntime
		public static bool IsILRuntime = true;
#else
		public static bool IsILRuntime = false;
#endif

        /// <summary>
        /// 是否使用编辑器资源
        /// </summary>
        public static bool UseEditorResouces = false;


        /// <summary>
        /// 玩家是否进入到游戏房间内，用作屏蔽服务器广播的无用消息
        /// </summary>
        public static bool IsJoinNNRoom = false;

        /// <summary>
        /// 是否打印输出服务器消息
        /// </summary>
        public static bool IsOutPutServerMsg;
    }
}