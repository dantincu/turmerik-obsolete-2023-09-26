import { sso } from "node-expose-sspi"

const ssoAuth = sso.auth()

export default function handler(req: any, res: any) {
  return new Promise<void>((resolve, reject) => {
    try {
      return ssoAuth(req, res, () => {
        res.json({ sso: req.sso })
        resolve()
      })
    } catch (error) {
      reject(error)
    }
  })
}
