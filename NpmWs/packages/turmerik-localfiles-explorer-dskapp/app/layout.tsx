"use client"; // This is a client component 👈🏽

import './globals.css'
import type { Metadata } from 'next'
import { Inter } from 'next/font/google'
import { useState, useEffect, useCallback } from "react";

import { axios } from "@turmerik/axios";
import { getInstance as getAppSettingsService } from './services/app-settings/app-settings-service'
import { AppSettings } from "./services/app-settings/app-settings";

const inter = Inter({ subsets: ['latin'] })

export const metadata: Metadata = {
  title: 'Create Next App',
  description: 'Generated by create next app',
}

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  const [appSettings, setAppSettings] = useState<AppSettings>()
  const [appSettingsResponse, setAppSettingsResponse] = useState<axios.ApiResponse<AppSettings, void>>();
  const [hasAppSettingsResponse, setHasAppSettingsResponse] = useState(false);
  const [hasAppSettings, setHasAppSettings] = useState(false);
  const appSettingsService = getAppSettingsService();

  const getAppSettings = useCallback(
    () => appSettingsService.getSettings().then(
      settings => {
      setHasAppSettings(appSettingsService.isSuccess)

      if (appSettingsService.isSuccess) {
         setAppSettings(settings);
       } else {
         setAppSettingsResponse(appSettingsService.response);
       }

       setHasAppSettingsResponse(true);
     }), [appSettingsService]);

  useEffect(() => {
    getAppSettings();
  }, [appSettings]);

  if (hasAppSettingsResponse) {
    if (hasAppSettings) {
      return (
        <html lang="en">
            <body className={inter.className}>
              <div className="trmrk-app">{children}</div>
            </body>
        </html>)
    } else {
      return (
          <html lang="en">
            <body className={inter.className}>
              <div className="trmrk-app-load-error">
                  <main className="flex min-h-screen flex-col items-center justify-between p-24">
                    { appSettingsResponse?.resp ? (<div>
                      <h1>Error while loading the app - { appSettingsResponse.resp.status }</h1> - { appSettingsResponse.resp.statusText }</div>) : (appSettingsResponse?.err ? (<div>
                        <h1>Error while loading the app</h1> { appSettingsResponse.err.message }</div>) : (<div>
                      <h2>Oops... something went wrong and the app could not be loaded</h2></div>)) }
                  </main>
                </div>
              </body>
          </html>)
    }
  } else {
    return (
      <html lang="en">
        <body className={inter.className}>
            <div className="trmrk-app-loading">
              <main className="flex min-h-screen flex-col items-center justify-between p-24"><h2>Loading...</h2></main>
            </div>
          </body>
      </html>)
  }
}
