﻿using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MyWeatherApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>

        /*
         * 判断是否被挂起
         */
        public bool isSuspended = false;

        // 用单例模式解决“对象引用对于非静态的字段、方法或属性是必须的”的报错
        public class connection
        {
            public SQLiteConnection conn;
            private static connection ptr;
            private connection() { }
            public static connection getInstance()
            {
                if (ptr == null)
                {
                    ptr = new connection();
                }
                return ptr;
            }
        }

        // 使用静态变量
        public static SQLiteConnection conn2;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += OnResuming;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // to complete
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            NewPage2.userName = "";
            MainPage.currentCity = "";
            NewPage1.currentCity = "";
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    if (ApplicationData.Current.LocalSettings.Values.ContainsKey("NavigationState"))
                    {
                        rootFrame.SetNavigationState((string)ApplicationData.Current.LocalSettings.Values["NavigationState"]);
                    }
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }

            LoadDatabase();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            isSuspended = true;
            //TODO: Save application state and stop any background activity
            Frame frame = Window.Current.Content as Frame;
            ApplicationData.Current.LocalSettings.Values["NavigationState"] = frame.GetNavigationState();
            deferral.Complete();
        }

        private void OnResuming(object sender, object e)
        {
            isSuspended = false;
        }

        private void LoadDatabase()
        {
            connection.getInstance().conn = new SQLiteConnection("myFavouriteCity.db");
            string sql = @"CREATE TABLE IF NOT EXISTS
                            Favourites (Id      VARCHAR( 500 ) PRIMARY KEY NOT NULL,
                                      UserName     VARCHAR( 500 ),
                                      City    VARCHAR( 500 ));";
            using (var statement = connection.getInstance().conn.Prepare(sql))
            {
                statement.Step();
            }

            conn2 = new SQLiteConnection("userInfo.db");
            string sql2 = @"CREATE TABLE IF NOT EXISTS
                            Info (UserName      VARCHAR( 500 ) PRIMARY KEY NOT NULL,
                                  Password         VARCHAR( 500 ));";
            using (var statement2 = conn2.Prepare(sql2))
            {
                statement2.Step();
            }
            
        }

        
    }
}
