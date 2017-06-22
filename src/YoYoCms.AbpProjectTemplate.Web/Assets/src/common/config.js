/**
 * Created by huanghx on 2017/6/21.
 */
class Config {
    constructor() {
        this.isDebug = process && process.isDebug
        this.apiHost = this.isDebug ? 'http://localhost:16240' : ''
    }
}

export default new Config()
