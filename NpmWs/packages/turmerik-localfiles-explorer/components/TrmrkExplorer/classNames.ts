export const getPaneCssClassName = (paneName: string, isVisible: boolean) =>
  ["w-1/3", "h-full", "overflow-scroll", `trmrk-${paneName}-pane`, isVisible ? "" : "hidden"].join(" ")
