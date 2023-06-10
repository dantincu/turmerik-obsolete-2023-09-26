import { ApiUri } from "./ApiUri";
import { getApiUri } from "./getApiUri";

export class ApiUriRetriever {
  private static _instance: ApiUriRetriever;

  private _apiUri: ApiUri;

  private constructor() {
    const apiUri = getApiUri();
    this._apiUri = Object.freeze(apiUri);
  }

  public static get instance(): ApiUriRetriever {
    if (!ApiUriRetriever._instance) {
      ApiUriRetriever._instance = new ApiUriRetriever();
    }

    return ApiUriRetriever._instance;
  }

  public get apiUri() {
    return this._apiUri;
  }
}
