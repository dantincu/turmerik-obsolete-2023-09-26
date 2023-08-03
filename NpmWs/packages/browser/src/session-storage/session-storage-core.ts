export const normalizeKey = (key: string | string[]) => {
  if (typeof key !== "string") {
    const keyPartsArr = key as string[]
    key = keyPartsArr.join("][")
    key = `[${key}]`
  }

  return key
}

export const getOrCreate = <T>(key: string[] | string, factory: () => [boolean, T]): [boolean, T, boolean] => {
  key = normalizeKey(key)
  let retVal: T
  let json: string = sessionStorage.getItem(key) as string
  const hasKey = typeof json === "string"
  let hasVal = true

  if (hasKey) {
    retVal = JSON.parse(json)
  } else {
    ;[hasVal, retVal] = factory()
    json = JSON.stringify(retVal)
    sessionStorage.setItem(key, json)
  }

  return [hasVal, retVal, hasKey]
}

export const getOrCreateAsync = async <T>(
  key: string[] | string,
  factory: () => Promise<[boolean, T]>,
): Promise<[boolean, T, boolean]> => {
  key = normalizeKey(key)
  let retVal: T
  let json: string = sessionStorage.getItem(key) as string
  const hasKey = typeof json === "string"
  let hasVal = true

  if (hasKey) {
    retVal = JSON.parse(json)
  } else {
    ;[hasVal, retVal] = await factory()

    if (hasVal) {
      json = JSON.stringify(retVal)
      sessionStorage.setItem(key, json)
    }
  }

  return [hasVal, retVal, hasKey]
}
