import axios from "axios";
import { AxiosResponse } from "axios";

import { ApiUriRetriever } from "../ApiService/ApiUriRetriever";
import { ApiUri } from "../ApiService/ApiUri";
import { ApiResponseStatus, getApiResponseStatus } from "./ApiResponseStatus";

export interface ApiResponse<T> {
  status: ApiResponseStatus;
  response: AxiosResponse<T> | null;
  data: T | null;
  err: any | null;
}

export class ApiService {
  private static _instance: ApiService;

  private _apiUri: ApiUri;
  private _apiBaseUri: string;

  private constructor() {
    this._apiUri = ApiUriRetriever.instance.apiUri;
    this._apiBaseUri = this._apiUri.apiBaseUri;
  }

  public static get instance(): ApiService {
    if (!ApiService._instance) {
      ApiService._instance = new ApiService();
    }

    return ApiService._instance;
  }

  public async get<T, D>(
    relUri: string,
    data: D | undefined = undefined
  ): Promise<ApiResponse<T>> {
    const apiResponse = await this.request(
      relUri,
      data,
      (url) => axios.get<T>(url),
      (url, obj) =>
        axios.get<T>(url, {
          data: obj,
        })
    );

    return apiResponse;
  }

  public async post<T, D>(
    relUri: string,
    data: D | undefined = undefined
  ): Promise<ApiResponse<T>> {
    const apiResponse = await this.request(
      relUri,
      data,
      (url) => axios.post<T>(url),
      (url, obj) =>
        axios.post<T>(url, {
          data: obj,
        })
    );

    return apiResponse;
  }

  public async put<T, D>(
    relUri: string,
    data: D | undefined = undefined
  ): Promise<ApiResponse<T>> {
    const apiResponse = await this.request(
      relUri,
      data,
      (url) => axios.put<T>(url),
      (url, obj) =>
        axios.put<T>(url, {
          data: obj,
        })
    );

    return apiResponse;
  }

  public async delete<T, D>(
    relUri: string,
    data: D | undefined = undefined
  ): Promise<ApiResponse<T>> {
    const apiResponse = await this.request(
      relUri,
      data,
      (url) => axios.delete<T>(url),
      (url, obj) =>
        axios.delete<T>(url, {
          data: obj,
        })
    );

    return apiResponse;
  }

  public async patch<T, D>(
    relUri: string,
    data: D | undefined = undefined
  ): Promise<ApiResponse<T>> {
    const apiResponse = await this.request(
      relUri,
      data,
      (url) => axios.patch<T>(url),
      (url, obj) =>
        axios.patch<T>(url, {
          data: obj,
        })
    );

    return apiResponse;
  }

  private getUrl(relUri: string) {
    const url = [this._apiBaseUri, relUri].join("/");
    return url;
  }

  private async request<T, D>(
    relUri: string,
    data: D | undefined,
    reqFunc: (url: string) => Promise<AxiosResponse<T, D>>,
    dataReqFunc:
      | ((url: string, data: D) => Promise<AxiosResponse<T, D>>)
      | null = null
  ) {
    const url = this.getUrl(relUri);

    let response: AxiosResponse<T> | null = null;
    let error: any | null = null;

    try {
      if (dataReqFunc) {
        response = await dataReqFunc(url, data as D);
      } else {
        response = await reqFunc(url);
      }
    } catch (err) {
      error = err;
    }

    const apiResponse = {
      status: getApiResponseStatus(response?.status ?? null),
      response: response,
      err: error,
      data: response?.data ?? null,
    } as ApiResponse<T>;

    return apiResponse;
  }
}
