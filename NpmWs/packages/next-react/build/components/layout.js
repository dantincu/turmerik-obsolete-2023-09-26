"use strict";
"use client"; // This is a client component 👈🏽
Object.defineProperty(exports, "__esModule", { value: true });
const jsx_runtime_1 = require("react/jsx-runtime");
const react_1 = require("react");
const react_error_boundary_1 = require("react-error-boundary");
const app_settings_service_1 = require("../services/api/app-settings-service");
function RootLayout({ children, props }) {
    const [appSettings, setAppSettings] = (0, react_1.useState)();
    const [appSettingsResponse, setAppSettingsResponse] = (0, react_1.useState)();
    const [hasAppSettingsResponse, setHasAppSettingsResponse] = (0, react_1.useState)(false);
    const [hasAppSettings, setHasAppSettings] = (0, react_1.useState)(false);
    const appSettingsService = (0, app_settings_service_1.getInstance)();
    const getAppSettings = (0, react_1.useCallback)(() => appSettingsService.getSettings().then(settings => {
        setHasAppSettings(appSettingsService.isSuccess);
        if (appSettingsService.isSuccess) {
            setAppSettings(settings);
        }
        else {
            setAppSettingsResponse(appSettingsService.response);
        }
        setHasAppSettingsResponse(true);
    }), [appSettingsService]);
    (0, react_1.useEffect)(() => {
        getAppSettings();
    }, [appSettings]);
    if (hasAppSettingsResponse) {
        if (hasAppSettings) {
            return ((0, jsx_runtime_1.jsx)("html", { lang: "en", children: (0, jsx_runtime_1.jsx)("body", { className: props.bodyClassName, children: (0, jsx_runtime_1.jsx)(react_error_boundary_1.ErrorBoundary, { fallback: (0, jsx_runtime_1.jsx)("div", { children: "Something went wrong" }), children: (0, jsx_runtime_1.jsx)("div", { className: "trmrk-app", children: children }) }) }) }));
        }
        else {
            return ((0, jsx_runtime_1.jsx)("html", { lang: "en", children: (0, jsx_runtime_1.jsx)("body", { className: props.bodyClassName, children: (0, jsx_runtime_1.jsx)("div", { className: "trmrk-app-load-error", children: (0, jsx_runtime_1.jsx)("main", { className: "flex min-h-screen flex-col items-center justify-between p-24", children: appSettingsResponse?.resp ? ((0, jsx_runtime_1.jsxs)("div", { children: [(0, jsx_runtime_1.jsxs)("h1", { children: ["Error while loading the app - ", appSettingsResponse.resp.status] }), " - ", appSettingsResponse.resp.statusText] })) : (appSettingsResponse?.err ? ((0, jsx_runtime_1.jsxs)("div", { children: [(0, jsx_runtime_1.jsx)("h1", { children: "Error while loading the app" }), " ", appSettingsResponse.err.message] })) : ((0, jsx_runtime_1.jsx)("div", { children: "Oops... something went wrong and the app could not be loaded" }))) }) }) }) }));
        }
    }
    else {
        return ((0, jsx_runtime_1.jsx)("html", { lang: "en", children: (0, jsx_runtime_1.jsx)("body", { className: props.bodyClassName, children: (0, jsx_runtime_1.jsx)("div", { className: "trmrk-app-loading", children: (0, jsx_runtime_1.jsx)("main", { className: "flex min-h-screen flex-col items-center justify-between p-24", children: (0, jsx_runtime_1.jsx)("h1", { children: "Loading..." }) }) }) }) }));
    }
}
exports.default = RootLayout;
