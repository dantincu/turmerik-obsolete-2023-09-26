"use client"; // This is a client component 👈🏽

import { useState, useEffect, useCallback } from "react";
import { ErrorBoundary } from "react-error-boundary";

import { axios } from "@turmerik/axios";
import { app_settings } from "@turmerik/browser"

import { getInstance as getAppSettingsService } from '../services/api/app-settings-service'

declare type AppSettings = app_settings.AppSettingsCore

export default function RootLayout({
  children,
  props
}: {
  children: React.ReactNode,
  props: {
    bodyClassName: string
  }
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
            <body className={props.bodyClassName}>
              <ErrorBoundary fallback={<div>Something went wrong</div>}>
                <div className="trmrk-app">
                  {children}
                </div>
              </ErrorBoundary>
            </body>
        </html>)
    } else {
      return (
          <html lang="en">
            <body className={props.bodyClassName}>
              <div className="trmrk-app-load-error">
                  <main className="flex min-h-screen flex-col items-center justify-between p-24">
                    { appSettingsResponse?.resp ? (<div>
                      <h1>Error while loading the app - { appSettingsResponse.resp.status }</h1> - { appSettingsResponse.resp.statusText }</div>) : (appSettingsResponse?.err ? (<div>
                        <h1>Error while loading the app</h1> { appSettingsResponse.err.message }</div>) : (<div>
                      Oops... something went wrong and the app could not be loaded</div>)) }
                  </main>
                </div>
              </body>
          </html>)
    }
  } else {
    return (
      <html lang="en">
        <body className={props.bodyClassName}>
            <div className="trmrk-app-loading">
              <main className="flex min-h-screen flex-col items-center justify-between p-24"><h1>Loading...</h1></main>
            </div>
          </body>
      </html>)
  }
}
