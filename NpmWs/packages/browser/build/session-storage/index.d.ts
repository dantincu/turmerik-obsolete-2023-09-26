import { AppSettingsCore } from "../app-settings/index";
export declare class SessionStorageService<TAppSettings extends AppSettingsCore> {
    appSettings: TAppSettings;
    constructor(appSettings: TAppSettings);
    getOrCreate<T>(key: string[], factory: () => [boolean, T]): [boolean, T, boolean];
    getOrCreateAsync<T>(key: string[], factory: () => Promise<[boolean, T]>): Promise<[boolean, T, boolean]>;
}
