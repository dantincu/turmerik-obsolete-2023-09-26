"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.ApiComponent = exports.ApiResponse = void 0;
const axios_1 = __importDefault(require("axios"));
const core_1 = require("@turmerik/core");
const isNullOrUndef = core_1.obj.isNullOrUndef;
class ApiResponse {
    resp;
    err;
    constructor(resp, err = null) {
        this.resp = resp;
        this.err = err;
        this.init();
    }
    isSuccess = false;
    isInformational = false;
    isRedirection = false;
    isClientError = false;
    isBadRequest = false;
    isUnauthorized = false;
    isForbidden = false;
    isResNotFound = false;
    isRequestTimeout = false;
    isConflict = false;
    isImaTeaPot = false;
    isTooEarly = false;
    isTooManyRequests = false;
    isUnavaillableForLegalReasons = false;
    isServerError = false;
    isInternalServerError = false;
    isBadGateway = false;
    isServiceUnavaillable = false;
    isGatewayTimeout = false;
    isNetworkAuthenticationRequired = false;
    init() {
        if (this.resp) {
            const status = this.resp.status;
            if (status >= 100) {
                this.handleStatus(status);
            }
        }
    }
    handleStatus(status) {
        if (status >= 100) {
            if (status < 200) {
                this.isInformational = true;
            }
            else if (status < 300) {
                this.isSuccess = true;
            }
            else if (status < 400) {
                this.isRedirection = true;
            }
            else if (status < 500) {
                this.handleClientErrorStatus(status);
            }
            else if (status < 600) {
                this.handleServerErrorStatus(status);
            }
        }
    }
    handleClientErrorStatus(status) {
        this.isClientError = true;
        switch (status) {
            case 400:
                this.isBadRequest = true;
                break;
            case 401:
                this.isUnauthorized = true;
                break;
            case 403:
                this.isForbidden = true;
                break;
            case 404:
                this.isResNotFound = true;
                break;
            case 408:
                this.isRequestTimeout = true;
                break;
            case 409:
                this.isConflict = true;
                break;
            case 418:
                this.isImaTeaPot = true;
                break;
            case 425:
                this.isTooEarly = true;
                break;
            case 429:
                this.isTooManyRequests = true;
                break;
            case 451:
                this.isUnavaillableForLegalReasons = true;
                break;
        }
    }
    handleServerErrorStatus(status) {
        this.isServerError = true;
        switch (status) {
            case 500:
                this.isInternalServerError = true;
                break;
            case 502:
                this.isBadGateway = true;
                break;
            case 503:
                this.isServiceUnavaillable = true;
                break;
            case 504:
                this.isGatewayTimeout = true;
                break;
            case 511:
                this.isNetworkAuthenticationRequired = true;
                break;
        }
    }
}
exports.ApiResponse = ApiResponse;
class ApiComponent {
    apiBaseUri;
    constructor(apiBaseUri) {
        this.apiBaseUri = apiBaseUri;
    }
    getApiUri(uri) {
        const apiUri = [this.apiBaseUri, uri].join("/");
        return apiUri;
    }
    get(uri, data = undefined, config = undefined) {
        return this.request((axios_1.default.get), uri, data, config, (config, data) => {
            if (!isNullOrUndef(data)) {
                config ??= {};
                config.data ??= data;
            }
            return config;
        });
    }
    post(uri, data, config = undefined) {
        return this.dataRequest((axios_1.default.post), uri, data, config);
    }
    put(uri, data, config = undefined) {
        return this.dataRequest((axios_1.default.put), uri, data, config);
    }
    delete(uri, config = undefined) {
        return this.request((axios_1.default.delete), uri, undefined, config);
    }
    patch(uri, data, config = undefined) {
        return this.dataRequest((axios_1.default.patch), uri, data, config);
    }
    async dataRequest(reqFunc, uri, data, config = undefined, configFactory = undefined) {
        if (!isNullOrUndef(configFactory)) {
            config = configFactory(config, data);
        }
        const apiUri = this.getApiUri(uri);
        let response;
        try {
            const resp = await reqFunc(apiUri, data, config);
            response = new ApiResponse(resp);
        }
        catch (err) {
            response = new ApiResponse(null, err);
        }
        return response;
    }
    request(reqFunc, uri, data, config = undefined, configFactory = undefined) {
        return this.dataRequest((apiUri, data, config) => reqFunc(apiUri, config), uri, data, config, configFactory);
    }
}
exports.ApiComponent = ApiComponent;
