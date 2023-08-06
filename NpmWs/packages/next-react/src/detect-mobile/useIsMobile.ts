import { useContext } from "react"
import { IsSsrMobileContext } from "./IsSsrMobileContext"
import { useWindowSize } from "./useWindowSize"

let mobileWidthThreshold = 992

export const useIsMobile = () => {
  const isSsrMobile = useContext(IsSsrMobileContext)
  const { width: windowWidth } = useWindowSize()
  const isBrowserMobile = !!windowWidth && windowWidth < mobileWidthThreshold

  return isSsrMobile || isBrowserMobile
}

export const getMobileWidthThreshold = () => mobileWidthThreshold
export const setMobileWidthThreshold = (value: number) => (mobileWidthThreshold = value)
