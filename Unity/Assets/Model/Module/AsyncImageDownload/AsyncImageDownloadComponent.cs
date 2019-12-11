/******************************************************************************************
*         【模块】{ 异步图片加载模块 }                                                                                                                      
*         【功能】{ 动态加载图片,如（大厅背景图，广告页图片）}                                                                                                                   
*         【修改日期】{ 2019年11月25日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class AsyncImageDownloadComponentAwakeSystem : AwakeSystem<AsyncImageDownloadComponent>
    {
        public override void Awake(AsyncImageDownloadComponent self)
        {
            self.Awake();
        }

    }

    public class AsyncImageDownloadComponent : Component
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static AsyncImageDownloadComponent instance;
        
        /// <summary>
        /// 本地图片保存路径
        /// </summary>
        private string imageCacheRootPath = Application.persistentDataPath + "/ImageCache/";

        /// <summary>
        /// 缓存在内存的texture,重复利用，key:url value:本地文件的绝对路径
        /// </summary>
        private Dictionary<string, WeakReference> textureCache;

        public void Awake()
        {
            textureCache = new Dictionary<string, WeakReference>();

            if (!Directory.Exists(imageCacheRootPath))
            {
                Directory.CreateDirectory(imageCacheRootPath);
            }
            
            //Log.Debug("Image Save Path: " + imageCacheRootPath);

            instance = this;
        }

        /// <summary>
        /// 根据url加载对应图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="image"></param>
        /// <param name="action"></param>
        /// <param name="needCache"></param>
        public void SetAsyncImage(string url, Image texture, Action action = null, bool needCache = true)
        {
            if (string.IsNullOrEmpty(url))
            {
                //Log.Debug("[SetAsyncImage] url is null.");

                return;
            }

            //只有需要缓存时才从内存中查找
            if (needCache)
            {
                //从内存获取
                if (getFromCache(imageCacheRootPath + url.GetHashCode(), texture))
                {
                    if (action != null)
                    {
                        action();
                    }

                    return;
                }
            }

            //判断是否是第一次加载图片(以url的hascode作为文件名)
            if (!File.Exists(imageCacheRootPath + url.GetHashCode()))
            {
                //如果之前不存在缓存文件
                texture.StopCoroutine("DownloadImage");

                texture.StartCoroutine(DownloadImage(url, texture, action, needCache));
            }
            else
            {
                //从本地加载
                texture.StopCoroutine("LoadLocalImage");

                texture.StartCoroutine(LoadLocalImage(imageCacheRootPath + url.GetHashCode(), texture, action, needCache));
            }
        }

        /// <summary>
        /// 加载本地图片，缓存到内存中
        /// </summary>
        /// <param name="path">本地文件绝对路径</param>
        /// <param name="texture">Image</param>
        /// <param name="action">加载完成时的回调</param>
        public void SetLocalImage(string path, Image texture, Action action = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                Log.Debug("[SetLocalImage] path is null.");

                return;
            }

            //从内存获取
            if (getFromCache(path, texture))
            {
                if (action != null)
                {
                    action();
                }

                return;
            }

            //先判断该文件是否存在，不存在直接返回。
            FileInfo fi = new FileInfo(path);

            if (!fi.Exists)
            {
                Log.Error("Path is Not Exists! " + path);

                return;
            }

            texture.StopCoroutine("LoadLocalImage");

            texture.StartCoroutine(LoadLocalImage(path, texture, action));
        }

        /// <summary>
        /// 清除指定key的缓存
        /// </summary>
        /// <param name="filePath"></param>
        public void clearCache(string filePath)
        {
            if (textureCache.ContainsKey(filePath))
            {
                textureCache.Remove(filePath);
            }
        }

        /// <summary>
        /// 异步加载远程图片
        /// </summary>
        /// <param name="url">图片url</param>
        /// <param name="texture">image</param>
        /// <param name="action">加载完成时的回调</param>
        /// <param name="needCache">是否需要缓存到内存中</param>
        /// <returns></returns>
        IEnumerator DownloadImage(string url, Image texture, Action action = null, bool needCache = true)
        {
            int filename = url.GetHashCode();

            //Log.Debug("[DownloadImage]downloading new image : " + filename);

            //如果界面频繁快速切换，之前的图片根本不需要加载，所有这里稍微延迟一会儿再请求网络.
            yield return new WaitForSeconds(0.008f);

            WWW www = new WWW(url);

            yield return www;

            if (www.error != null)
            {
                //Log.Warning("[DownloadImage]www.error :" + www.error);

                if (action != null)
                {
                    action();
                }
            }
            else
            {
                //Log.Debug("[DownloadImage]download ok : " + filename);

                Texture2D image = www.texture;

                //图片必须是JPG 或者 PNG
                //如果image为空，或者为红色问号(无效图片)
                if (image == null || (image.width == 8 && image.height == 8))
                {
                    Log.Error("[DownloadImage]www.texture is null or a question mark,return.");

                    if (action != null)
                    {
                        action();
                    }
                }
                else
                {
                    image.wrapMode = TextureWrapMode.Clamp;

                    //可能存在性能问题，建议用RawImage组件
                    //RawImageObject.texture = image;

                    Sprite sprite = CreateSprite(image);

                    texture.sprite = sprite;

                    if (needCache)
                    {
                        textureCache[imageCacheRootPath + url.GetHashCode()] = new WeakReference(sprite);
                    }

                    if (action != null)
                    {
                        action();
                    }

                    //将图片保存至缓存路径
                    byte[] pngData = www.bytes;

                    //Log.Debug("[DwonloadImage] " + filename + ",size: " + (pngData.Length / 1024) + "KB");

                    File.WriteAllBytes(imageCacheRootPath + filename, pngData);
                }
            }
        }

        /// <summary>
        /// 异步加载本地图片
        /// </summary>
        /// <param name="filePath">本地文件绝对路径</param>
        /// <param name="texture">Image</param>
        /// <param name="action">加载完成时的回调</param>
        /// <param name="needCache">是否需要缓存到内存中</param>
        /// <returns></returns>
        IEnumerator LoadLocalImage(string filePath, Image texture, Action action = null, bool needCache = true)
        {
            //Log.Debug("getting local image :" + filePath);

            yield return new WaitForSeconds(0.008f);

            WWW www = new WWW("file:///" + filePath);

            yield return www;

            if (www.error != null)
            {
                Log.Warning("[LoadLocalImage] www.error : " + www.error);

                DeleteFile(filePath);
            }
            else
            {
                Texture2D image = www.texture;

                if (image == null || (image.width == 8 && image.height == 8))
                {
                    Log.Error("www.texture is null or a question mark,delete it.");

                    DeleteFile(filePath);
                }
                else
                {
                    image.wrapMode = TextureWrapMode.Clamp;

                    Sprite sprite = CreateSprite(image);

                    //直接贴图
                    texture.sprite = sprite;

                    if (needCache)
                    {
                        textureCache[filePath] = new WeakReference(sprite);
                    }
                }
            }

            if (action != null)
            {
                action();
            }
        }

        /// <summary>
        /// 从textureCache读取
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="texture"></param>
        /// <returns></returns>
        private bool getFromCache(string localPath, Image texture)
        {
            if (textureCache.ContainsKey(localPath))
            {
                WeakReference wr = textureCache[localPath];

                if (wr != null && wr.IsAlive)
                {
                    texture.sprite = (Sprite)wr.Target;

                    //有可能在获取到的瞬间被回收了
                    if (texture.sprite == null)
                    {
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="filePath"></param>
        private void DeleteFile(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);

            if (fi.Exists)
            {
                Log.Debug("[DeleteFile] delete the file: " + filePath);

                fi.Delete();
            }
        }

        /// <summary>
        /// 创建精灵
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        private Sprite CreateSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }

        /// <summary>
        /// 释放
        /// </summary>
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.textureCache.Clear();

            base.Dispose();
        }
    }
}
