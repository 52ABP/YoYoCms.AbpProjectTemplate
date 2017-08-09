/**
 * Created by huanghx on 2017/6/21.
 */
class Config {
    constructor() {
        this.isDebug = process && process.isDebug
        this.apiHost = this.isDebug ? '//localhost:16240' : ''
        this.apiHostName = this.apiHost.replace('//', '')
        this.showPageTab = false // 是否增加tab页
    }
}

export default new Config()
