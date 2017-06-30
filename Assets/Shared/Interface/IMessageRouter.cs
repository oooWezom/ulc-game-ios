using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    interface IMessageRouter
    {
        void SendAndroidMessage(UnityMessage message);
        void OnMessage(string message);
        void ObtainActivity();
        void ReleaseActivity();
    }
}