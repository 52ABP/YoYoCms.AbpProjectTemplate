/**
 * Created by huanghx on 2017/7/5.
 */

export default {
    install: function (Vue, options) {
        // 3. 注入组件
        Vue.mixin({
            mounted () {
                Waves.init()
            },
            // deactivated() {
            //     abp.setContentLoading(true)
            // },
            // beforeDestroy() {
            //     abp.setContentLoading(true)
            // }
        })
    }
}