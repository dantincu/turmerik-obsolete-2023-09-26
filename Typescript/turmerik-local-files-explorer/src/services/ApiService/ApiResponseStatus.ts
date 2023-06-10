export interface ApiResponseStatus {
  isSuccessfull: boolean;

  isNotError: boolean | undefined;
  isInformational: boolean | undefined;
  isRedirection: boolean | undefined;

  isClientError: boolean | undefined;
  isServerError: boolean | undefined;

  isBadRequest: boolean | undefined;

  isUnauthorized: boolean | undefined;
  isProxyAuthenticationRequired: boolean | undefined;

  isForbidden: boolean | undefined;
  isResourceNotFound: boolean | undefined;
  isMethodNotAllowed: boolean | undefined;
  isNotAcceptable: boolean | undefined;

  isRequestTimeout: boolean | undefined;

  isConflict: boolean | undefined;
  isResourceGone: boolean | undefined;
  isLengthRequired: boolean | undefined;
  isPreconditionFailed: boolean | undefined;

  isPayloadTooLarge: boolean | undefined;
  isUriTooLong: boolean | undefined;

  isUnsupportedMediaType: boolean | undefined;
  isRangeNotSatisfiable: boolean | undefined;
  isExpectationFailed: boolean | undefined;
}

export const handleStatusCode = (
  responseStatus: ApiResponseStatus,
  status: number
) => {
  if (status <= 199) {
    responseStatus.isNotError = true;
    responseStatus.isInformational = true;
  } else if (status <= 299) {
    responseStatus.isSuccessfull = true;
  } else if (status <= 399) {
    responseStatus.isNotError = true;
    responseStatus.isRedirection = true;
  } else if (status <= 499) {
    responseStatus.isClientError = true;
    handleClientErrorStatusCode(responseStatus, status);
  } else if (status <= 599) {
    responseStatus.isServerError = true;
  }
};

export const handleClientErrorStatusCode = (
  responseStatus: ApiResponseStatus,
  status: number
) => {
  switch (status) {
    case 400:
      responseStatus.isBadRequest = true;
      break;
    case 401:
      responseStatus.isUnauthorized = true;
      break;
    case 403:
      responseStatus.isForbidden = true;
      break;
    case 404:
      responseStatus.isResourceNotFound = true;
      break;
    case 405:
      responseStatus.isMethodNotAllowed = true;
      break;
    case 406:
      responseStatus.isNotAcceptable = true;
      break;
    case 407:
      responseStatus.isProxyAuthenticationRequired = true;
      break;
    case 408:
      responseStatus.isRequestTimeout = true;
      break;
    case 409:
      responseStatus.isConflict = true;
      break;
    case 410:
      responseStatus.isResourceGone = true;
      break;
    case 411:
      responseStatus.isLengthRequired = true;
      break;
    case 412:
      responseStatus.isPreconditionFailed = true;
      break;
    case 413:
      responseStatus.isPayloadTooLarge = true;
      break;
    case 414:
      responseStatus.isUriTooLong = true;
      break;
    case 415:
      responseStatus.isUnsupportedMediaType = true;
      break;
    case 416:
      responseStatus.isRangeNotSatisfiable = true;
      break;
    case 417:
      responseStatus.isExpectationFailed = true;
      break;
    default:
      break;
  }
};

export const getApiResponseStatus = (status: number | null) => {
  const responseStatus = {
    isSuccessfull: false,
  } as ApiResponseStatus;

  if (typeof status === "number") {
    handleStatusCode(responseStatus, status);
  }

  return responseStatus;
};
