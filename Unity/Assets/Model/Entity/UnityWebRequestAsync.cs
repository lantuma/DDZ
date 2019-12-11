﻿using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ETModel
{
    public class MyCertificateHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    [ObjectSystem]
	public class UnityWebRequestUpdateSystem : UpdateSystem<UnityWebRequestAsync>
	{
		public override void Update(UnityWebRequestAsync self)
		{
			self.Update();
		}
	}
	
	public class UnityWebRequestAsync : Component
	{
		public UnityWebRequest Request;
        protected MyCertificateHandler cerHandler = null;
        public bool isCancel;

		public ETTaskCompletionSource tcs;
		
		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();

			this.Request?.Dispose();
			this.Request = null;
			this.isCancel = false;
		}

		public float Progress
		{
			get
			{
				if (this.Request == null)
				{
					return 0;
				}
				return this.Request.downloadProgress;
			}
		}

		public ulong ByteDownloaded
		{
			get
			{
				if (this.Request == null)
				{
					return 0;
				}
				return this.Request.downloadedBytes;
			}
		}

		public void Update()
		{
			if (this.isCancel)
			{
				this.tcs.SetException(new Exception($"request error: {this.Request.error}"));
				return;
			}
			
			if (!this.Request.isDone)
			{
				return;
			}
			if (!string.IsNullOrEmpty(this.Request.error))
			{
				this.tcs.SetException(new Exception($"request error: {this.Request.error}"));
				return;
			}

			this.tcs.SetResult();
		}

		public ETTask DownloadAsync(string url)
		{
			this.tcs = new ETTaskCompletionSource();
            this.cerHandler = new MyCertificateHandler();
            url = url.Replace(" ", "%20");
			this.Request = UnityWebRequest.Get(url);
            this.Request.useHttpContinue = true;
            this.Request.chunkedTransfer = false;
            this.Request.certificateHandler = this.cerHandler;
			this.Request.SendWebRequest();
			
			return this.tcs.Task;
		}
	}
}
