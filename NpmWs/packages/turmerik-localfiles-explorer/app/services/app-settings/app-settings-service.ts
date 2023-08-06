import { obj } from "@turmerik/core"
import { app_settings } from "@turmerik/browser"
import { axios } from "@turmerik/axios"

import { AppSettings } from "./app-settings"
import { getInstance as getApiService } from "../api/api-service"

const Lazy = obj.Lazy
const ApiComponent = axios.ApiComponent

export class AppSettingsService extends app_settings.AppSettingsServiceBase<AppSettings> {}

const instance = new Lazy<AppSettingsService>(() => new AppSettingsService(getApiService(), "api/AppSettings"))

export const getInstance = () => instance.value
