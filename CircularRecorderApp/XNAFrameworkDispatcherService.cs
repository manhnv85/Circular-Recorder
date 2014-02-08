using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Xna.Framework;

namespace CircularRecorderApp
{
    public class XNAFrameworkDispatcherService : IApplicationService
    {
        public DispatcherTimer frameworkDispatcherTimer;

        public XNAFrameworkDispatcherService()
        {
            frameworkDispatcherTimer = new DispatcherTimer();
            frameworkDispatcherTimer.Interval = TimeSpan.FromTicks(333333);
            frameworkDispatcherTimer.Tick += frameworkDispatcherTimer_Tick;
            FrameworkDispatcher.Update();
        }

        void frameworkDispatcherTimer_Tick(object sender, EventArgs e)
        { FrameworkDispatcher.Update(); }

        void IApplicationService.StartService(ApplicationServiceContext context)
        { this.frameworkDispatcherTimer.Start(); }

        void IApplicationService.StopService() { frameworkDispatcherTimer.Stop(); }
    }
}
