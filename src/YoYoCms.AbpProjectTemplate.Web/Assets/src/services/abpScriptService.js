/**
 * Created by huanghx on 2017/6/23.
 */

import apiHelper from './apiHelper'

class AbpScriptService {
    constructor() {
        this.isNeedLoad = true // 是否需要加载
    }

    getScripts() {
        return apiHelper.get(`/AbpScripts/GetScripts?v=${Date.now()}`)
    }
}

export default new AbpScriptService()