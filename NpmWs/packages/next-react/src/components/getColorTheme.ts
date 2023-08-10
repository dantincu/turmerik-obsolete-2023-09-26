export const basicThemes = Object.freeze(["light", "dark"])

export const getSystemColorThemeCssClass = () => {
  const idx = window.matchMedia("(prefers-color-scheme: dark)").matches ? 1 : 0

  const themeClass = basicThemes[idx]
  return themeClass
}

export const getColorThemeCssClass = (colorTheme: string | null = null) => {
  let themeClass: string

  if (typeof colorTheme === "string" && basicThemes.indexOf(colorTheme) >= 0) {
    themeClass = colorTheme
  } else if (typeof window !== "undefined") {
    themeClass = getSystemColorThemeCssClass()
  } else {
    themeClass = ""
  }

  return themeClass
}
