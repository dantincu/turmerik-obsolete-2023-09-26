import { AxiosResponse, AxiosRequestConfig } from "axios";

import * as trmrk_core from "@turmerik/core";
import { ApiComponent } from "./axios";

const { parseUri, uriToString, queryToString } = trmrk_core.uri.uri;

export const getApiProxyBaseUri = (
  apiBaseUri: string,
  proxyBaseRelUri: string
) => {
  const parsedApiBaseUri = parseUri(apiBaseUri);
  parsedApiBaseUri.pathParts = undefined;

  const apiHost = uriToString(parsedApiBaseUri);
  const proxyBaseUri = [apiHost, proxyBaseRelUri].join("/");

  return proxyBaseUri;
};

export class ProxyApiComponent extends ApiComponent {
  constructor(
    apiBaseUri: string,
    public proxyBaseRelUri: string = "api/Proxy",
    public pathQueryParam: string = "api_path"
  ) {
    super(getApiProxyBaseUri(apiBaseUri, proxyBaseRelUri));
  }

  public getApiUri(queryStr: string) {
    const apiUri = [this.apiBaseUri, queryStr].join("?");
    return apiUri;
  }

  async dataRequest<T, D>(
    reqFunc: (
      apiUri: string,
      data: D | undefined,
      config: AxiosRequestConfig<D> | undefined
    ) => Promise<AxiosResponse<T>>,
    uri: string,
    data: D | undefined,
    config: AxiosRequestConfig<D> | undefined = undefined,
    configFactory:
      | ((
          config: AxiosRequestConfig<D> | undefined,
          data: D | undefined
        ) => AxiosRequestConfig<D> | undefined)
      | undefined = undefined
  ) {
    const parsedUri = parseUri(uri);
    const query = parsedUri.query ?? new Map<string, string>();

    const path = encodeURIComponent(parsedUri.pathParts!.join("/"));
    query.set(this.pathQueryParam, path);

    uri = queryToString(query);
    return super.dataRequest<T, D>(reqFunc, uri, data, config, configFactory);
  }
}
