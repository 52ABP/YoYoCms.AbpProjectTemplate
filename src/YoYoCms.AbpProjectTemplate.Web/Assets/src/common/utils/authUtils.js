/**
 * Created by huanghx on 2017/6/22.
 */
const TOKENKEY = 'abp.token.key'
const USERKEY = 'abp.userinfo'
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
        // 如果获取不到
        if (!tokenStr) return null
        let tokenObj = JSON.parse(tokenStr)
        // 如果过期
        if (tokenObj.expireTime <= Date.now()) return null

        return tokenObj.token
    }

    // 设置和获取用户信息
    getUserInfo() {
        let userStr = localStorage.getItem(USERKEY)
        if (!userStr) return null
        return JSON.parse(userStr)
    }

    setUserInfo(user) {
        localStorage.setItem(USERKEY, JSON.stringify(user))
    }
}

export default new AuthUtils()