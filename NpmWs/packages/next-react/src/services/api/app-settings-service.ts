import { app_settings } from "@turmerik/browser"

declare type AppSettings = app_settings.AppSettingsCore
declare type AppSettingsService = app_settings.AppSettingsServiceBase<AppSettings>

let instance = null as unknown as AppSettingsService
export const getInstance = () => instance

export const registerInstance = (instn: AppSettingsService) => {
  instance = instn
}
