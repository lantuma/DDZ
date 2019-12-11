/******************************************************************************************
*         【模块】{ 帮助类 }                                                                                                                      
*         【功能】{ 随机数生成（方便以后使用） }                                                                                                                   
*         【修改日期】{ 2019年11月26日 }                                                                                                                        
*         【贡献者】{ 周瑜 整合 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
#if !SERVER
using UnityEngine;
#endif

namespace ETModel
{
    public static class RandomExHelper
    {
        // 默认生成一个自动分配随机种子的随机数生成器
        private static System.Random globalRandomGenerator = CreateRandom();

        /// <summary>
        /// 创建一个新的全局随机数生成器
        /// </summary>
        public static void GenerateNewRandomGenerator()
        {
            globalRandomGenerator = CreateRandom();
        }

        /// <summary>
        /// 创建一个新的全局随机数生成器，使用指定的种子初始化此对象
        /// </summary>
        public static void GenerateNewRandomGenerator(int seed)
        {
            globalRandomGenerator = CreateRandom(seed);
        }

        /// <summary>
        /// 创建一个产生不重复随机数的随机数生成器实例, 自动分配随机种子
        /// </summary>
        /// <returns></returns>
        private static System.Random CreateRandom()
        {
            long tick = DateTime.Now.Ticks;
            return new System.Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
        }

        /// <summary>
        /// 创建一个产生不重复随机数的随机数生成器实例, 使用指定的种子初始化此对象
        /// </summary>
        /// <returns></returns>
        private static System.Random CreateRandom(int seed)
        {
            return new System.Random(seed);
        }

        /// <summary>
        /// 无符号64位整型
        /// </summary>
        /// <returns></returns>
        public static UInt64 RandomUInt64()
        {
            var bytes = new byte[8];
            globalRandomGenerator.NextBytes(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }

        /// <summary>
        /// 有符号64位整型
        /// </summary>
        /// <returns></returns>
        public static Int64 RandomInt64()
        {
            var bytes = new byte[8];
            globalRandomGenerator.NextBytes(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// 具有最大范围的随机正整型
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RandomNumber(int max)
        {
            int value = globalRandomGenerator.Next(0, max);
            return value;
        }

        /// <summary>
        /// 指定范围内的随机整型
        /// </summary>
        /// <param name="lower">返回的随机数的下界(包含)</param>
        /// <param name="upper">返回的随机数的上界(不包含)</param>
        /// <returns></returns>
        public static int RandomNumber(int lower, int upper)
        {
            int value = globalRandomGenerator.Next(lower, upper);
            return value;
        }

        /// <summary>
        /// [0.0-1.0)范围内的随机浮点数。
        /// 返回一个大于或等于 0.0 且小于 1.0 的随机浮点数。
        /// </summary>
        /// <returns></returns>
        public static float RandomFloat01()
        {
            return (float)globalRandomGenerator.NextDouble();
        }

        /// <summary>
        /// 随机浮点数
        /// </summary>
        /// <returns></returns>
        public static float RandomFloat()
        {
            return RandomInt64() * RandomFloat01();
        }

        /// <summary>
        /// 具有最大范围的随机正浮点数
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float RandomFloat(float max)
        {
            return (float)globalRandomGenerator.NextDouble() * max;
        }

        /// <summary>
        /// 指定范围内的随机浮点数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float RandomFloat(float min, float max)
        {
            if (min < max)
                return (float)globalRandomGenerator.NextDouble() * (max - min) + min;
            else
                return max;
        }
        
        /// <summary>
        /// 随机布尔值
        /// </summary>
        /// <returns></returns>
        public static bool RandomBoolean()
        {
            if (globalRandomGenerator.Next() % 2 == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 测试随机事件在一次独立实验中是否发生
        /// </summary>
        /// <param name="probability"> [0f, 1f] 范围的概率 </param>
        /// <returns> 如果事件发生返回 true, 否则返回 false </returns>
        public static bool Test(float probability)
        {
            return RandomFloat01() < probability;
        }

        /// <summary>
        /// 产生正态分布的随机数
        /// 正态分布随机数落在 μ±σ, μ±2σ, μ±3σ 的概率依次为 68.26%, 95.44%, 99.74%
        /// </summary>
        /// <param name="averageValue"> 正态分布的平均值, 即 N(μ, σ^2) 中的 μ </param>
        /// <param name="standardDeviation"> 正态分布的标准差, 即 N(μ, σ^2) 中的 σ </param>
        /// <returns> 返回正态分布的随机数. 理论值域是 μ±∞ </returns>
        public static float Normal(float averageValue, float standardDeviation)
        {
            //
            // https://en.wikipedia.org/wiki/Box-Muller_transform
            //
            return averageValue + standardDeviation * (float)
            (
                Math.Sqrt(-2 * Math.Log(1 - RandomFloat01())) * Math.Sin(Math.PI * 2 * RandomFloat01())
            );
        }

        /// <summary>
        /// 随机获取List中的一个元素
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Choice<T>(List<T> list)
        {
            if (list.Count == 0)
            {
                return default(T);
            }

            int index = RandomNumber(0, list.Count);
            return list[index];
        }

        /// <summary>
        /// 随机获取Dictionary中的一个元素
        /// </summary>
        /// <param name="dict"></param>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Choice<K, T>(Dictionary<K, T> dict)
        {
            if (dict.Count == 0)
            {
                return default(T);
            }

            int index = RandomNumber(0, dict.Count);
            return dict.Values.ToArray()[index];
        }

#if !SERVER
        /// <summary>
        /// 随机颜色
        /// </summary>
        /// <returns></returns>
        public static Color RandomColor()
        {
            Byte[] temp = new Byte[4];
            globalRandomGenerator.NextBytes(temp);
            return new Color(temp[0], temp[1], temp[2], temp[3]);
        }

        public static Color RandomColor(int alpha)
        {
            Byte[] temp = new Byte[3];
            globalRandomGenerator.NextBytes(temp);

            if (alpha < 0)
                alpha = 0;
            if (alpha > 255)
                alpha = 255;

            return new Color(temp[0], temp[1], temp[2], alpha);
        }

        /// <summary>
        /// 随机三维单位向量/单位球体内的随机坐标点
        /// </summary>
        /// <returns></returns>
        public static Vector3 RandomNormalizedVector3()
        {
            return new Vector3(RandomInt64(), RandomInt64(), RandomInt64()).normalized;
        }

        /// <summary>
        /// 随机三维向量
        /// </summary>
        /// <returns></returns>
        public static Vector3 RandomVector3()
        {
            return new Vector3(RandomFloat(), RandomFloat(), RandomFloat());
        }

        /// <summary>
        /// 具有最大范围的随机三维向量/顶点在原点的长方体内的随机坐标点
        /// </summary>
        /// <param name="xMax">返回的随机数的上界(不包含)</param>
        /// <param name="yMax">返回的随机数的上界(不包含)</param>
        /// <param name="zMax">返回的随机数的上界(不包含)</param>
        /// <returns></returns>
        public static Vector3 RandomVector3(float xMax, float yMax = 0, float zMax = 0)
        {
            return new Vector3(RandomFloat(xMax), RandomFloat(yMax), RandomFloat(zMax));
        }

        /// <summary>
        /// 指定范围的三维向量/长方体内的随机坐标点
        /// </summary>
        /// <param name="xMin">返回的随机数的下界(包含)</param>
        /// <param name="xMax">返回的随机数的上界(不包含)</param>
        /// <param name="yMin">返回的随机数的下界(包含)</param>
        /// <param name="yMax">返回的随机数的上界(不包含)</param>
        /// <param name="zMin">返回的随机数的下界(包含)</param>
        /// <param name="zMax">返回的随机数的上界(不包含)</param>
        /// <returns></returns>
        public static Vector3 RandomVector3(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
        {
            return new Vector3(RandomFloat(xMin, xMax), RandomFloat(yMin, yMax), RandomFloat(zMin, zMax));
        }

        /// <summary>
        /// 随机二维向量
        /// </summary>
        /// <returns></returns>
        public static Vector2 RandomVector2()
        {
            return new Vector2(RandomFloat(), RandomFloat());
        }

        /// <summary>
        /// 具有最大范围的随机二维向量
        /// </summary>
        /// <param name="xMax">返回的随机数的上界(不包含)</param>
        /// <param name="yMax">返回的随机数的上界(不包含)</param>
        /// <returns></returns>
        public static Vector2 RandomVector2(float xMax, float yMax = 0)
        {
            return new Vector2(RandomFloat(xMax), RandomFloat(yMax));
        }

        /// <summary>
        /// 指定范围的随机二维向量
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMin"></param>
        /// <param name="yMax"></param>
        /// <returns></returns>
        public static Vector2 RandomVector2(float xMin, float xMax, float yMin, float yMax)
        {
            return new Vector2(RandomFloat(xMin, xMax), RandomFloat(yMin, yMax));

        }

        /// <summary>
        /// 返回圆圈范围内的随机坐标点
        /// </summary>
        /// <param name="range"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector3 RandomVector3InRangeCircle(float range, float y = 0)
        {
            float length = RandomFloat(0, range);
            float angle = RandomFloat(0, 360);
            return new Vector3(
                (float)(length * Math.Sin(angle * (Math.PI / 180))),
                y,
                (float)(length * Math.Cos(angle * (Math.PI / 180)))
            );
        }
#endif
    }
}