import axios from "axios"
import { AxiosResponse, AxiosError, AxiosRequestConfig } from "axios"

import { obj as core } from "@turmerik/core"
const isNullOrUndef = core.isNullOrUndef

export class ApiResponse<T, D> {
  constructor(
    public resp: AxiosResponse<T, D> | null,
    public err: AxiosError<T, D> | null = null,
  ) {}
}

export class ApiComponent {
  constructor(public apiBaseUri: string) {}

  public getApiUri(uri: string) {
    const apiUri = [this.apiBaseUri, uri].join("/")
    return apiUri
  }

  public get<T, D>(uri: string, data: D, config: AxiosRequestConfig<D> | undefined = undefined) {
    return this.request(
      axios.get<T, AxiosResponse<T>, D>,
      uri,
      data,
      config,
      (config: AxiosRequestConfig<D> | undefined, data: D) => {
        if (!isNullOrUndef(data)) {
          config ??= {}
          config.data ??= data
        }

        return config
      },
    )
  }

  public post<T, D>(uri: string, data: D, config: AxiosRequestConfig<D> | undefined = undefined) {
    return this.dataRequest(axios.post<T, AxiosResponse<T>, D>, uri, data, config)
  }

  public put<T, D>(uri: string, data: D, config: AxiosRequestConfig<D> | undefined = undefined) {
    return this.dataRequest(axios.put<T, AxiosResponse<T>, D>, uri, data, config)
  }

  public delete<T, D>(uri: string, config: AxiosRequestConfig<D> | undefined = undefined) {
    return this.request(axios.delete<T, AxiosResponse<T>, D>, uri, undefined as D, config)
  }

  public patch<T, D>(uri: string, data: D, config: AxiosRequestConfig<D> | undefined = undefined) {
    return this.dataRequest(axios.patch<T, AxiosResponse<T>, D>, uri, data, config)
  }

  private async dataRequest<T, D>(
    reqFunc: (apiUri: string, data: D, config: AxiosRequestConfig<D> | undefined) => Promise<AxiosResponse<T>>,
    uri: string,
    data: D,
    config: AxiosRequestConfig<D> | undefined = undefined,
    configFactory:
      | ((config: AxiosRequestConfig<D> | undefined, data: D) => AxiosRequestConfig<D> | undefined)
      | undefined = undefined,
  ) {
    if (!isNullOrUndef(configFactory)) {
      config = configFactory!(config, data)
    }

    const apiUri = this.getApiUri(uri)
    let response: ApiResponse<T, D>

    try {
      const resp = await reqFunc(apiUri, data, config)
      response = new ApiResponse<T, D>(resp)
    } catch (err) {
      response = new ApiResponse<T, D>(null, err as AxiosError<T, D>)
    }

    return response
  }

  private request<T, D>(
    reqFunc: (apiUri: string, config: AxiosRequestConfig<D> | undefined) => Promise<AxiosResponse<T>>,
    uri: string,
    data: D,
    config: AxiosRequestConfig<D> | undefined = undefined,
    configFactory:
      | ((config: AxiosRequestConfig<D> | undefined, data: D) => AxiosRequestConfig<D> | undefined)
      | undefined = undefined,
  ) {
    return this.dataRequest<T, D>(
      (apiUri: string, data: D, config: AxiosRequestConfig<D> | undefined) => reqFunc(apiUri, config),
      uri,
      data,
      config,
      configFactory,
    )
  }
}
