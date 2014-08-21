using Alta_LED.Data.Init;
using Alta_LED.Data.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Vlc.DotNet.Core;

namespace Alta_LED
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Matrix_Template dataResource;
     
        public void initVLC()
        {
            // Set libvlc.dll and libvlccore.dll directory path
            VlcContext.LibVlcDllsPath = @"VLC";

            // Set the vlc plugins directory path
            VlcContext.LibVlcPluginsPath = @"VLC\plugins";


            // Ignore the VLC configuration file
            VlcContext.StartupOptions.IgnoreConfig = true;

            // Enable file based logging
            VlcContext.StartupOptions.LogOptions.LogInFile = true;

            // Shows the VLC log console (in addition to the applications window)
            // VlcContext.StartupOptions.LogOptions.ShowLoggerConsole = true;

            // Set the log level for the VLC instance
            //   VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.Debug;
            VlcContext.StartupOptions.AddOption("--ffmpeg-hw");
            // Disable showing the movie file name as an overlay
            VlcContext.StartupOptions.AddOption("--no-video-title-show");
            VlcContext.StartupOptions.AddOption("--rtsp-tcp");
            VlcContext.StartupOptions.AddOption("--rtsp-mcast");
            // VlcContext.StartupOptions.AddOption("--rtsp-host=192.168.10.35");
            // VlcContext.StartupOptions.AddOption("--sap-addr=192.168.10.35");
            VlcContext.StartupOptions.AddOption("--rtsp-port=8554");
            VlcContext.StartupOptions.AddOption("--rtp-client-port=8554");
            VlcContext.StartupOptions.AddOption("--sout-rtp-rtcp-mux");
            VlcContext.StartupOptions.AddOption("--rtsp-wmserver");


            VlcContext.StartupOptions.AddOption("--file-caching=18000");
            VlcContext.StartupOptions.AddOption("--sout-rtp-caching=18000");
            VlcContext.StartupOptions.AddOption("--sout-rtp-port=8554");
            VlcContext.StartupOptions.AddOption("--sout-rtp-proto=tcp");
            VlcContext.StartupOptions.AddOption("--network-caching=1000");

            // Pauses the playback of a movie on the last frame
            VlcContext.StartupOptions.AddOption("--play-and-pause");

            // Initialize the VlcContext
            VlcContext.Initialize();
        }
        private void App_Exit(object sender, ExitEventArgs e)
        {
            VlcContext.CloseAll();
            dataResource.saveXml();
            CommonUtilities.myLog.Flush();
            CommonUtilities.myLog.Close();
            
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.initVLC();

            dataResource = new Matrix_Template();
            dataResource=dataResource.loadXml() as Matrix_Template;
            CommonUtilities.setting = new setting();
            CommonUtilities.myLog.Listeners.Add(new XmlWriterTraceListener("output.xml"));

        }
    }
}
