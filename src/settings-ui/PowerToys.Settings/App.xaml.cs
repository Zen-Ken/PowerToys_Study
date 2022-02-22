﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using Microsoft.PowerToys.Settings.UI.Library.Telemetry.Events;
using Microsoft.PowerToys.Telemetry;

namespace PowerToys.Settings
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        private MainWindow settingsWindow;

        public bool ShowOobe { get; set; }

        public bool ShowScoobe { get; set; }

        public Type StartupPage { get; set; } = typeof(Microsoft.PowerToys.Settings.UI.Views.GeneralPage);

        public void OpenSettingsWindow(Type type)
        {
            if (settingsWindow == null)
            {
                settingsWindow = new MainWindow();
            }
            else if (settingsWindow.WindowState == WindowState.Minimized)
            {
                settingsWindow.WindowState = WindowState.Normal;
            }

            settingsWindow.Show();
            settingsWindow.NavigateToSection(type);
        }

        private void InitHiddenSettingsWindow()
        {
            settingsWindow = new MainWindow();

            Utils.ShowHide(settingsWindow);
            Utils.CenterToScreen(settingsWindow);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (!ShowOobe && !ShowScoobe)
            {
                settingsWindow = new MainWindow();
                settingsWindow.Show();
                settingsWindow.NavigateToSection(StartupPage);
            }
            else
            {
                // Create the Settings window so that it's fully initialized and
                // it will be ready to receive the notification if the user opens
                // the Settings from the tray icon.
                InitHiddenSettingsWindow();
                if (ShowOobe)
                {
                    PowerToysTelemetry.Log.WriteEvent(new OobeStartedEvent());
                    OobeWindow oobeWindow = new OobeWindow((int)Microsoft.PowerToys.Settings.UI.OOBE.Enums.PowerToysModulesEnum.Overview);
                    oobeWindow.Show();
                }
                else if (ShowScoobe)
                {
                    PowerToysTelemetry.Log.WriteEvent(new ScoobeStartedEvent());
                    OobeWindow scoobeWindow = new OobeWindow((int)Microsoft.PowerToys.Settings.UI.OOBE.Enums.PowerToysModulesEnum.WhatsNew);
                    scoobeWindow.Show();
                }
            }
        }
    }
}
