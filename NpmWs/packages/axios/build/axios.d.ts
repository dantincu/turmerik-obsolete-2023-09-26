import { AxiosResponse, AxiosError, AxiosRequestConfig } from "axios";
export declare class ApiResponse<T, D> {
    resp: AxiosResponse<T, D> | null;
    err: AxiosError<T, D> | null;
    constructor(resp: AxiosResponse<T, D> | null, err?: AxiosError<T, D> | null);
}
export declare class ApiComponent {
    apiBaseUri: string;
    constructor(apiBaseUri: string);
    getApiUri(uri: string): string;
    get<T, D>(uri: string, data: D, config?: AxiosRequestConfig<D> | undefined): Promise<ApiResponse<T, D>>;
    post<T, D>(uri: string, data: D, config?: AxiosRequestConfig<D> | undefined): Promise<ApiResponse<T, D>>;
    put<T, D>(uri: string, data: D, config?: AxiosRequestConfig<D> | undefined): Promise<ApiResponse<T, D>>;
    delete<T, D>(uri: string, config?: AxiosRequestConfig<D> | undefined): Promise<ApiResponse<T, D>>;
    patch<T, D>(uri: string, data: D, config?: AxiosRequestConfig<D> | undefined): Promise<ApiResponse<T, D>>;
    private dataRequest;
    private request;
}
