"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.setMobileWidthThreshold = exports.getMobileWidthThreshold = exports.useIsMobile = void 0;
const react_1 = require("react");
const IsSsrMobileContext_1 = require("./IsSsrMobileContext");
const useWindowSize_1 = require("./useWindowSize");
let mobileWidthThreshold = 992;
const useIsMobile = () => {
    const isSsrMobile = (0, react_1.useContext)(IsSsrMobileContext_1.IsSsrMobileContext);
    const { width: windowWidth } = (0, useWindowSize_1.useWindowSize)();
    const isBrowserMobile = !!windowWidth && windowWidth < mobileWidthThreshold;
    return isSsrMobile || isBrowserMobile;
};
exports.useIsMobile = useIsMobile;
const getMobileWidthThreshold = () => mobileWidthThreshold;
exports.getMobileWidthThreshold = getMobileWidthThreshold;
const setMobileWidthThreshold = (value) => (mobileWidthThreshold = value);
exports.setMobileWidthThreshold = setMobileWidthThreshold;
