"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.getOrCreateAsync = exports.getOrCreate = exports.normalizeKey = void 0;
const normalizeKey = (key) => {
    if (typeof key !== "string") {
        const keyPartsArr = key;
        key = keyPartsArr.join("][");
        key = `[${key}]`;
    }
    return key;
};
exports.normalizeKey = normalizeKey;
const getOrCreate = (key, factory) => {
    key = (0, exports.normalizeKey)(key);
    let retVal;
    let json = sessionStorage.getItem(key);
    const hasKey = typeof json === "string";
    let hasVal = true;
    if (hasKey) {
        retVal = JSON.parse(json);
    }
    else {
        ;
        [hasVal, retVal] = factory();
        json = JSON.stringify(retVal);
        sessionStorage.setItem(key, json);
    }
    return [hasVal, retVal, hasKey];
};
exports.getOrCreate = getOrCreate;
const getOrCreateAsync = async (key, factory) => {
    key = (0, exports.normalizeKey)(key);
    let retVal;
    let json = sessionStorage.getItem(key);
    const hasKey = typeof json === "string";
    let hasVal = true;
    if (hasKey) {
        retVal = JSON.parse(json);
    }
    else {
        ;
        [hasVal, retVal] = await factory();
        if (hasVal) {
            json = JSON.stringify(retVal);
            sessionStorage.setItem(key, json);
        }
    }
    return [hasVal, retVal, hasKey];
};
exports.getOrCreateAsync = getOrCreateAsync;
