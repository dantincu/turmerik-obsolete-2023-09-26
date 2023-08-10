import type { NextRequest, NextResponse } from "next/server"
import type { NextURL } from "next/dist/server/web/next-url"
import { AxiosRequestConfig } from "axios"

import { uri, obj } from "@turmerik/core"
import { axios } from "@turmerik/axios"

const apiComponent = new axios.ApiComponent(
  [process.env.NEXT_PUBLIC_TRMRK_FS_EXPLORER_API_HOST as string, "api"].join("/"),
)

const apiPathKey = "api_path"

const getApiUri = (url: string) => {
  const parsedUri = uri.parseUri(url)
  parsedUri.hostName = undefined

  const query = parsedUri.query!
  const apiPath = query.get(apiPathKey)!

  query.delete(apiPathKey)

  if (query.size === 0) {
    parsedUri.query = undefined
  }

  parsedUri.pathParts = apiPath.split("/").filter((part) => part.length > 0)
  const apiUri = uri.uriToString(parsedUri)

  return apiUri
}

const handleResponse = <T, D>(method: string, res: any, response: axios.ApiResponse<T, D>) => {
  console.log("response", response)
  if (response.isSuccess) {
    res.status(200)
  } else if (response.resp) {
    res.status(response.resp.status)
  } else {
    res.status(500, response.err?.message ?? "Internal Server Error")
  }

  const data = response.resp?.data

  if (!obj.isNullOrUndef(data)) {
    res.json(data)
  }

  res.send()
}

export default function handler(req: any, res: any) {
  return new Promise<void>((resolve, reject) => {
    try {
      const apiUri = getApiUri(req.url)
      const body = req.body ?? ""

      let data: any | undefined = undefined

      if (body.length > 0) {
        data = JSON.parse(body)
      }

      let resProm: Promise<axios.ApiResponse<unknown, unknown>> | null = null

      switch (req.method) {
        case "GET":
          resProm = apiComponent.get(apiUri)
          break
        case "DELETE":
          resProm = apiComponent.delete(apiUri)
          break
        case "POST":
          resProm = apiComponent.post(apiUri, data)
          break
        case "PUT":
          resProm = apiComponent.put(apiUri, data)
          break
        case "PATCH":
          resProm = apiComponent.delete(apiUri)
          break
      }

      if (resProm) {
        resProm
          .then((response) => {
            try {
              handleResponse(req.method, res, response)
              res.send()
              resolve()
            } catch (error) {
              reject(error)
            }
          })
          .catch((reason) => {
            reject(reason)
          })
      } else {
        res.status(405, "Method not allowed")
        res.send()
        resolve()
      }
    } catch (error) {
      reject(error)
    }
  })
}
