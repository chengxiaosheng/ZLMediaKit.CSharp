using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZLMediaKit.CSharp.Models
{
    public class ProxyPlayer : IDisposable
    {
        private IntPtr _id = IntPtr.Zero;
        private bool disposedValue;

        private ProxyPlayer() { }
        internal MediaInfo MediaInfo { get; set; }

        private PInvoke.on_mk_proxy_player_close mkProxyPlayerClose;

        internal ProxyPlayer(string url, MediaInfo mediaInfo,Dictionary<string,string> options,bool enableHls = false,bool enableMp4 = false)
        {
            if (string.IsNullOrEmpty(url)) throw new InvalidDataException("Url参数错误");
            this.MediaInfo = mediaInfo;
            this._id = PInvoke.ZLMediaKitMethod.mk_proxy_player_create(mediaInfo.VHost, mediaInfo.App, mediaInfo.StreamId, enableHls ? 1 : 0, enableMp4 ? 1 : 0);
            mkProxyPlayerClose = new PInvoke.on_mk_proxy_player_close(mk_proxy_player_close);
            this.SetOptions(options);
            PInvoke.ZLMediaKitMethod.mk_proxy_player_set_on_close(this._id, mkProxyPlayerClose, IntPtr.Zero);
            this.Play(url);
        }

        private Dictionary<string, string> _playerOptions = new Dictionary<string, string>();
        public IReadOnlyDictionary<string, string> PlayerOptions => _playerOptions;

        private void SetOptions(Dictionary<string,string> options)
        {
            if (options == null || options.Count == 0) return;
            foreach(var item in options)
            {
                this._playerOptions[item.Key] = item.Value;
                PInvoke.ZLMediaKitMethod.mk_proxy_player_set_option(this._id, item.Key, item.Value); 
            }
        }

        private void Play(string url)
        {
            PInvoke.ZLMediaKitMethod.mk_proxy_player_play(this._id, url);
        }

        private void mk_proxy_player_close(IntPtr userdata)
        {
            //向上层传递关闭事件
            OnClose?.Invoke();
            //注销当前实例
            this.Dispose(); 
        }

        //
        internal event Action OnClose;



        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                PInvoke.ZLMediaKitMethod.mk_proxy_player_release(this._id);
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~ProxyPlayer()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
