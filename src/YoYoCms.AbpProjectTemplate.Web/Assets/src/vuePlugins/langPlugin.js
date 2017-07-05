/**
 * Created by huanghx on 2017/7/4.
 */

let appLocalizationSource = abp.localization.getSource('AbpProjectTemplate')
let lang = {
    L: function () {
        return appLocalizationSource.apply(this, arguments)
    }
}
window.lang = lang

export default {
    install: function (Vue, options) {
        // 3. 注入组件
        Vue.mixin({
            methods: {
                L(...args) {
                    return lang.L(...args)
                }
            }
        })
    }
}