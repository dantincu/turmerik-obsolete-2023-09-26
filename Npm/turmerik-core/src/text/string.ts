export const trimStart = (input: string, str: string) => {
  const len = str.length

  while (input.startsWith(str)) {
    input = input.substring(len)
  }

  return input
}

export const trimEnd = (input: string, str: string) => {
  const len = str.length

  while (input.endsWith(str)) {
    input = input.substring(0, input.length - len + 1)
  }

  return input
}

export const trim = (input: string, str: string) => {
  input = trimStart(input, str)
  input = trimEnd(input, str)

  return input
}
