import * as turmerik_core from "./index"
const turmerik_core_uri = turmerik_core.uri
const parseUri = turmerik_core_uri.parseUri
const uriToString = turmerik_core_uri.uriToString

const test = (uri: string) => {
  const parsed = parseUri(uri)
  const srlzd = uriToString(parsed)

  console.log(uri, srlzd, parsed)

  const reParsed = parseUri(srlzd)
  const reSrlzd = uriToString(reParsed)

  if (reSrlzd !== srlzd) {
    console.error("ERROR: ", srlzd, reSrlzd)
    throw ["ERROR", srlzd, reSrlzd].join(",")
  }
}

test("https://asdfasdf.com:9000/asdfasd#qewrwe?zxcvzxcv=123&iuytui=876")
test("https://asdfasdf.com/qewrwe?zxcvzxcv=123&iuytui=876")
test("https://asdfasdf.com:9000/asdfasd#?zxcvzxcv=123&iuytui=876")
test("https://asdfasdf.com/asdfasd#qewrwe?")
test("https://asdfasdf.com:9000/?zxcvzxcv=123&iuytui=876")
test("https://asdfasdf.com/?")
test("https://asdfasdf.com:9000/asdfasd#qewrwe")
test("https://asdfasdf.com/asdfasd#qewrwe#")
test("https://asdfasdf.com:9000/asdfasd")
test("https://asdfasdf.com/#qewrwe")

test("/asdfasd#qewrwe?zxcvzxcv=123&iuytui=876")
test("qewrwe?zxcvzxcv=123&iuytui=876")
test("/asdfasd#?zxcvzxcv=123&iuytui=876")
test("asdfasd#qewrwe?")
test("?zxcvzxcv=123&iuytui=876")
test("/?")
test("asdfasd#qewrwe")
test("/asdfasd#qewrwe#")
test("asdfasd")
test("/#qewrwe")
