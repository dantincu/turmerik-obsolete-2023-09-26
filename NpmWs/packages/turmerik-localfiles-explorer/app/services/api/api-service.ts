import { obj } from "@turmerik/core"
import { axios_proxy } from "@turmerik/axios"

const Lazy = obj.Lazy

const instance = new Lazy<axios_proxy.ProxyApiComponent>(
  () => new axios_proxy.ProxyApiComponent(process.env.NEXT_PUBLIC_TURMERIK_LOCALFILES_EXPLORER_PROXY_API_HOST ?? ""),
)

export const getInstance = () => instance.value
