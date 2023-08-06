import { axios } from "@turmerik/axios"

let instance: axios.ApiComponent = null as unknown as axios.ApiComponent
export const getInstance = () => instance

export const registerInstance = (instn: axios.ApiComponent) => {
  instance = instn
}
