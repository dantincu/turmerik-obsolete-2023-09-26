import { trim } from "../text/string"

export const absoluteUriRegex = /^[a-zA-Z0-9_\-+.]+:\/{2}[a-zA-Z0-9_\-+.]+(:[0-9]+)?\/?/

export const getHostPathHashAndQueryFromUri = (uri: string) => {
  const match = uri.match(absoluteUriRegex)
  let host: string | undefined
  let path: string | undefined

  let hash: string | undefined
  let query: string | undefined

  if (match) {
    host = match[0]
    uri = uri.substring(host.length)
  }

  ;[path, hash] = uri.split("#")

  if (typeof hash === "string") {
    ;[hash, query] = hash.split("?")
  } else {
    ;[path, query] = uri.split("?")
  }

  return [host, path, hash, query]
}

export const parseUri = (uri: string) => {
  const parsed = {} as ParsedUri
  const [host, path, hash, query] = getHostPathHashAndQueryFromUri(uri)

  if (typeof host === "string") {
    const [scheme, hostNameAndPort] = host.split("://")
    const [hostName, port] = hostNameAndPort.split(":")

    parsed.scheme = scheme
    parsed.hostName = hostName
    parsed.port = port?.split("/")[0]
  }

  if (typeof path === "string") {
    parsed.pathParts = path.split("/").filter((part) => part.length > 0)
  }

  parsed.hash = hash

  if (typeof query === "string") {
    const queryObj = new Map<string, string>()

    const queryParts = query
      .split("&")
      .filter((part) => part.length > 0)
      .map((queryParamPair) => queryParamPair.split("="))

    for (const queryParamPair of queryParts) {
      const [key, value] = queryParamPair

      if (key.length > 0) {
        queryObj.set(key, value)
      }
    }

    parsed.query = queryObj
  }

  return parsed
}

export const uriToString = (parsed: ParsedUri) => {
  let uri = ""

  if (typeof parsed.hostName == "string") {
    uri = parsed.hostName

    if (["string", "number"].indexOf(typeof parsed.port) >= 0) {
      uri = [uri, parsed.port].join(":")
    }

    if (typeof parsed.scheme == "string") {
      uri = [parsed.scheme, uri].join("://")
    }
  }

  if (parsed.pathParts) {
    const path = parsed.pathParts.join("/")
    uri = [uri, path].join("/")
    uri = trim(uri, "/")
  }

  if (typeof parsed.hash === "string") {
    uri = [uri, parsed.hash].join("#")
  }

  if (parsed.query) {
    const queryStr = queryToString(parsed.query)
    uri = [uri, queryStr].join("?")
  }

  return uri
}

export const queryToString = (query: Map<string, string>, prependQuestionMark = false) => {
  const queryStrParts: string[] = []

  for (const [key, value] of query) {
    const str = `${key}=${value}`
    queryStrParts.push(str)
  }

  let retStr = queryStrParts.join("&")

  if (prependQuestionMark) {
    retStr = "?" + retStr
  }

  return retStr
}

export interface ParsedUri {
  scheme: string | undefined
  hostName: string | undefined
  port: string | undefined
  pathParts: string[] | undefined
  hash: string | undefined
  query: Map<string, string> | undefined
}
