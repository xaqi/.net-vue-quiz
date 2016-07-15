using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wx.DAL;
using Wx.BLL;
using System.Linq;
using System.Diagnostics;
using Wx.Common;

namespace Wx.Web.Tests
{
    [TestClass]
    public class RouletteBLLTest
    {

        [TestMethod]
        public void Resolve()
        {
            var rbll = new RouletteBLL();
            rbll.Resolve(true);
        }

        [TestMethod]
        public void ChannelQr()
        {
            var rbll = new ChannelBLL();
            var a = System.Configuration.ConfigurationSettings.AppSettings.AllKeys.Contains("Environment");
            rbll.GetInviteQrCode(1);
        }

    }
}
