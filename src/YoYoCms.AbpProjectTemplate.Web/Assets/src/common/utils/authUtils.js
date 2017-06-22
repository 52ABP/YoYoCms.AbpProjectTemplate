/**
 * Created by huanghx on 2017/6/22.
 */
const TOKENKEY = 'abp.token.key'
class AuthUtils {
    // 设置token
    setToken(token) {
        let tokenObj = {
            token,
            expireTime: Date.now() + 60 * 60 * 2 * 1e3 // 2小时过期
        }
        localStorage.setItem(TOKENKEY, JSON.stringify(tokenObj))
    }

    // 获取token
    getToken() {
        let tokenStr = localStorage.getItem(TOKENKEY)
        if (!tokenStr) return null
        let tokenObj = JSON.parse(tokenStr)
        if (tokenObj.expireTime >= Date.now()) return null

        return tokenObj.token
    }
}

export default new AuthUtils()