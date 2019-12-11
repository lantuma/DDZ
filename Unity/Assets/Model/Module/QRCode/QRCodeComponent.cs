 /******************************************************************************************
*         【模块】{ QRCode模块 }                                                                                                                      
*         【功能】{ 动态生成二维码 }                                                                                                                   
*         【修改日期】{ 2019年5月6日 }                                                                                                                        
*         【贡献者】{ 周瑜 }                                                                                                                
*                                                                                                                                        
 ******************************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.Common;

namespace ETModel
{
    [ObjectSystem]
    public class QRCodeComponentAwakeSystem : AwakeSystem<QRCodeComponent>
    {
        public override void Awake(QRCodeComponent self)
        {
            self.Awake();
        }
    }

    public class QRCodeComponent : Component
    {
        public void Awake()
        {
            //Log.Debug("QRCode组件初始化>>>>>>");
        }

        /// <summary>
        /// 生成二维码精灵
        /// </summary>
        /// <param name="qrCodeStr"></param>
        /// <returns></returns>
        public Sprite CreateQRCode(string qrCodeStr)
        {
            Texture2D qrCodeTexture = new Texture2D(256, 256);

            var textForEncoding = qrCodeStr;

            if (!string.IsNullOrEmpty(textForEncoding))
            {
                var color32 = Encode(textForEncoding, qrCodeTexture.width, qrCodeTexture.height);

                qrCodeTexture.SetPixels32(color32);

                qrCodeTexture.Apply();
            }

            return Sprite.Create(qrCodeTexture, new Rect(0, 0, qrCodeTexture.width, qrCodeTexture.height), new Vector2(0.5f, 0.5f));
        }

        /// <summary>
        /// 生成任意尺寸的正方形
        /// </summary>
        /// <param name="qrCodeStr"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Sprite GenerateQRImage(string qrCodeStr, int width=256, int height=256)
        {
            //编码成color32

            MultiFormatWriter writer = new MultiFormatWriter();

            Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>();

            hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");

            hints.Add(EncodeHintType.MARGIN, 1);

            hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.M);

            BitMatrix bitMatrix = writer.encode(qrCodeStr, BarcodeFormat.QR_CODE, width, height, hints);

            //转成texture2d

            int w = bitMatrix.Width;

            int h = bitMatrix.Height;

            Texture2D texture = new Texture2D(w, h);

            for (int x = 0; x < h; x++)
            {
                for (int y = 0; y < w; y++)
                {
                    if (bitMatrix[x, y])
                    {
                        texture.SetPixel(y, x, Color.black);
                    }
                    else
                    {
                        texture.SetPixel(y, x, Color.white);
                    }
                }
            }

            texture.Apply();

            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        public Color32[] Encode(string textForEncoding, int width, int height)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,

                Options = new ZXing.Common.EncodingOptions
                {
                    Height = height,

                    Width = width,

                    Margin = 1
                }
            };

            return writer.Write(textForEncoding);
        }
        /// <summary>
        /// 识别二维码
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public string DecodeQR(Color32[] data, int width, int height)
        {
            BarcodeReader br = new BarcodeReader();

            Result result = br.Decode(data, width, height);

            if (result != null)
            {
                return result.Text;
            }

            return string.Empty;
        }

    }
}
