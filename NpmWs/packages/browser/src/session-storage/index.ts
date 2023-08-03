import { AppSettingsCore } from "../app-settings/index"
import { getOrCreate, getOrCreateAsync } from "./session-storage-core"

export class SessionStorageService<TAppSettings extends AppSettingsCore> {
  constructor(public appSettings: TAppSettings) {}

  public getOrCreate<T>(key: string[], factory: () => [boolean, T]) {
    key.splice(0, 0, this.appSettings.TrmrkPrefix)
    return getOrCreate(key, factory)
  }

  public getOrCreateAsync<T>(key: string[], factory: () => Promise<[boolean, T]>) {
    key.splice(0, 0, this.appSettings.TrmrkPrefix)
    return getOrCreateAsync(key, factory)
  }
}
