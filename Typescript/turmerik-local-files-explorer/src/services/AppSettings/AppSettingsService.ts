import axios from "axios";
import { AxiosResponse } from "axios";

import { ApiUri } from "../ApiService/ApiUri";
import { ApiUriRetriever } from "../ApiService/ApiUriRetriever";
import { ApiService, ApiResponse } from "../ApiService/ApiService";
import { AppSettings } from "./AppSettings";

export class AppSettingsService {
  private static _instance: AppSettingsService;

  private _apiUri: ApiUri;
  private _apiService: ApiService;
  private _settings: AppSettings | null;

  private _appSettingsResponse: ApiResponse<AppSettings> | null;

  private constructor() {
    this._apiUri = ApiUriRetriever.instance.apiUri;
    this._apiService = ApiService.instance;
    this._settings = null;
    this._appSettingsResponse = null;
  }

  public static get instance(): AppSettingsService {
    if (!AppSettingsService._instance) {
      AppSettingsService._instance = new AppSettingsService();
    }

    return AppSettingsService._instance;
  }

  public get settings(): AppSettings | null {
    return this._settings;
  }

  public get appSettingsResponse() {
    return this._appSettingsResponse;
  }

  public async init() {
    this._appSettingsResponse = await this._apiService.get(
      this._apiUri.appSettingsRelUri
    );

    if (this._appSettingsResponse.status.isSuccessfull) {
      this._settings = this._appSettingsResponse.data;
    }
  }
}
