using System;
using UnityEngine;

//namespace MMGame.Framework
//{
    public static class ScreenExtension
    {

		// 获得分辨率，当选择 Free Aspect 直接返回相机的像素宽和高
		public static Resolution GetScreenResolution()
		{
			var resolution = Screen.currentResolution;
			//Vector2 dimensions = new Vector2(Screen.width, Screen.height);

#if UNITY_EDITOR
			// 获取编辑器 GameView 的分辨率
			float gameViewPixelWidth = 0, gameViewPixelHeight = 0;
			float gameViewAspect = 0;

			if (Editor__GetGameViewSize(out gameViewPixelWidth, out gameViewPixelHeight, out gameViewAspect))
			{
				if (gameViewPixelWidth != 0 && gameViewPixelHeight != 0)
				{
					resolution.width = (int)gameViewPixelWidth;
					resolution.height = (int)gameViewPixelHeight;
				}
			}
#endif

			return resolution;
		}

        /// <summary>
        /// Determines if is special aspect the specified aspect.
        /// </summary>
        /// <returns><c>true</c> if is special aspect the specified aspect; otherwise, <c>false</c>.</returns>
        /// <param name="aspect">Aspect.</param>
        public static bool IsSpecialAspect(float aspect)
        {
            var resolution = Screen.currentResolution;

            float gameViewPixelWidth = resolution.width;
            float gameViewPixelHeight = resolution.height;
            float gameViewAspect = gameViewPixelWidth / gameViewPixelHeight;

            #if UNITY_EDITOR
            // 获取编辑器 GameView 的分辨率

            if (Editor__GetGameViewSize(out gameViewPixelWidth, out gameViewPixelHeight, out gameViewAspect))
            {
                if (gameViewPixelWidth != 0 && gameViewPixelHeight != 0)
                {
                    resolution.width = (int)gameViewPixelWidth;
                    resolution.height = (int)gameViewPixelHeight;
                }
            }
            #endif 

            return (int)(gameViewAspect * 100) == (int)(aspect * 100);
        }

#if UNITY_EDITOR
		static bool Editor__getGameViewSizeError = false;
		static bool Editor__gameViewReflectionError = false;

		// 尝试获取 GameView 的分辨率
		// 当正确获取到 GameView 的分辨率时，返回 true
		static bool Editor__GetGameViewSize(out float width, out float height, out float aspect)
		{
			try
			{
				Editor__gameViewReflectionError = false;

				System.Type gameViewType = System.Type.GetType("UnityEditor.GameView,UnityEditor");
				System.Reflection.MethodInfo GetMainGameView = gameViewType.GetMethod("GetMainGameView", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
				object mainGameViewInst = GetMainGameView.Invoke(null, null);
				if (mainGameViewInst == null)
				{
					width = height = aspect = 0;
					return false;
				}
				System.Reflection.FieldInfo s_viewModeResolutions = gameViewType.GetField("s_viewModeResolutions", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
				if (s_viewModeResolutions == null)
				{
					System.Reflection.PropertyInfo currentGameViewSize = gameViewType.GetProperty("currentGameViewSize", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
					object gameViewSize = currentGameViewSize.GetValue(mainGameViewInst, null);
					System.Type gameViewSizeType = gameViewSize.GetType();
					int gvWidth = (int)gameViewSizeType.GetProperty("width").GetValue(gameViewSize, null);
					int gvHeight = (int)gameViewSizeType.GetProperty("height").GetValue(gameViewSize, null);
					int gvSizeType = (int)gameViewSizeType.GetProperty("sizeType").GetValue(gameViewSize, null);
					if (gvWidth == 0 || gvHeight == 0)
					{
						width = height = aspect = 0;
						return false;
					}
					else if (gvSizeType == 0)
					{
						width = height = 0;
						aspect = (float)gvWidth / (float)gvHeight;
						return true;
					}
					else
					{
						width = gvWidth; height = gvHeight;
						aspect = (float)gvWidth / (float)gvHeight;
						return true;
					}
				}
				else
				{
					Vector2[] viewModeResolutions = (Vector2[])s_viewModeResolutions.GetValue(null);
					float[] viewModeAspects = (float[])gameViewType.GetField("s_viewModeAspects", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).GetValue(null);
					string[] viewModeStrings = (string[])gameViewType.GetField("s_viewModeAspectStrings", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).GetValue(null);
					if (mainGameViewInst != null
						&& viewModeStrings != null
						&& viewModeResolutions != null && viewModeAspects != null)
					{
						int aspectRatio = (int)gameViewType.GetField("m_AspectRatio", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).GetValue(mainGameViewInst);
						string thisViewModeString = viewModeStrings[aspectRatio];
						if (thisViewModeString.Contains("Standalone"))
						{
							width = UnityEditor.PlayerSettings.defaultScreenWidth; height = UnityEditor.PlayerSettings.defaultScreenHeight;
							aspect = width / height;
						}
						else if (thisViewModeString.Contains("Web"))
						{
							width = UnityEditor.PlayerSettings.defaultWebScreenWidth; height = UnityEditor.PlayerSettings.defaultWebScreenHeight;
							aspect = width / height;
						}
						else
						{
							width = viewModeResolutions[aspectRatio].x; height = viewModeResolutions[aspectRatio].y;
							aspect = viewModeAspects[aspectRatio];
							// this is an error state
							if (width == 0 && height == 0 && aspect == 0)
							{
								return false;
							}
						}
						return true;
					}
				}
			}
			catch (System.Exception e)
			{
				if (Editor__getGameViewSizeError == false)
				{
					Debug.LogError("GameCamera.GetGameViewSize - has a Unity update broken this?\nThis is not a fatal error !\n" + e.ToString());
					Editor__getGameViewSizeError = true;
				}
				Editor__gameViewReflectionError = true;
			}
			width = height = aspect = 0;
			return false;
		}
#endif
	}
//}

