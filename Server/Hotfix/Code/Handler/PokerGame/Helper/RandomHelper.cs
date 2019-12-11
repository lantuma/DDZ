using System;
using System.Diagnostics;
using System.Text;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class RandomHelper
    {
        public static int GenerateRandomCode(int length)
        {
            var result = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(0, 10));
            }
            return int.Parse(result.ToString());
        }

        public static string GenerateRandomOrde(int length)
        {
            var result = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(0, 10));
            }
            return result.ToString();
        }


        public static Random GetRandom()
        {
            var buffer = Guid.NewGuid().ToByteArray();
            return new Random(BitConverter.ToInt32(buffer, 0));
        }


        public static int GenerateRandomPlayerId(int length)
        {
            var result = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(i==0?1: 0,10));
            }
            return int.Parse(result.ToString());

        }
    }
 }
