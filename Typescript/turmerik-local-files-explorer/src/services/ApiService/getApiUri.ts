import { ApiUri } from "./ApiUri";

const getAppSettingsUriStr = () => {
  let appSettingsUriStr: string;

  if (process.env.NODE_ENV === "production") {
    appSettingsUriStr = process.env
      .REACT_APP_TURMERIK_FILE_EXPLORER_APP_SETTINGS_URI as string;
  } else {
    appSettingsUriStr = process.env
      .REACT_APP_DEV_TURMERIK_FILE_EXPLORER_APP_SETTINGS_URI as string;
  }

  return appSettingsUriStr;
};

export const getApiUri = () => {
  const appSettingsUriStr = getAppSettingsUriStr();
  const addr = new URL(appSettingsUriStr);

  const host = addr.host;
  const path = addr.pathname;

  const pathParts = path.split("/");
  const apiUri = pathParts.splice(0, 1).join("/");
  const apiBaseUri = [host, apiUri].join("/");
  const appSettingsRelUri = pathParts.join("/");

  const appSettingsUri: ApiUri = {
    apiHost: host,
    apiBasePath: apiUri,
    apiBaseUri: apiBaseUri,
    appSettingsRelUri: appSettingsRelUri,
  };

  return appSettingsUri;
};
