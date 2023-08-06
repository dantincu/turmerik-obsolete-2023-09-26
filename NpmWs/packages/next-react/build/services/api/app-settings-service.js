"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.registerInstance = exports.getInstance = void 0;
let instance = null;
const getInstance = () => instance;
exports.getInstance = getInstance;
const registerInstance = (instn) => {
    instance = instn;
};
exports.registerInstance = registerInstance;
