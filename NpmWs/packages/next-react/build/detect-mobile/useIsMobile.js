"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.useIsMobile = void 0;
const react_1 = require("react");
const IsSsrMobileContext_1 = require("./IsSsrMobileContext");
const useWindowSize_1 = require("./useWindowSize");
const useIsMobile = () => {
    const isSsrMobile = (0, react_1.useContext)(IsSsrMobileContext_1.IsSsrMobileContext);
    const { width: windowWidth } = (0, useWindowSize_1.useWindowSize)();
    const isBrowserMobile = !!windowWidth && windowWidth < 992;
    return isSsrMobile || isBrowserMobile;
};
exports.useIsMobile = useIsMobile;
