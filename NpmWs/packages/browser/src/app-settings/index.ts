import { axios } from "@turmerik/axios"

import { AppSettingsCore as AppSettings } from "./app-settings-core"
import { getOrCreateAsync, normalizeKey } from "../session-storage/session-storage-core"

declare type ApiComponent = axios.ApiComponent
declare type ApiResponse<T, D> = axios.ApiResponse<T, D>

export abstract class AppSettingsServiceBase<TAppSettings extends AppSettings> {
  public settings!: TAppSettings
  public response!: ApiResponse<TAppSettings, void>
  public hasErr = false
  public isSuccess = false

  public get initialized() {
    return this.hasErr || this.isSuccess
  }

  private appSettingsSessionStorageKey: string

  constructor(
    private api: ApiComponent,
    private appSettingsUri: string,
    appSettingsSessionStorageKey: string | undefined = undefined,
  ) {
    this.appSettingsSessionStorageKey = appSettingsSessionStorageKey ?? normalizeKey(["trmrk", "app-settings"])
  }

  public async getSettings() {
    if (!this.initialized) {
      await this.loadSettings()
    }

    return this.settings
  }

  public async loadSettings() {
    if (this.appSettingsSessionStorageKey.length > 0) {
      const [hasValue, settings, hasKey] = await getOrCreateAsync<TAppSettings>(this.appSettingsSessionStorageKey, () =>
        this.refreshSettings(),
      )

      this.settings = settings
      this.isSuccess ||= hasKey

      return [hasValue, settings]
    } else {
      const [hasValue, settings] = await this.refreshSettings()
      return [hasValue, settings]
    }
  }

  public async refreshSettings(): Promise<[boolean, TAppSettings]> {
    this.reset()

    this.response = await this.api.get<TAppSettings, void>(this.appSettingsUri)
    this.isSuccess = this.response.isSuccess

    if (this.isSuccess) {
      this.settings = this.response.resp?.data as TAppSettings
    } else {
      this.hasErr = true
    }

    return [this.isSuccess, this.settings]
  }

  public reset() {
    this.hasErr = false
    this.isSuccess = false
    this.settings = null as unknown as TAppSettings
    this.response = null as unknown as ApiResponse<TAppSettings, void>
  }
}

export type AppSettingsCore = AppSettings
