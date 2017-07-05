/**
 * Created by huanghx on 2017/6/26.
 */
//
import serviceHelper from '../serviceHelper'
import apiHelper from '../apiHelper'
let language = serviceHelper.requireService('language')

language.changeLanguage = function (lang) {
    return apiHelper.get(`/AbpLocalization/ChangeCulture?cultureName=${lang}&returnUrl=`)
}

export default language