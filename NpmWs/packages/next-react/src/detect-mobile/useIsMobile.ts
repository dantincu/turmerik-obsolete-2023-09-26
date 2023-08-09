import { useContext } from "react"
import { IsSsrMobileContext } from "./IsSsrMobileContext"
import { useWindowSize } from "./useWindowSize"

let mobileWidthThreshold = 992
let mobileHeightToWidthThreshold = 1

export const useIsMobile = () => {
  const isSsrMobile = useContext(IsSsrMobileContext)
  const { width: windowWidth, height: windowHeight } = useWindowSize()

  const hasWidth = !!windowWidth
  const hasHeight = !!windowHeight

  let isBrowserMobile = hasWidth && windowWidth < mobileWidthThreshold

  if (hasWidth && mobileHeightToWidthThreshold) {
    isBrowserMobile ||= hasHeight && windowHeight > windowWidth * mobileHeightToWidthThreshold
  }

  return isSsrMobile || isBrowserMobile
}

export const getMobileWidthThreshold = () => mobileWidthThreshold
export const setMobileWidthThreshold = (value: number) => (mobileWidthThreshold = value)

export const getMobileHeightToWidthThreshold = () => mobileHeightToWidthThreshold
export const setMobileHeightToWidthThreshold = (value: number) => (mobileHeightToWidthThreshold = value)
