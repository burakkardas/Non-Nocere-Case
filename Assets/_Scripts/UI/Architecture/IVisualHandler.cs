using System;

namespace _Scripts.UI.Architecture
{
    public interface IVisualHandler
    {
        void SetCulling();
        void OnClickListen(Action callBack);
        void OnClickListen(ref Action<string> callBack,string contentId);
        void OnClickListen(string buttonName, ref Action<string> callBack, string contentId);
    }
}