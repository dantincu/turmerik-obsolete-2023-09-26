"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.getIsSsrMobile = void 0;
const mobile_detect_1 = __importDefault(require("mobile-detect"));
const getIsSsrMobile = (context) => {
    const md = new mobile_detect_1.default(context.req.headers["user-agent"]);
    return Boolean(md.mobile());
};
exports.getIsSsrMobile = getIsSsrMobile;
