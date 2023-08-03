"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SessionStorageService = void 0;
const session_storage_core_1 = require("./session-storage-core");
class SessionStorageService {
    appSettings;
    constructor(appSettings) {
        this.appSettings = appSettings;
    }
    getOrCreate(key, factory) {
        key.splice(0, 0, this.appSettings.TrmrkPrefix);
        return (0, session_storage_core_1.getOrCreate)(key, factory);
    }
    getOrCreateAsync(key, factory) {
        key.splice(0, 0, this.appSettings.TrmrkPrefix);
        return (0, session_storage_core_1.getOrCreateAsync)(key, factory);
    }
}
exports.SessionStorageService = SessionStorageService;
