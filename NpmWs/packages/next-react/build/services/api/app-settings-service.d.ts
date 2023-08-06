import { app_settings } from "@turmerik/browser";
declare type AppSettings = app_settings.AppSettingsCore;
declare type AppSettingsService = app_settings.AppSettingsServiceBase<AppSettings>;
export declare const getInstance: () => AppSettingsService;
export declare const registerInstance: (instn: AppSettingsService) => void;
export {};
