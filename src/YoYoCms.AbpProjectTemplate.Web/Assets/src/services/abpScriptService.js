/**
 * Created by huanghx on 2017/6/23.
 */

import apiHelper from './apiHelper'

class AbpScriptService {
    getScripts() {
        return apiHelper.get(`/AbpScripts/GetScripts?v=${Date.now()}`)
    }
}

export default new AbpScriptService()