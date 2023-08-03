import { axios } from "@turmerik/axios";
import { AppSettingsCore as AppSettings } from "./app-settings-core";
declare type ApiComponent = axios.ApiComponent;
declare type ApiResponse<T, D> = axios.ApiResponse<T, D>;
export declare abstract class AppSettingsServiceBase<TAppSettings extends AppSettings> {
    private api;
    private appSettingsUri;
    settings: TAppSettings;
    response: ApiResponse<TAppSettings, void>;
    hasErr: boolean;
    isSuccess: boolean;
    get initialized(): boolean;
    private appSettingsSessionStorageKey;
    constructor(api: ApiComponent, appSettingsUri: string, appSettingsSessionStorageKey?: string | undefined);
    getSettings(): Promise<TAppSettings>;
    loadSettings(): Promise<(boolean | TAppSettings)[]>;
    refreshSettings(): Promise<[boolean, TAppSettings]>;
    reset(): void;
}
export type AppSettingsCore = AppSettings;
export {};
