import axios from "axios"
import { AxiosResponse, AxiosError, AxiosRequestConfig } from "axios"

import { obj as core } from "@turmerik/core"
const isNullOrUndef = core.isNullOrUndef

export class ApiResponse<T, D> {
  constructor(
    public resp: AxiosResponse<T, D> | null,
    public err: AxiosError<T, D> | null = null,
  ) {
    this.init()
  }

  public isSuccess = false

  public isInformational = false
  public isRedirection = false

  public isClientError = false
  public isBadRequest = false
  public isUnauthorized = false
  public isForbidden = false
  public isResNotFound = false
  public isRequestTimeout = false
  public isConflict = false
  public isImaTeaPot = false
  public isTooEarly = false
  public isTooManyRequests = false
  public isUnavaillableForLegalReasons = false

  public isServerError = false
  public isInternalServerError = false
  public isBadGateway = false
  public isServiceUnavaillable = false
  public isGatewayTimeout = false
  public isNetworkAuthenticationRequired = false

  private init() {
    if (this.resp) {
      const status = this.resp.status

      if (status >= 100) {
        this.handleStatus(status)
      }
    }
  }

  private handleStatus(status: number) {
    if (status >= 100) {
      if (status < 200) {
        this.isInformational = true
      } else if (status < 300) {
        this.isSuccess = true
      } else if (status < 400) {
        this.isRedirection = true
      } else if (status < 500) {
        this.handleClientErrorStatus(status)
      } else if (status < 600) {
        this.handleServerErrorStatus(status)
      }
    }
  }

  private handleClientErrorStatus(status: number) {
    this.isClientError = true

    switch (status) {
      case 400:
        this.isBadRequest = true
        break
      case 401:
        this.isUnauthorized = true
        break
      case 403:
        this.isForbidden = true
        break
      case 404:
        this.isResNotFound = true
        break
      case 408:
        this.isRequestTimeout = true
        break
      case 409:
        this.isConflict = true
        break
      case 418:
        this.isImaTeaPot = true
        break
      case 425:
        this.isTooEarly = true
        break
      case 429:
        this.isTooManyRequests = true
        break
      case 451:
        this.isUnavaillableForLegalReasons = true
        break
    }
  }

  private handleServerErrorStatus(status: number) {
    this.isServerError = true

    switch (status) {
      case 500:
        this.isInternalServerError = true
        break
      case 502:
        this.isBadGateway = true
        break
      case 503:
        this.isServiceUnavaillable = true
        break
      case 504:
        this.isGatewayTimeout = true
        break
      case 511:
        this.isNetworkAuthenticationRequired = true
        break
    }
  }
}

export class ApiComponent {
  constructor(public apiBaseUri: string) {}

  public getApiUri(uri: string) {
    const apiUri = [this.apiBaseUri, uri].join("/")
    return apiUri
  }

  public get<T, D>(
    uri: string,
    data: D | undefined = undefined,
    config: AxiosRequestConfig<D> | undefined = undefined,
  ) {
    return this.request(
      axios.get<T, AxiosResponse<T>, D>,
      uri,
      data,
      config,
      (config: AxiosRequestConfig<D> | undefined, data: D | undefined) => {
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
    reqFunc: (
      apiUri: string,
      data: D | undefined,
      config: AxiosRequestConfig<D> | undefined,
    ) => Promise<AxiosResponse<T>>,
    uri: string,
    data: D | undefined,
    config: AxiosRequestConfig<D> | undefined = undefined,
    configFactory:
      | ((config: AxiosRequestConfig<D> | undefined, data: D | undefined) => AxiosRequestConfig<D> | undefined)
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
    data: D | undefined,
    config: AxiosRequestConfig<D> | undefined = undefined,
    configFactory:
      | ((config: AxiosRequestConfig<D> | undefined, data: D | undefined) => AxiosRequestConfig<D> | undefined)
      | undefined = undefined,
  ) {
    return this.dataRequest<T, D>(
      (apiUri: string, data: D | undefined, config: AxiosRequestConfig<D> | undefined) => reqFunc(apiUri, config),
      uri,
      data,
      config,
      configFactory,
    )
  }
}
