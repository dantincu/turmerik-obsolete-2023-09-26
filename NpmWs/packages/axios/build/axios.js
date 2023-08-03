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
    get(uri, data, config = undefined) {
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
