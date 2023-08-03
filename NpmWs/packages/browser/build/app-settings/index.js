"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AppSettingsServiceBase = void 0;
const session_storage_core_1 = require("../session-storage/session-storage-core");
class AppSettingsServiceBase {
    api;
    appSettingsUri;
    settings;
    response;
    hasErr = false;
    isSuccess = false;
    get initialized() {
        return this.hasErr || this.isSuccess;
    }
    appSettingsSessionStorageKey;
    constructor(api, appSettingsUri, appSettingsSessionStorageKey = undefined) {
        this.api = api;
        this.appSettingsUri = appSettingsUri;
        this.appSettingsSessionStorageKey = appSettingsSessionStorageKey ?? (0, session_storage_core_1.normalizeKey)(["trmrk", "app-settings"]);
    }
    async getSettings() {
        if (!this.initialized) {
            await this.loadSettings();
        }
        return this.settings;
    }
    async loadSettings() {
        const [hasValue, settings, hasKey] = await (0, session_storage_core_1.getOrCreateAsync)(this.appSettingsSessionStorageKey, () => this.refreshSettings());
        this.settings = settings;
        this.isSuccess ||= hasKey;
        return [hasValue, settings];
    }
    async refreshSettings() {
        this.reset();
        this.response = await this.api.get(this.appSettingsUri);
        this.isSuccess = this.response.isSuccess;
        if (this.isSuccess) {
            this.settings = this.response.resp?.data;
        }
        else {
            this.hasErr = true;
        }
        return [this.isSuccess, this.settings];
    }
    reset() {
        this.hasErr = false;
        this.isSuccess = false;
        this.settings = null;
        this.response = null;
    }
}
exports.AppSettingsServiceBase = AppSettingsServiceBase;
