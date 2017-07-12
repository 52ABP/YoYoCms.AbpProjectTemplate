/**
 * Created by huanghx on 2017/7/12.
 */
// 权限插件
export default {
    install: function (Vue, options) {
        // 3. 注入组件
        Vue.mixin({
            methods: {
                HasP(...args) {
                    return abp.auth.isGranted(...args)
                }
            }
        })
    }
}