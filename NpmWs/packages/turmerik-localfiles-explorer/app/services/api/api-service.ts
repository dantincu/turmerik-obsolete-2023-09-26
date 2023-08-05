import { obj } from "@turmerik/core"
import { axios } from "@turmerik/axios"

const Lazy = obj.Lazy

const instance = new Lazy<axios.ApiComponent>(
  () => new axios.ApiComponent(process.env.NEXT_PUBLIC_TURMERIK_LOCALFILES_EXPLORER_API_HOST as string),
)

export const getInstance = () => instance.value
