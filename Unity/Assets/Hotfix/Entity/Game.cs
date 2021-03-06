﻿using UnityEngine;

namespace ETHotfix
{
	public static class Game
	{
		private static EventSystem eventSystem;

		public static EventSystem EventSystem
		{
			get
			{
				return eventSystem ?? (eventSystem = new EventSystem());
			}
		}
		
		private static Scene scene;

		public static Scene Scene
		{
			get
			{
				if (scene != null)
				{
					return scene;
				}
				scene = new Scene() { Name = "ClientH" };
				return scene;
			}
		}

		private static ObjectPool objectPool;

		public static ObjectPool ObjectPool
		{
			get
			{
				if (objectPool != null)
				{
					return objectPool;
				}
				objectPool = new ObjectPool() { Name = "ClientH" };
				return objectPool;
			}
		}

        private static PopupComponent popupComponent;

        /// <summary>
        /// 弹窗组件
        /// </summary>
        public static PopupComponent PopupComponent
        {
            get
            {
                if (popupComponent != null)
                {
                    return popupComponent;
                }

               popupComponent = scene.GetComponent<PopupComponent>();
               return popupComponent;
            }
        }
        
        public static void Close()
		{
			eventSystem = null;
            
            PopupComponent?.Dispose();
            popupComponent = null;

            scene?.Dispose();
			scene = null;
			
			objectPool?.Dispose();
			objectPool = null;

            
        }
	}
}